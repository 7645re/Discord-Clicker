using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Perk> Perks { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Perk>()
                .HasMany(c => c.Users)
                .WithMany(s => s.Perks)
                .UsingEntity<UserPerk>(
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserPerks)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Perk)
                    .WithMany(p => p.UserPerks)
                    .HasForeignKey(pt => pt.PerkId),
                j =>
                {
                    j.Property(pt => pt.Count).HasDefaultValue(0);
                    j.HasKey(t => new { t.PerkId, t.UserId });
                    j.ToTable("UserPerks");
                });
        }
    }
}