namespace discord_clicker.Models;

public class UserUpgrade: IUserItem<Upgrade>
{
    public int UserId { get; set; }
#nullable enable
    public User? User { get; set; }

    public int ItemId { get; set; }
#nullable enable
    public Upgrade? Item { get; set; }

    public uint Count { get; set; }
}