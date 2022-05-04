using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;
using System.Numerics;

namespace discord_clicker.Models.Person;

public class User
{
    [Key]
    public int Id { get; set; }
    public long Money { get; set; }
    public long AllMoney { get; set; }
    public string Nickname { get; set; }
    public string Password { get; set; }
    public long Click { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public DateTime PlayStartDate { get; set; }
    public DateTime LastRequestDate {get; set; }
    public long ClickCoefficient { get; set; }
    public long PassiveCoefficient { get; set; }
    public List<Build> Builds { get; set; } = new ();
    public List<Upgrade> Upgrades { get; set; } = new ();
    public List<UserBuild> UserBuilds { get; set; } = new ();
    public List<UserUpgrade> UserUpgrades { get; set; } = new ();
    public List<Achievement> Achievements { get; set; } = new ();
    public List<UserAchievement> UserAchievements { get; set; } = new ();
}