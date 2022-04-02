﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace discord_clicker.Models;

/// <summary>
/// Rich Build Model
/// </summary>
public class Build : IItem<Build, BuildModel>
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
    public Build Create(Dictionary<string, object> parameters) {
        return new Build() {
            Id=(int)parameters["Id"],
            Name=(string)parameters["Name"],
            Cost=(decimal)parameters["Cost"],
            Description=(string)parameters["Description"],
            PassiveCoefficient=(decimal)parameters["PassiveCoefficient"]
        };
    }
    public (bool, string, User) Get(User user, decimal money) {
        bool enoughMoney = user.Money + money >= this.Cost;
        bool presenceUserItem = user.UserBuilds.FirstOrDefault(ub => ub.ItemId == this.Id) != null;
        if (!enoughMoney) {
            return (false, "not enough money", user);
        }
        if (!presenceUserItem) {
            user.UserBuilds.Add(new UserBuild {UserId = user.Id, ItemId=this.Id, Count=0, Item=this, PassiveCoefficient=0});
        }
        user.PassiveCoefficient+=this.PassiveCoefficient;
        user.Money = user.Money + money - this.Cost;
        user.UserBuilds.First(ub => ub.ItemId == this.Id).Count++;
        user.UserBuilds.First(ub => ub.ItemId == this.Id).PassiveCoefficient+=this.PassiveCoefficient;
        uint buildCount = user.UserBuilds.First(ub => ub.ItemId == this.Id).Count;
        Achievement? achievement = user.Achievements.FirstOrDefault(a => a.AchievementObjectType == "Build" 
            && a.AchievementObjectId == this.Id && a.AchievementObjectCount == buildCount);
        if (achievement != null) {
            user.UserAchievements.Add(new UserAchievement {UserId=user.Id, ItemId=achievement.Id, Count=1, DateOfAchievement=DateTime.UtcNow});
        }
        return (true, "succes", user);   
    }
}