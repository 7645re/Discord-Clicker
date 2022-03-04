namespace discord_clicker.Models
{
     public class AchievementModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary> Build or upgrade or clicks for which achievement is given </summary>
        public string AchievementObject { get; set; }
        /// <summary> Number of objects to get achievements </summary>
        public decimal AchievementObjectCount { get; set; }
        #nullable enable
        public string? Description { get; set; }
    }
}
