using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using discord_clicker.Models.Person;

namespace discord_clicker.Models.Items.AchievementClasses;

public class Achievement : IItem
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string AchievementObjectType { get; set; }
    public uint AchievementObjectId { get; set; } 
    public decimal AchievementObjectCount { get; set; }
    #nullable enable
    public string? Description { get; set; }
    public List<User> Users { get; set; } = new ();
    public List<UserAchievement> UserAchievements { get; set; } = new();
    public (bool, string, User) Get(User user, decimal money) {
        return (true, "", user);
    }
}