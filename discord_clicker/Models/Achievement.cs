using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;

namespace discord_clicker.Models
{
    /// <summary>
    /// Rich Achievement Model
    /// </summary>
    public class Achievement : IItem<AchievementModel>
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
        public AchievementModel ToViewModel () {
            return new AchievementModel() {
                Id=this.Id,
                Name=this.Name,
                Description=this.Description,
                AchievementObject=this.AchievementObject,
                AchievementObjectCount=this.AchievementObjectCount,
            };
        }
        public (bool, string, User) Get(User user, decimal money) {
            return (true, "", user);
        }
    }
}
