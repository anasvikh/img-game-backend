﻿// <auto-generated />
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
    [Migration("20200416171944_voted_count")]
    partial class voted_count
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

                    b.Property<string>("Src");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Imaginarium.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Round");

                    b.Property<int>("Status");

                    b.Property<List<string>>("Users");

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

                    b.Property<int>("Round");

                    b.Property<bool>("Used");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("GameId");

                    b.ToTable("PlayCards");
                });

            modelBuilder.Entity("Imaginarium.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
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
#pragma warning restore 612, 618
        }
    }
}
