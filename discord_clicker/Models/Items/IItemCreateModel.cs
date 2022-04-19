namespace discord_clicker.Models.Items;

public interface IItemCreateModel
{
    public string Name { get; set; }
    #nullable enable
    public string? Description { get; set; }
}