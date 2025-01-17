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
    [Migration("20200415060225_third")]
    partial class third
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

                    b.ToTable("Card");
                });

            modelBuilder.Entity("Imaginarium.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Round");

                    b.Property<int>("Status");

                    b.Property<List<string>>("Users");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Imaginarium.Models.PlayCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardId");

                    b.Property<int>("Round");

                    b.Property<bool>("Selected");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("PlayCard");
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
                });
#pragma warning restore 612, 618
        }
    }
}
