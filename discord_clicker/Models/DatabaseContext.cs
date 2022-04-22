using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;
using discord_clicker.Models.Person;

namespace discord_clicker.Models;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Build> Builds { get; set; }
    public DbSet<Upgrade> Upgrades { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserBuild> UsersBuilds { get; set; }
    public DbSet<UserAchievement> UsersAchievements { get; set; }
    public DbSet<UserUpgrade> UsersUpgrades { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Role adminRole = new Role() { Name = "admin", Id = 1};
        Role userRole = new Role() { Name = "user", Id = 2};

        User admin = new User()
        {
            Nickname = "admin", Password = "admin", RoleId = adminRole.Id, Id = 1,
            Money = 0, AllMoney = 0, Click = 0, PlayStartDate = DateTime.UtcNow,
            LastRequestDate = DateTime.UtcNow, ClickCoefficient = 1, PassiveCoefficient = 0};
        
        modelBuilder.Entity<Role>().HasData(adminRole, userRole);
        modelBuilder.Entity<User>().HasData(admin);
        
        modelBuilder
            .Entity<Build>()
            .HasMany(c => c.Users)
            .WithMany(s => s.Builds)
            .UsingEntity<UserBuild>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserBuilds)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Item)
                    .WithMany(p => p.UserBuilds)
                    .HasForeignKey(pt => pt.ItemId),
                j =>
                {
                    j.HasKey(t => new { t.ItemId, t.UserId });
                    j.ToTable("UserBuilds");
                });
        modelBuilder
            .Entity<Upgrade>()
            .HasMany(c => c.Users)
            .WithMany(s => s.Upgrades)
            .UsingEntity<UserUpgrade>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserUpgrades)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Item)
                    .WithMany(p => p.UserUpgrades)
                    .HasForeignKey(pt => pt.ItemId),
                j =>
                {
                    j.HasKey(t => new { t.ItemId, t.UserId });
                    j.ToTable("UserUpgrades");
                });
        modelBuilder
            .Entity<Achievement>()
            .HasMany(c => c.Users)
            .WithMany(s => s.Achievements)
            .UsingEntity<UserAchievement>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserAchievements)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Item)
                    .WithMany(p => p.UserAchievements)
                    .HasForeignKey(pt => pt.ItemId),
                j =>
                {
                    j.HasKey(t => new { t.ItemId, t.UserId });
                    j.ToTable("UserAchievements");
                });
    }
}