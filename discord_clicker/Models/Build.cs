﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;
using System.Linq;

namespace discord_clicker.Models
{
    /// <summary>
    /// Rich Build Model
    /// </summary>
    public class Build : IItem<BuildModel>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new ();
        public decimal PassiveCoefficient { get; set; }
        public List<UserBuild> UserBuilds { get; set; } = new();
        public BuildModel ToViewModel() {
            return new BuildModel() {
                Id=this.Id,
                Name=this.Name,
                Cost=this.Cost,
                Description=this.Description,
                PassiveCoefficient=this.PassiveCoefficient
            };
        }
        public (bool, string, User) Get(User user, decimal money) {
            bool enoughMoney = user.Money + money >= this.Cost;
            bool presenceRowUserItem = user.UserBuilds.Where(ub => ub.ItemId == this.Id).FirstOrDefault() != null;
            if (enoughMoney) {
                if (!presenceRowUserItem) {
                    user.UserBuilds.Add(new UserBuild {UserId = user.Id, ItemId=this.Id, Count=0, Item=this, PassiveCoefficient=0});
                }
                user.PassiveCoefficient+=this.PassiveCoefficient;
                user.Money = user.Money + money - this.Cost;
                user.UserBuilds.Where(ub => ub.ItemId == this.Id).First().Count++;
                user.UserBuilds.Where(ub => ub.ItemId == this.Id).First().PassiveCoefficient+=this.PassiveCoefficient;
                return (true, "succes", user);   
            }
            return (false, "not enough money", user);
        }
    }
}
