using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;

namespace discord_clicker.Models
{
    public class Upgrade
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public uint BuildId { get; set; }
        public string Action { get; set; }
        public uint ConditionGet { get; set; }
        public bool ForEachBuild { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new();
        public List<UserUpgrade> UserUpgrades { get; set; } = new();
    }
        public static class UpgradeExtension {
        public static UpgradeModel ToUpgradeModel(this Upgrade upgrade) => new UpgradeModel {
            Id=upgrade.Id,
            Name=upgrade.Name,
            Cost=upgrade.Cost,
            BuildId=upgrade.BuildId,
            Action=upgrade.Action,
            ConditionGet=upgrade.ConditionGet,
            ForEachBuild=upgrade.ForEachBuild,
            Description=upgrade.Description,
        };
    }
}