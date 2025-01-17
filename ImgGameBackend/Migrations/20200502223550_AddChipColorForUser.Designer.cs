﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Imaginarium.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Imaginarium.Migrations
{
    [DbContext(typeof(ImaginariumContext))]
    [Migration("20200502223550_AddChipColorForUser")]
    partial class AddChipColorForUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Imaginarium.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardSetId");

                    b.Property<int>("NumberInSet");

                    b.Property<string>("Src");

                    b.HasKey("Id");

                    b.HasIndex("CardSetId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Imaginarium.Models.CardSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameEng");

                    b.Property<string>("NameRus");

                    b.HasKey("Id");

                    b.ToTable("CardSets");
                });

            modelBuilder.Entity("Imaginarium.Models.CardSetGame", b =>
                {
                    b.Property<int>("CardSetId");

                    b.Property<int>("GameId");

                    b.HasKey("CardSetId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("CardSetGames");
                });

            modelBuilder.Entity("Imaginarium.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivePlayerName");

                    b.Property<int?>("Round");

                    b.Property<int>("Status");

                    b.Property<int>("VotedOnRoundCount");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Imaginarium.Models.PlayCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardId");

                    b.Property<int>("GameId");

                    b.Property<int?>("Round");

                    b.Property<bool>("Used");

                    b.Property<string>("Username");

                    b.Property<List<string>>("VotedUsers");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("GameId", "CardId")
                        .IsUnique();

                    b.ToTable("PlayCards");
                });

            modelBuilder.Entity("Imaginarium.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChipColor");

                    b.Property<string>("ConnectionId");

                    b.Property<int>("GameId");

                    b.Property<string>("Name");

                    b.Property<int>("Points");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Imaginarium.Models.Card", b =>
                {
                    b.HasOne("Imaginarium.Models.CardSet", "CardSet")
                        .WithMany("Cards")
                        .HasForeignKey("CardSetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Imaginarium.Models.CardSetGame", b =>
                {
                    b.HasOne("Imaginarium.Models.CardSet", "CardSet")
                        .WithMany("CardSetGames")
                        .HasForeignKey("CardSetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Imaginarium.Models.Game", "Game")
                        .WithMany("CardSetGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Imaginarium.Models.PlayCard", b =>
                {
                    b.HasOne("Imaginarium.Models.Card", "Card")
                        .WithMany("PlayCards")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Imaginarium.Models.Game", "Game")
                        .WithMany("PlayCards")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Imaginarium.Models.User", b =>
                {
                    b.HasOne("Imaginarium.Models.Game", "Game")
                        .WithMany("Users")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
