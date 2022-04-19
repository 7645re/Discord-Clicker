namespace discord_clicker.Models.Items.UpgradeClasses;

public class UpgradeCreateModel : IItemCreateModel
{
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public uint BuildId { get; set; }
    public string Action { get; set; }
    public uint ConditionGet { get; set; }
    public decimal GetMoney { get; set; }
    public bool ForEachBuild { get; set; }
    public string Description { get; set; }
}