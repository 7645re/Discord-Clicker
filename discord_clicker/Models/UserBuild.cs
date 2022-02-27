namespace discord_clicker.Models
{
    public class UserBuild
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int BuildId { get; set; }
        public Build? Build { get; set; }

        public uint Count { get; set; }
    }
}
