﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using discord_clicker.Models;

namespace discord_clicker.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20220225180651_upgrade")]
    partial class upgrade
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("discord_clicker.Models.Build", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("PassiveCoefficient")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Builds");
                });

            modelBuilder.Entity("discord_clicker.Models.Upgrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<long>("BuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("ConditionGet")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("ForEachBuild")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Upgrades");
                });

            modelBuilder.Entity("discord_clicker.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<decimal>("ClickCoefficient")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("LastRequestDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Money")
                        .HasColumnType("numeric");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<decimal>("PassiveCoefficient")
                        .HasColumnType("numeric");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("discord_clicker.Models.UserBuild", b =>
                {
                    b.Property<int>("BuildId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<long>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.HasKey("BuildId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBuilds");
                });

            modelBuilder.Entity("discord_clicker.Models.UserUpgrade", b =>
                {
                    b.Property<int>("UpgradeId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<long>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.HasKey("UpgradeId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserUpgrades");
                });

            modelBuilder.Entity("discord_clicker.Models.UserBuild", b =>
                {
                    b.HasOne("discord_clicker.Models.Build", "Build")
                        .WithMany("UserBuilds")
                        .HasForeignKey("BuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("discord_clicker.Models.User", "User")
                        .WithMany("UserBuilds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Build");

                    b.Navigation("User");
                });

            modelBuilder.Entity("discord_clicker.Models.UserUpgrade", b =>
                {
                    b.HasOne("discord_clicker.Models.Upgrade", "Upgrade")
                        .WithMany("UserUpgrades")
                        .HasForeignKey("UpgradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("discord_clicker.Models.User", "User")
                        .WithMany("UserUpgrades")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Upgrade");

                    b.Navigation("User");
                });

            modelBuilder.Entity("discord_clicker.Models.Build", b =>
                {
                    b.Navigation("UserBuilds");
                });

            modelBuilder.Entity("discord_clicker.Models.Upgrade", b =>
                {
                    b.Navigation("UserUpgrades");
                });

            modelBuilder.Entity("discord_clicker.Models.User", b =>
                {
                    b.Navigation("UserBuilds");

                    b.Navigation("UserUpgrades");
                });
#pragma warning restore 612, 618
        }
    }
}
