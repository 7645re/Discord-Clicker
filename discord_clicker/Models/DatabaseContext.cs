using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace discord_clicker.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<Upgrade> Upgrades { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserBuild> UsersBuilds { get; set; }
        public DbSet<UserAchievement> UsersAchievements { get; set; }
        public DbSet<UserUpgrade> UsersUpgrades { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
    public static class DbSetExtension {
        public static async Task<IQueryable<T>> If<T, P>(this IIncludableQueryable<T, P> source, bool condition, Func<IIncludableQueryable<T, P>, IQueryable<T>> transform) where T : class
        {
            return condition ? transform(source) : source;
        }
        public static async Task<IQueryable<T>> If<T>(this IQueryable<T> source, bool condition,Func<IQueryable<T>, IQueryable<T>> transform)
        { 
            return condition? transform(source) : source;
        }
        }
}