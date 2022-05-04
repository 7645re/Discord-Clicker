using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models.Items.UpgradeClasses;

public class Upgrade : IItem<Upgrade, UpgradeCreateModel>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public long Cost { get; set; }
    public uint BuildId { get; set; }
    public string Action { get; set; }
    public uint ConditionGet { get; set; }
    public long GetMoney { get; set; }
    public bool ForEachBuild { get; set; }
    public string Description { get; set; }
    public List<User> Users { get; set; } = new();
    public List<UserUpgrade> UserUpgrades { get; set; } = new();

    public (bool, string, User) Get(User user, long money, DbSet<Achievement> achievements)
    {
        bool enoughMoney = user.Money + money >= Cost;
        bool presenceUserUpgrade = user.UserUpgrades.FirstOrDefault(up => up.ItemId == Id) != null;
        bool presenceUserBuild = user.UserBuilds.FirstOrDefault(ub => ub.ItemId == BuildId) != null;
        if (!presenceUserBuild)
        {
            return (false, "you dont have item for this upgrade", user);
        }

        if (!enoughMoney)
        {
            return (false, "not enough money", user);
        }

        uint buildCount = user.UserBuilds.First(ub => ub.ItemId == BuildId).Count;
        bool conditionDone = buildCount >= ConditionGet;
        if (!conditionDone)
        {
            return (false, "not condition done", user);
        }

        if (!presenceUserUpgrade)
        {
            user.UserUpgrades.Add(new UserUpgrade {UserId = user.Id, ItemId = Id, Count = 0, Item = this});
        }

        user.Money = user.Money + money - Cost;
        user.UserUpgrades.First(up => up.ItemId == Id).Count++;
        uint upgradeCount = user.UserUpgrades.First(up => up.ItemId == Id).Count;
        switch (Action, ForEachBuild)
        {
            case ("+", true):
                user.UserBuilds.First(ub => ub.ItemId == BuildId).PassiveCoefficient += buildCount * GetMoney;
                break;
            case ("+", false):
                user.UserBuilds.First(ub => ub.ItemId == BuildId).PassiveCoefficient += GetMoney;
                break;
            case ("*", true):
                user.UserBuilds.First(ub => ub.ItemId == BuildId).PassiveCoefficient += buildCount * GetMoney;
                break;
            case ("*", false):
                user.UserBuilds.First(ub => ub.ItemId == BuildId).PassiveCoefficient += GetMoney;
                break;
        }

        user.PassiveCoefficient = 0;
        user.UserBuilds.ForEach(ub => user.PassiveCoefficient += ub.PassiveCoefficient);
        Achievement? achievement = user.Achievements.FirstOrDefault(a =>
            a.AchievementObjectType == "Upgrade" && a.AchievementObjectId == Id &&
            a.AchievementObjectCount == upgradeCount);
        if (achievement != null)
        {
            user.UserAchievements.Add(new UserAchievement
            {
                UserId = user.Id, ItemId = achievement.Id, Count = 1, DateOfAchievement = DateTime.UtcNow
            });
        }

        return (true, "success", user);
    }
}