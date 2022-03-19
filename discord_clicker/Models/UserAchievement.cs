using System;

namespace discord_clicker.Models
{
    public class UserAchievement: IUserItem<Achievement>
    {
        public int UserId { get; set; }
        #nullable enable
        public User? User { get; set; }

        public int ItemId { get; set; }
        #nullable enable
        public Achievement? Item { get; set; }

        public uint Count { get; set; }
        public DateTime DateOfachievement { get; set; }
    }
}
