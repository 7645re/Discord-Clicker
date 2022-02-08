using Microsoft.EntityFrameworkCore;
namespace discord_clicker.Models
{
    public class UserPerk
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PerkId { get; set; }
        public Perk? Perk { get; set; }

        public uint Count { get; set; }
    }
}
