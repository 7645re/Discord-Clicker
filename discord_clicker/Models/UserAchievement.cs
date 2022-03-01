using System;

namespace discord_clicker.Models
{
    public class UserAchievement
    {
        public int UserId { get; set; }
        #nullable enable
        public User? User { get; set; }

        public int AchievementId { get; set; }
        #nullable enable
        public Achievement? Achievement { get; set; }

        public DateTime DateOfachievement { get; set; }
    }
}
