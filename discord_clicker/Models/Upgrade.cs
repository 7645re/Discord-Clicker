using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;
using System.Linq;
using System;

namespace discord_clicker.Models
{
    /// <summary>
    /// Rich Upgrade Model
    /// </summary>
    public class Upgrade : IItem<Upgrade, UpgradeModel>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public uint BuildId { get; set; }
        public string Action { get; set; }
        public uint ConditionGet { get; set; }
        public decimal GetMoney { get; set; }
        public bool ForEachBuild { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new();
        public List<UserUpgrade> UserUpgrades { get; set; } = new();

        public UpgradeModel ToViewModel () {
            return new UpgradeModel() {
                Id=this.Id,
                Name=this.Name,
                Cost=this.Cost,
                BuildId=this.BuildId,
                Action=this.Action,
                ConditionGet=this.ConditionGet,
                ForEachBuild=this.ForEachBuild,
                Description=this.Description,
                GetMoney=this.GetMoney
            };
        }
        public Upgrade Create(Dictionary<string, object> parameters) {
            return new Upgrade() {
                Id=(int)parameters["Id"],
                Name=(string)parameters["Name"],
                Cost=(decimal)parameters["Cost"],
                BuildId=(uint)parameters["BuildId"],
                Action=(string)parameters["Action"],
                ConditionGet=(uint)parameters["ConditionGet"],
                ForEachBuild=(bool)parameters["ForEachBuild"],
                Description=(string)parameters["Description"],
                GetMoney=(decimal)parameters["GetMoney"]
            };
        }
        public (bool, string, User) Get(User user, decimal money) {
            bool enoughMoney = user.Money + money >= this.Cost;
            bool presenceUserUpgrade = user.UserUpgrades.Where(up => up.ItemId == this.Id).FirstOrDefault() != null;
            bool presenceUserBuild = user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).FirstOrDefault() != null;
            if (!presenceUserBuild) {
                return (false, "you dont have item for this upgrade", user);
            }
            if (!enoughMoney) {
                return (false, "not enough money", user);
            }
            uint buildCount = user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).First().Count;
            bool conditionDone = buildCount >= this.ConditionGet;
            if (!conditionDone) {
                return (false, "not condition done", user);
            }
            if (!presenceUserUpgrade) {
                user.UserUpgrades.Add(new UserUpgrade {UserId = user.Id, ItemId=this.Id, Count=0, Item=this});
            }
            user.Money = user.Money + money - this.Cost;
            user.UserUpgrades.Where(up => up.ItemId == this.Id).First().Count++;
            uint upgradeCount = user.UserUpgrades.Where(up => up.ItemId == this.Id).First().Count;
            switch (this.Action, this.ForEachBuild) {
                case ("+", true):
                    user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).First().PassiveCoefficient+=buildCount*this.GetMoney;
                break;
                case ("+", false):
                    user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).First().PassiveCoefficient+=this.GetMoney;
                break;
                case ("*", true):
                    user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).First().PassiveCoefficient+=buildCount*this.GetMoney;
                break;
                case ("*", false):
                    user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).First().PassiveCoefficient+=this.GetMoney;
                break;
            }
            user.PassiveCoefficient=0;
            user.UserBuilds.ForEach(ub => user.PassiveCoefficient+=ub.PassiveCoefficient);
            Achievement? achievement = user.Achievements.Where(a => a.AchievementObjectType == "Upgrade" && a.AchievementObjectId == this.Id && a.AchievementObjectCount == upgradeCount).FirstOrDefault();
            if (achievement != null) {
                user.UserAchievements.Add(new UserAchievement {UserId=user.Id, ItemId=achievement.Id, Count=1, DateOfachievement=DateTime.UtcNow});
            }
            return (true, "succes", user);   
        }
    }
}