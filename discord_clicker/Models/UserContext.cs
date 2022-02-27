using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<Upgrade> Upgrades { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
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
                    .HasOne(pt => pt.Build)
                    .WithMany(p => p.UserBuilds)
                    .HasForeignKey(pt => pt.BuildId),
                j =>
                {
                    j.Property(pt => pt.Count).HasDefaultValue(0);
                    j.HasKey(t => new { t.BuildId, t.UserId });
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
                    .HasOne(pt => pt.Upgrade)
                    .WithMany(p => p.UserUpgrades)
                    .HasForeignKey(pt => pt.UpgradeId),
                j =>
                {
                    j.Property(pt => pt.Count).HasDefaultValue(0);
                    j.HasKey(t => new { t.UpgradeId, t.UserId });
                    j.ToTable("UserUpgrades");
                });
        }
    }
}