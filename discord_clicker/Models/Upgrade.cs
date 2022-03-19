using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;
using System.Linq;

namespace discord_clicker.Models
{
    /// <summary>
    /// Rich Upgrade Model
    /// </summary>
    public class Upgrade : IItem<UpgradeModel>
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
            };
        }
        public (bool, string, User) Get(User user, decimal money) {
            bool enoughMoney = user.Money + money >= this.Cost;
            bool presenceRowUserItem = user.UserUpgrades.Where(up => up.ItemId == this.Id).FirstOrDefault() != null;
            bool presenceUserItem = user.UserBuilds.Where(ub => ub.ItemId == this.BuildId).FirstOrDefault() != null;
            uint upgradeCount = user.UserBuilds.Where(ub => ub.ItemId == this.Id).First().Count;
            bool conditionDone = upgradeCount >= this.ConditionGet;
            if (!presenceRowUserItem) {
                return (false, "you dont have item for this upgrade", user);
            }
            if (!enoughMoney) {
                return (false, "not enough money", user);
            }
            if (!conditionDone) {
                return (false, "not condition done", user);
            }
            if (!presenceRowUserItem) {
                user.UserUpgrades.Add(new UserUpgrade {UserId = user.Id, ItemId=this.Id, Count=0, Item=this});
            }
            user.Money = user.Money + money - this.Cost;
            user.UserUpgrades.Where(up => up.ItemId == this.Id).First().Count++;
            // switch (this.Action, this.ForEachBuild) {
            //     case ("+", true):
            //         user.UserBuilds.Where(ub =>)
            //     break;  
            // }

            return (true, "succes", user);   
        }
    }
}