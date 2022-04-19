using System;
using System.Collections.Generic;


namespace discord_clicker.Models.Person;

public class UserViewModel
{
    public decimal Money { get; set; }
    public decimal AllMoney { get; set; }
    public decimal Click { get; set; }
    public DateTime PlayStartDate { get; set; }
    public string Nickname { get; set; }
    public decimal ClickCoefficient { get; set; }
    public decimal PassiveCoefficient { get; set; }
    public Role Role { get; set; }
    public Dictionary<int, Dictionary<string, object>> Builds { get; set; } = new ();
    public Dictionary<int, Dictionary<string, object>> Upgrades { get; set; } = new ();
    public Dictionary<int, Dictionary<string, object>> Achievements { get; set; } = new ();
}