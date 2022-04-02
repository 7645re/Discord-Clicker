using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;

namespace discord_clicker.Models;

/// <summary>
/// Rich Achievement Model
/// </summary>
public class Achievement : IItem<Achievement, AchievementModel>
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    /// <summary> Build or upgrade or clicks for which achievement is given </summary>
    public string AchievementObjectType { get; set; }
    public uint AchievementObjectId { get; set; } 
    /// <summary> Number of objects to get achievements </summary>
    public decimal AchievementObjectCount { get; set; }
#nullable enable
    public string? Description { get; set; }

    public List<User> Users { get; set; } = new ();
    public List<UserAchievement> UserAchievements { get; set; } = new();
    public AchievementModel ToViewModel () {
        return new AchievementModel() {
            Id=this.Id,
            Name=this.Name,
            Description=this.Description,
            AchievementObjectType=this.AchievementObjectType,
            AchievementObjectId=this.AchievementObjectId,
            AchievementObjectCount=this.AchievementObjectCount,
        };
    }
    public Achievement Create(Dictionary<string, object> parameters) {
        return new Achievement() {
            Id=(int)parameters["Id"],
            Name=(string)parameters["Name"],
            Description=(string)parameters["Description"],
            AchievementObjectType=(string)parameters["AchievementObjectType"],
            AchievementObjectId=(uint)parameters["AchievementObjectId"],
            AchievementObjectCount=(decimal)parameters["AchievementObjectCount"]
        };
    }
    public (bool, string, User) Get(User user, decimal money) {
        return (true, "", user);
    }
}