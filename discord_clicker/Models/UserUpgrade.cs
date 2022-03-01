namespace discord_clicker.Models
{
    public class UserUpgrade
    {
        public int UserId { get; set; }
        #nullable enable
        public User? User { get; set; }

        public int UpgradeId { get; set; }
        #nullable enable
        public Upgrade? Upgrade { get; set; }

        public uint Count { get; set; }
    }
}
