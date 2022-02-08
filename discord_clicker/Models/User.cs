using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace discord_clicker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public ulong Money { get; set; }
        public int Tier { get; set; }
        public uint ClickCoefficient { get; set; }
        public ulong PassiveCoefficient { get; set; }
        public List<Perk> Perks { get; set; } = new();
        public List<UserPerk> UserPerks { get; set; } = new();
    }
}
