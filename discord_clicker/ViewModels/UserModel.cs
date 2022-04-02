using System.Collections.Generic;
using System;

namespace discord_clicker.ViewModels;

public class UserModel
{
    public int Id { get; set; }
    public decimal Money { get; set; }
    public decimal AllMoney { get; set; }
    public decimal Click { get; set; }
    public DateTime PlayStartDate { get; set; }
    public string Nickname { get; set; }
    public decimal ClickCoefficient { get; set; }
    public decimal PassiveCoefficient { get; set; }
    public Dictionary<int, Dictionary<string, object>> Builds { get; set; } = new ();
    public Dictionary<string, uint> Upgrades { get; set; } = new ();
    public Dictionary<string, DateTime> Achievements { get; set; } = new ();
}