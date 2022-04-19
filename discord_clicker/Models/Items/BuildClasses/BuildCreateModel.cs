namespace discord_clicker.Models.Items.BuildClasses;

public class BuildCreateModel : IItemCreateModel
{
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public decimal PassiveCoefficient { get; set; }
    public string Description { get; set; }
}