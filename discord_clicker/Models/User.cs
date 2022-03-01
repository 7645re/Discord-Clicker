using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;
using System;

namespace discord_clicker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public decimal Money { get; set; }
        public decimal AllMoney { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public decimal Click { get; set; }
        public DateTime PlayStartDate { get; set; }
        public DateTime LastRequestDate {get; set; }
        public decimal ClickCoefficient { get; set; }
        public decimal PassiveCoefficient { get; set; }
        public List<Build> Builds { get; set; } = new ();
        public List<Upgrade> Upgrades { get; set; } = new ();
        public List<UserBuild> UserBuilds { get; set; } = new ();
        public List<UserUpgrade> UserUpgrades { get; set; } = new ();
        public List<Achievement> Achievements { get; set; } = new ();
        public List<UserAchievement> UserAchievements { get; set; } = new ();
    }
    public static class UserExtension {
        public static UserModel ToUserModel(this User user) => new UserModel {
            Id=user.Id,
            Money=user.Money,
            Nickname=user.Nickname,
            ClickCoefficient=user.ClickCoefficient,
            PassiveCoefficient=user.PassiveCoefficient,
            
        };
    }
}
