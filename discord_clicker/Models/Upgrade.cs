using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace discord_clicker.Models
{
    public class Upgrade
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public uint BuildId { get; set; }
        public string Action { get; set; }
        public uint ConditionGet { get; set; }
        public bool ForEachBuild { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new();
        public List<UserUpgrade> UserUpgrades { get; set; } = new();
    }
}