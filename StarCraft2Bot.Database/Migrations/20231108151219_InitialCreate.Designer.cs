﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StarCraft2Bot.Database;

#nullable disable

namespace StarCraft2Bot.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231108151219_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.DataPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("CurrentBuild")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("CurrentBuild");

                    b.Property<int>("CurrentMinerals")
                        .HasColumnType("INTEGER")
                        .HasColumnName("CurrentMinerals");

                    b.Property<int>("CurrentVespene")
                        .HasColumnType("INTEGER")
                        .HasColumnName("CurrentVespene");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GameId");

                    b.Property<int>("IngameSeconds")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IngameSeconds");

                    b.Property<int>("KilledMinerals")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KilledUnits")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KilledVespene")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LostBuildings")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LostBuildings");

                    b.Property<int>("LostMinerals")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LostMinerals");

                    b.Property<int>("LostUnits")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LostUnits");

                    b.Property<int>("LostVespene")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LostVespene");

                    b.Property<int>("Supply")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Supply");

                    b.Property<int>("TotalMinerals")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TotalMinerals");

                    b.Property<int>("TotalVespene")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TotalVespene");

                    b.Property<int>("WorkerCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("WorkerCount");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Datapoints", (string)null);
                });

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int>("EnemyRace")
                        .HasColumnType("INTEGER")
                        .HasColumnName("EnemyRace");

                    b.Property<int>("GameLength")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GameLength");

                    b.Property<DateTime>("GameStart")
                        .HasColumnType("TEXT")
                        .HasColumnName("GameStart");

                    b.Property<string>("MapName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MyRace")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MyRace");

                    b.Property<int>("Result")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Result");

                    b.HasKey("Id");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.GameValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GameId");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Key");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameValues", (string)null);
                });

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.DataPoint", b =>
                {
                    b.HasOne("StarCraft2Bot.Database.Entities.Game", "Game")
                        .WithMany("DataPoints")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.GameValue", b =>
                {
                    b.HasOne("StarCraft2Bot.Database.Entities.Game", "Game")
                        .WithMany("Values")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("StarCraft2Bot.Database.Entities.Game", b =>
                {
                    b.Navigation("DataPoints");

                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}
