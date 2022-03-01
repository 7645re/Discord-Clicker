using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace discord_clicker.Models
{
     public class Achievement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary> Build or upgrade or clicks for which achievement is given </summary>
        public string AchievementObject { get; set; }
        /// <summary> Number of objects to get achievements </summary>
        public decimal AchievementObjectCount { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new ();
        public List<UserAchievement> UserAchievements { get; set; } = new();
    }
}
