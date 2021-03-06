#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models.Items.BuildClasses;

public class Build : IItem<Build, BuildCreateModel>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public long Cost { get; set; }
    public string Description { get; set; }

    public long PassiveCoefficient { get; set; }
    public List<User> Users { get; set; } = new();
    public List<UserBuild> UserBuilds { get; set; } = new();

    public (bool, string, User) Get(User user, long money, DbSet<Achievement> achievements)
    {
        UserBuild? userBuild = user.UserBuilds.FirstOrDefault(ub => ub.ItemId == Id);
        uint buildCount = 1;
        if (userBuild != null)
        {
            buildCount = userBuild.Count;
        }

        bool enoughMoney = money >= (long) (Cost * Math.Pow(1.15, buildCount));
        bool presenceUserItem = user.UserBuilds.FirstOrDefault(ub => ub.ItemId == Id) != null;
        if (!enoughMoney) {
            return (false, "not enough money", user);
        }
        if (!presenceUserItem) {
            user.UserBuilds.Add(new UserBuild
            {
                UserId = user.Id, ItemId=Id, Count=0, Item=this, PassiveCoefficient=0
            });
        }
        user.PassiveCoefficient+=PassiveCoefficient;
        user.Money = money - (long) (Cost * Math.Pow(1.15, buildCount));
        user.UserBuilds.First(ub => ub.ItemId == Id).Count++;
        user.UserBuilds.First(ub => ub.ItemId == Id).PassiveCoefficient+=PassiveCoefficient;
        buildCount = user.UserBuilds.First(ub => ub.ItemId == Id).Count;
        #nullable enable
        Achievement? achievement = achievements.
            FirstOrDefault(a => a.AchievementObjectType == "Build" 
                                && a.AchievementObjectId == Id && a.AchievementObjectCount == buildCount);
        
        if (achievement != null) {
            user.UserAchievements.Add(new UserAchievement 
                {UserId=user.Id, ItemId=achievement.Id, Count=1, DateOfAchievement=DateTime.UtcNow});
        }
        return (true, "success", user);   
    }
}