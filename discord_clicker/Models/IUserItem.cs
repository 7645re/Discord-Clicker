namespace discord_clicker.Models;

public interface IUserItem<T>
{
    public int UserId { get; set; }
#nullable enable
    public User? User { get; set; }

    public int ItemId { get; set; }
#nullable enable
    public T? Item { get; set; }

    public uint Count { get; set; }
}