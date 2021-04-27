using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Imaginarium.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Imaginarium.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Imaginarium.Hubs
{
    public class GameHub : Hub
    {
        private readonly ImaginariumContext _dbContext;
        private readonly IHostingEnvironment _hostEnvironment;

        static Random rnd = new Random();

        public GameHub(
            ILoggerFactory loggerFactory,
            ImaginariumContext dbContext,
            IHostingEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation($"{Context.ConnectionId} вошел в чат");
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CheckGameStatus(int gameId, string username)
        {
            var game = await _dbContext.Games
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId && x.Users.Any(u => u.Name == username));

            if (game.Status == StatusType.Active || game.Status == StatusType.New)
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());

            var isCreator = username == game.Creator;

            await Clients.Caller.SendAsync("CheckGameStatus", gameId, username, game.Status, isCreator );
        }

        public async Task GetCardSets(bool isSuperUser)
        {
            var cardSets = await _dbContext.CardSets
                    .ToListAsync();

            if (!isSuperUser)
            {
                cardSets = cardSets
                    .Where(x => x.IsAvailable)
                    .ToList();
            }

            var result = cardSets
                    .GroupBy(s => s.Group)
            .Select(g => new
            {
                GroupName = g.Key,
                Items = g.Select(x => new
                {
                    x.Id,
                    Value = x.NameRus
                })
            })
            .ToList();

            await Clients.Caller.SendAsync("GetCardSets", result);
        }

        public async Task CreateGame(string username, List<int> selectedCardSet)
        {
            try
            {
                var game = new Game();
                game.AddUser(username, GenerateChipId(game.Users), game.Users.Count);
                game.ActivePlayerName = username;
                game.Creator = username;
                _dbContext.Games.Add(game);

                var cardSets = await _dbContext.CardSets
                    .Where(x => selectedCardSet.Contains(x.Id))
                    .ToListAsync();

                foreach (var set in cardSets)
                {
                    _dbContext.CardSetGames.Add(new CardSetGame()
                    {
                        GameId = game.Id,
                        CardSetId = set.Id
                    });
                }

                var qqx = Context.Items;

                await _dbContext.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());

                await Clients.Caller.SendAsync("CreateGame", game.Id);
            }
            catch(Exception ex)
            {
                await Clients.Caller.SendAsync("CreateGame", null, ex);
            }

        }

        public async Task JoinGame(string username, int gameId)
        {
            var game = _dbContext.Games
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == gameId && x.Status == StatusType.New);

            if (game == null)
            {
                await Clients.Caller.SendAsync("JoinGame", gameId, false, $"Игра {gameId} не найдена");
                return;
            }

            if (game.Users.Any(u => u.Name == username))
            {
                await Clients.Caller.SendAsync("JoinGame", gameId, false, $"Игрок {username} уже присоединился к игре. Выберите другое имя");
                return;
            }

            game.AddUser(username, GenerateChipId(game.Users), game.Users.Count);
            _dbContext.Games.Update(game);
            _dbContext.SaveChanges();

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());

            await Clients.Caller.SendAsync("JoinGame", gameId, true, null);
            await Clients.Group(game.Id.ToString()).SendAsync("JoinGame", gameId, null, null);
        }

        public async Task StartGame(int gameId, User[] orderedUserList)
        {
            var game = _dbContext.Games
                .Include(g => g.Users)
                .FirstOrDefault(x => x.Id == gameId);

            game.Status = StatusType.Active;
            game.Round = 0;

            game.Users.ForEach(user =>
            {
                user.Order = orderedUserList.FirstOrDefault(u => u.Name == user.Name).Order;
                _dbContext.Entry(user).State = EntityState.Modified;
            });

            game.ActivePlayerName = game.Users.OrderBy(u => u.Order).First().Name;

            _dbContext.Games.Update(game);
            _dbContext.SaveChanges();

            var userCount = _dbContext.Games
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == gameId)?.Users.Count();

            var message = $"Игра начинается. Количество игроков: {userCount}";
            await Clients.Group(game.Id.ToString()).SendAsync("StartGame", game.Id, message);
        }

        public async Task GetUsers(int gameId)
        {
            var usersList = await _dbContext.Users
                .Where(x => x.GameId == gameId)
                .OrderBy(x => x.Order)
                .Select(x => new { x.Name, x.ChipId, x.Order })
                .ToListAsync();

            await Clients.Group(gameId.ToString()).SendAsync("GetUsers", usersList);
        }

        public async Task SelectCard(string username, int gameId, int playCardId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var playCard = await _dbContext.PlayCards
                .FirstOrDefaultAsync(x => x.Id == playCardId && x.GameId == gameId);

            playCard.Round = game.Round;
            _dbContext.PlayCards.Update(playCard);
            await _dbContext.SaveChangesAsync();

            var roundCards = await _dbContext.PlayCards
                .Where(x => x.Round == game.Round && x.GameId == gameId)
                .ToListAsync();

            var isAllCardsSended = game.Users.Count <= roundCards.Count; // todo: test <=

            //if (game.ActivePlayerName == username) // голос ведущего сразу учитывается в голосовании, чтобы он мог не голосовать за свою карту (для доработки: ведущий не должен голосовать за свою карту)
            //{
            //    playCard.VotedUsers.Add(username);
            //    _dbContext.PlayCards.Update(playCard);

            //    game.VotedOnRoundCount++;
            //    _dbContext.Games.Update(game);

            //    await _dbContext.SaveChangesAsync();
            //}

            await Clients.Group(game.Id.ToString()).SendAsync("SelectCard", isAllCardsSended, username);
        }

        public async Task VoteForCard(string username, int gameId, int playCardId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var playCard = await _dbContext.PlayCards
                .FirstOrDefaultAsync(x => x.Id == playCardId && x.GameId == gameId);

            if (game.ActivePlayerName != username && playCard.Username == username) // не ведущий проголосовал за свою карту
            {
                await Clients.Caller.SendAsync("VoteForCard", false, username, "Ты не можешь голосовать за свою карту");
                return;
            }

            if (game.ActivePlayerName == username && playCard.Username != username) // ведущий проголосовал за не свою карту
            {
                await Clients.Caller.SendAsync("VoteForCard", false, username, "Ты ведущий. Выбери свою карту");
                return;
            }

            var votedPlayCard = await _dbContext.PlayCards
                .Where(x => x.Round == game.Round && x.GameId == gameId)
                .FirstOrDefaultAsync(x => x.VotedUsers.Contains(username));

            if (votedPlayCard != null)
            {
                var voteToRemove = votedPlayCard.VotedUsers.SingleOrDefault(r => r == username);
                if (voteToRemove != null)
                {
                    votedPlayCard.VotedUsers.Remove(voteToRemove);
                }
                game.VotedOnRoundCount--;
            }

            playCard.VotedUsers.Add(username);
            _dbContext.PlayCards.Update(playCard);

            game.VotedOnRoundCount++;
            _dbContext.Games.Update(game);

            await _dbContext.SaveChangesAsync();

            var isAllCardsSended = game.Users.Count == game.VotedOnRoundCount;

            await Clients.Group(game.Id.ToString()).SendAsync("VoteForCard", isAllCardsSended, username, null);
        }

        public async Task GetRoundResults(int gameId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var roundCards = _dbContext.PlayCards
                .Include(x => x.Card)
                .Where(x => x.Round == game.Round && x.GameId == gameId);

            Logger.LogInformation("");

            var activePlayCard = roundCards
                .Where(x => x.Username == game.ActivePlayerName)
                .FirstOrDefault();

            var players = game.Users.Where(u => activePlayCard.VotedUsers.Any(v => v == u.Name))
                .Select(x => new UserResultViewModel {
                    Name = x.Name,
                    ChipId = x.ChipId,
                    IsCardOwner = activePlayCard.Username == x.Name
                })
                .ToList();

            var results = new RoundResultsViewModel()
            {
                ActivePlayCard = new PlayCardResultsViewModel()
                {
                    Id = activePlayCard.Id,
                    NumberInSet = activePlayCard.Card.NumberInSet,
                    Src = activePlayCard.Card.Src,
                    Players = players
                },
                ResultsList = new List<PlayerResults>()
            };
            foreach (var user in game.Users)
            {
                var points = await GetPointsForUserForRound(user.Name, gameId, roundCards.ToList());

                user.Points += points;
                if (user.Points < 1) // первая клетка поля = 1
                {
                    user.Points = 1;
                }

                results.ResultsList.Add(new PlayerResults()
                {
                    Username = user.Name,
                    RoundPoints = points,
                    TotalPoints = user.Points,
                    ChipId = user.ChipId
                });

            }

            results.ResultsList = results.ResultsList
                .OrderByDescending(r => r.RoundPoints)
                .ToList();

            var currentRoundCards = _dbContext.PlayCards
                .Where(x => x.Round == game.Round && x.GameId == gameId);

            foreach (var item in currentRoundCards)
            {
                item.Used = true;
                _dbContext.PlayCards.Update(item);
            }

            if (game.VotedOnRoundCount != 0) // если еще не сбросили. чтобы не инкрементить для каждого игрока
            {
                game.Round++;
                game.VotedOnRoundCount = 0;
                Logger.LogInformation($"Определение ведущего для игроков: {JsonConvert.SerializeObject(game.Users.Select(x => x.Name).ToList())}");
                var activePlayerNumber = (game.Round % game.Users.Count).Value;
                game.ActivePlayerName = game.Users.OrderBy(x => x.Order).ToList()[activePlayerNumber].Name;
                //var activePlayerIndex = game.Users.FindIndex(x => x.Name == game.ActivePlayerName);
                //var nextActivePlayer = game.Users[activePlayerIndex++] ?? game.Users[0];
                Logger.LogInformation($"Ведущий - {game.ActivePlayerName}(номер ведущего в списке - {activePlayerNumber}, раунд - {game.Round}, количество игроков - {game.Users.Count}");
                //Logger.LogInformation($" сл ведущий - {nextActivePlayer.Name}");
            }
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();

            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
            await Clients.Group(game.Id.ToString()).SendAsync("GetRoundResults", results);
        }

        public async Task GetCards(string username, int gameId, RoundType type)
        {
            var result = new List<PlayCardViewModel>();

            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            if (type == RoundType.Choice)
            {
                result = await GetUserCards(username, gameId);
            }

            if (type == RoundType.Guessing)
            {
                result = await GetRoundCards(gameId);
            }

            game.RoundType = type;
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();

            await Clients.Caller.SendAsync("GetCards", result, game.ActivePlayerName);
        }

        public async Task LeaveGame(string username, int gameId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var userForRemove = game.Users
                .FirstOrDefault(x => x.Name == username);

            var needNewRound = false;
            var needRoundResults = false;

            if (userForRemove != null)
            {
                game.Users.Remove(userForRemove);
            }

            if (game.Users.Count == 0)
            {
                game.Status = StatusType.Ended;
            }
            else if (game.RoundType == RoundType.Guessing)
            {
                needRoundResults = true;
                // ведущий - раунд продолжается как обычно, голос ведущего добавляется, если еще не был учтен
                // не ведущий - раунд продолжается как обычно, голос игрока отдается за свою карточку
                var playCard = await _dbContext.PlayCards
                    .FirstOrDefaultAsync(x => x.GameId == gameId && x.Username == userForRemove.Name);

                if (playCard != null && !playCard.VotedUsers.Any(u => u == userForRemove.Name))
                {
                    playCard.VotedUsers.Add(userForRemove.Name);
                    _dbContext.PlayCards.Update(playCard);

                    game.VotedOnRoundCount++;
                    _dbContext.Games.Update(game);

                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (game.RoundType == RoundType.Choice)
            {
                if (game.ActivePlayerName == userForRemove.Name)
                {
                    // ведущий - сбрасываем раунд, карточки возвращаются к игрокам
                    needNewRound = true;
                    var roundPlayCards = _dbContext.PlayCards
                        .Where(c => c.GameId == game.Id && c.Round == game.Round)
                        .ToList();

                    foreach (var card in roundPlayCards)
                    {
                        card.Round = null;
                        _dbContext.Entry(card).State = EntityState.Modified;
                    }
                    _dbContext.SaveChanges();

                    var activePlayerNumber = (game.Round % game.Users.Count).Value;
                    game.ActivePlayerName = game.Users.OrderBy(x => x.Order).ToList()[activePlayerNumber].Name;
                }
                else
                {
                    // не ведущий - карточка игрока отбрасывается, раунд продолжается
                    var userRoundPlayCard = _dbContext.PlayCards
                        .Where(c => c.GameId == game.Id && c.Round == game.Round)
                        .FirstOrDefault(c => c.Username == userForRemove.Name);

                    if (userRoundPlayCard != null)
                    {
                        userRoundPlayCard.Round = null;
                        _dbContext.PlayCards.Update(userRoundPlayCard);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }

            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, game.Id.ToString());

            await Clients.Caller.SendAsync("LeaveGame", true);

            var isAllCardsSended = game.RoundType == RoundType.Choice ?
                game.Users.Count <= game.PlayCards.Where(c => c.Round == game.Round).Count() :
                game.Users.Count <= game.VotedOnRoundCount;

            await Clients.Group(game.Id.ToString()).SendAsync("SomeoneLeaveGame", isAllCardsSended, game.RoundType, needNewRound, needRoundResults);
        }

        private async Task<List<PlayCardViewModel>> GetRoundCards(int gameId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var roundCards = await _dbContext.PlayCards
                .Include(x => x.Card)
                .Where(x => x.Round == game.Round && x.GameId == gameId)
                .Select(x => new PlayCardViewModel()
                {
                    Id = x.Id,
                    Src = x.Card.Src,
                    NumberInSet = x.Card.NumberInSet
                })
                .ToListAsync();

            return roundCards;
        }

        private async Task<List<PlayCardViewModel>> GetUserCards(string username, int gameId)
        {
            var userCards = _dbContext.PlayCards
                .Include(x => x.Card)
                .Where(card => card.Username == username && card.GameId == gameId && !card.Used);

            while (userCards.Count() < 7)
            {
                var newPlayCard = await GenerateCard(username, gameId);
                _dbContext.PlayCards.Add(newPlayCard);
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _dbContext.Entry(newPlayCard).State = EntityState.Detached;
                    Logger.LogError($"Не удалось добавить карту для пользователя: {ex.Message}");
                }
            }

            var result = userCards
                .Select(x => new PlayCardViewModel()
                {
                    Id = x.Id,
                    Src = x.Card.Src,
                    NumberInSet = x.Card.NumberInSet
                })
                .ToList();

            return result;
        }

        private async Task<PlayCard> GenerateCard(string username, int gameId)
        {
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.CardSetGames)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            var selectedCardSetsIds = game.CardSetGames.
                Select(x => x.CardSetId)
                .ToList();

            var usedCards = game.PlayCards
                .Select(x => x.CardId).ToList();

            var unusedCards = await _dbContext.Cards
                .Where(x => selectedCardSetsIds.Contains(x.CardSetId) && !usedCards.Contains(x.Id))
                .ToListAsync();

            Logger.LogInformation($"Генерация карточки. unusedCard.Count: {unusedCards.Count} {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

            int index = rnd.Next(unusedCards.Count() - 1);
            var randomCard = unusedCards[index];

            var newPlayCard = new PlayCard()
            {
                GameId = gameId,
                CardId = randomCard.Id,
                Username = username,
            };

            return newPlayCard;
        }

        private int GenerateChipId(List<User> users)
        {
            var usedIds = users.Select(x => x.ChipId).ToList();

            var allIds = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            var unusedIds = allIds
                .Where(x => !usedIds.Contains(x))
                .ToList();

            if (allIds.Count >= usedIds.Count) // без повторов
            {
                int index = rnd.Next(unusedIds.Count() - 1);
                return unusedIds[index];
            }
            else // с повторами (участников больше чем цветов)
            {
                int index = rnd.Next(allIds.Count() - 1);
                return unusedIds[index];
            }
        }

        private async Task<int> GetPointsForUserForRound(string user, int gameId, List<PlayCard> roundCards)
        {
            Logger.LogInformation("");
            var game = await _dbContext.Games
                .Include(x => x.PlayCards)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            if (user == game.ActivePlayerName)
            {
                var userCard = roundCards
                    .FirstOrDefault(x => x.Username == user);

                Logger.LogInformation($"Карточка игрока {user}. Количество проголосовавших: {userCard.VotedUsers.Count}: {string.Join(",", userCard.VotedUsers)} ");

                // единственный проголосовавший за карту - ведущий
                if (userCard.VotedUsers.Count == 1)
                {
                    Logger.LogInformation($"игрок {user} ведущий. Никто не угадал его карту");
                    return -2;
                }

                // все проголосовали за карту (включая ведущего)
                else if (userCard.VotedUsers.Count == game.Users.Count)
                {
                    Logger.LogInformation($"игрок {user} ведущий. Все угадали его карту");
                    return -3;
                }

                // прибавляем 3 + за каждого проголосовавшего (минус голос ведущего)
                else
                {
                    Logger.LogInformation($"игрок {user} ведущий. Получает 3 очка плюс 1 за каждого игрока: {string.Join(",", userCard.VotedUsers)}");
                    return 3 + userCard.VotedUsers.Count - 1;
                }

            }
            else
            {
                var userCard = roundCards
                    .FirstOrDefault(x => x.Username == user);

                Logger.LogInformation($"Карточка игрока {user}. Количество проголосовавших: {userCard.VotedUsers.Count}: {string.Join(",", userCard.VotedUsers)} ");

                // по 1 баллу за каждого проголосовавшего за карточку пользователя
                var points = userCard.VotedUsers.Count;
                Logger.LogInformation($"игрок {user}. Получает 1 за каждого игрока: {string.Join(",", userCard.VotedUsers)}");

                var activePlayerCard = roundCards
                    .FirstOrDefault(x => x.Username == game.ActivePlayerName);

                Logger.LogInformation($"Карточка ведущего {game.ActivePlayerName}. Количество проголосовавших: {userCard.VotedUsers.Count}: {string.Join(",", activePlayerCard.VotedUsers)} ");

                // если игрок угадал карточку ведущего, при этом угадали карточку не все игроки
                if (activePlayerCard.VotedUsers.Contains(user) && activePlayerCard.VotedUsers.Count < game.Users.Count)
                {
                    points += 3;
                    Logger.LogInformation($"игрок {user} угадал карточку ведущего. Еще плюс 3");
                }
                return points;
            }
        }

        private void GenerateCardsForNewCardsSet(string folderName, int cardsCount, int setId)
        {
            var newCardsList = new List<Card>();

            for (int i = 1; i <= cardsCount; i++)
            {
                newCardsList.Add(new Card()
                {
                    Src = $"/images/CardSets/{folderName}/{i}.jpg",
                    NumberInSet = i,
                    CardSetId = setId
                });
            }

            _dbContext.Cards.AddRange(newCardsList);
            _dbContext.SaveChanges();
        }
    }
}