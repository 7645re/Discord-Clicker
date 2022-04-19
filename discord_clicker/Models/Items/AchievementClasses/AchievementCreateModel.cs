namespace discord_clicker.Models.Items.AchievementClasses;

public class AchievementCreateModel : IItemCreateModel
{
    public string Name { get; set; }
    public string AchievementObjectType { get; set; }
    public uint AchievementObjectId { get; set; } 
    public decimal AchievementObjectCount { get; set; }
    public string Description { get; set; }
}