namespace discord_clicker.Models
{
    public class UserUpgrade
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int UpgradeId { get; set; }
        public Upgrade? Upgrade { get; set; }

        public uint Count { get; set; }
    }
}
