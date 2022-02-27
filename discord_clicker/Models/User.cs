using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace discord_clicker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public DateTime LastRequestDate {get; set; }
        public decimal ClickCoefficient { get; set; }
        public decimal PassiveCoefficient { get; set; }
        public List<Build> Builds { get; set; } = new ();
        public List<Upgrade> Upgrades { get; set; } = new ();
        public List<UserBuild> UserBuilds { get; set; } = new ();
        public List<UserUpgrade> UserUpgrades { get; set; } = new ();
    }
}
