using System;
using System.Collections.Generic;


namespace discord_clicker.Models.Person;

public class UserViewModel
{
    public int Id { get; set; }
    public decimal Money { get; set; }
    public decimal AllMoney { get; set; }
    public decimal Click { get; set; }
    public DateTime PlayStartDate { get; set; }
    public string Nickname { get; set; }
    public decimal ClickCoefficient { get; set; }
    public decimal PassiveCoefficient { get; set; }
    public string Role { get; set; }
    public Dictionary<int, Dictionary<string, object>> Builds { get; set; } = new ();
    public Dictionary<int, Dictionary<string, object>> Upgrades { get; set; } = new ();
    public Dictionary<int, Dictionary<string, object>> Achievements { get; set; } = new ();
}