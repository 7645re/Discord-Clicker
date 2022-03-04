using System.Collections.Generic;
using discord_clicker.Models;
using System;

namespace discord_clicker.ViewModels
{
    public class UserModel
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public decimal AllMoney { get; set; }
        public DateTime PlayStartDate { get; set; }
        public string Nickname { get; set; }
        public decimal ClickCoefficient { get; set; }
        public decimal PassiveCoefficient { get; set; }
        public Dictionary<string, uint> Builds { get; set; } = new ();
        public Dictionary<string, uint> Upgrades { get; set; } = new ();
        public Dictionary<string, DateTime> Achievements { get; set; } = new ();
    }
}
