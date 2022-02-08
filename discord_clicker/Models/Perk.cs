using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace discord_clicker.Models
{
    public class Perk
    {
        [Key]
        public int Id { get; set; }
        public ulong Cost { get; set; }
        public string Name { get; set; }
        public ulong PassiveCoefficient { get; set; }
        public uint ClickCoefficient { get; set; }
        public int Tier { get; set; }
        public List<User> Users { get; set; } = new();
        public List<UserPerk> UserPerks { get; set; } = new();

    }
}
