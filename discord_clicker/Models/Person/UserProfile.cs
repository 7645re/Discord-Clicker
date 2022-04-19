using AutoMapper;
using System;
using System.Collections.Generic;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;
using discord_clicker.ViewModels;

namespace discord_clicker.Models.Person;

public class UserProfile : Profile
{
    public Dictionary<int, Dictionary<string, object>> MapBuilds(List<UserBuild> userBuilds)
    {
        Dictionary<int, Dictionary<string, object>> builds = new Dictionary<int, Dictionary<string, object>>();
        foreach (UserBuild userBuild in userBuilds)
        {
            if (userBuild.Item != null)
            {
                builds.Add(userBuild.Item.Id,
                    new Dictionary<string, object>
                    {
                        {"ItemName", userBuild.Item.Name}, {"ItemCount", userBuild.Count},
                        {"PassiveCoefficient", userBuild.PassiveCoefficient}
                    });
            }
        }
        
        return builds;
    }

    public Dictionary<string, uint> MapUpgrades(List<UserUpgrade> userUpgrades)
    {
        Dictionary<string, uint> upgrades = new Dictionary<string, uint>();
        foreach (UserUpgrade userUpgrade in userUpgrades)
        {
            if (userUpgrade.Item != null)
            {
                upgrades.Add(userUpgrade.Item.Name, userUpgrade.Count);
            }
        }

        return upgrades;
    }

    public Dictionary<string, DateTime> MapAchievements(List<UserAchievement> userAchievements)
    {
        Dictionary<string, DateTime> achievements = new Dictionary<string, DateTime>();
        foreach (UserAchievement userAchievement in userAchievements)
        {
            if (userAchievement.Item != null)
            {
                achievements.Add(userAchievement.Item.Name, userAchievement.DateOfAchievement);
            }
        }

        return achievements;
    }

    public UserProfile()
    {
        CreateMap<UserCreateModel, User>().ForMember("PlayStartDate",
            opt => opt.MapFrom(c => DateTime.UtcNow)).ForMember("LastRequestDate",
            opt => opt.MapFrom(c => DateTime.UtcNow)).ForMember("ClickCoefficient",
            opt => opt.MapFrom(c => 1));
        CreateMap<RegisterModel, User>().ForMember("PlayStartDate",
            opt => opt.MapFrom(c => DateTime.UtcNow)).ForMember("LastRequestDate",
            opt => opt.MapFrom(c => DateTime.UtcNow)).ForMember("ClickCoefficient",
            opt => opt.MapFrom(c => 1));
        CreateMap<User, UserViewModel>().ForMember("Builds",
            opt => opt.MapFrom(c => MapBuilds(c.UserBuilds)))
            .ForMember("Upgrades",
                opt => opt.MapFrom(c => MapUpgrades(c.UserUpgrades)))
            .ForMember("Achievements",
                opt => opt.MapFrom(c => MapAchievements(c.UserAchievements)));
    }
}
