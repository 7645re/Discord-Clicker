
namespace discord_clicker.ViewModels
{
    public class UpgradeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public uint BuildId { get; set; }
        public string Action { get; set; }
        public uint ConditionGet { get; set; }
        public bool ForEachBuild { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public decimal GetMoney { get; set;} 
    }
}
