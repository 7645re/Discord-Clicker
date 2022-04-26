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
    private Dictionary<int, Dictionary<string, object>> MapBuilds(List<UserBuild> userBuilds)
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

    private Dictionary<int, Dictionary<string, object>> MapUpgrades(List<UserUpgrade> userUpgrades)
    {
        Dictionary<int, Dictionary<string, object>> upgrades = new();
        foreach (UserUpgrade userUpgrade in userUpgrades)
        {
            if (userUpgrade.Item != null)
            {
                upgrades.Add(userUpgrade.Item.Id, new Dictionary<string, object>
                {
                    {"ItemName", userUpgrade.Item.Name}, {"ItemCount", userUpgrade.Count}
                });
            }
        }
        return upgrades;
    }

    private Dictionary<int, Dictionary<string, object>> MapAchievements(List<UserAchievement> userAchievements)
    {
        Dictionary<int, Dictionary<string, object>> achievements = new();
        foreach (UserAchievement userAchievement in userAchievements)
        {
            if (userAchievement.Item != null)
            {
                achievements.Add(userAchievement.Item.Id, 
                    new Dictionary<string, object>
                    {
                        {"ItemName", userAchievement.Item.Name}, {"ItemDate", userAchievement.DateOfAchievement}
                    });
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
                opt => opt.MapFrom(c => MapAchievements(c.UserAchievements)))
            .ForMember("Role", opt => opt.MapFrom(c => c.Role.Name));
    }
}
