using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Imaginarium.Models
{
    public class ImaginariumContext : DbContext
    {
        public ImaginariumContext(DbContextOptions<ImaginariumContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardSet> CardSets { get; set; }
        public DbSet<PlayCard> PlayCards { get; set; }
        public DbSet<CardSetGame> CardSetGames { get; set; }

        public ImaginariumContext()
        {
            Database.EnsureCreated();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardSetGame>()
                .HasKey(t => new { t.CardSetId, t.GameId });

            modelBuilder.Entity<CardSetGame>()
                .HasOne(sc => sc.CardSet)
                .WithMany(s => s.CardSetGames)
                .HasForeignKey(sc => sc.CardSetId);

            modelBuilder.Entity<CardSetGame>()
                .HasOne(sc => sc.Game)
                .WithMany(c => c.CardSetGames)
                .HasForeignKey(sc => sc.GameId);

            modelBuilder.Entity<PlayCard>()
                .HasIndex(p => new { p.GameId, p.CardId }).IsUnique();
        }
    }
}


