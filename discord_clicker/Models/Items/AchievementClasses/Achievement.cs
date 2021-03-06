using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models.Items.AchievementClasses;

public class Achievement : IItem<Achievement, AchievementCreateModel>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string AchievementObjectType { get; set; }
    public uint AchievementObjectId { get; set; }
    public decimal AchievementObjectCount { get; set; }
    public string Description { get; set; }
    public List<User> Users { get; set; } = new();
    public List<UserAchievement> UserAchievements { get; set; } = new();

    public (bool, string, User) Get(User user, long money, DbSet<Achievement> achievements)
    {
        return (true, "", user);
    }
}