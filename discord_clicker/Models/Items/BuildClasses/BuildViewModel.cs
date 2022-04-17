namespace discord_clicker.Models.Items.BuildClasses;

public class BuildViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; }
    public decimal PassiveCoefficient { get; set; }
}