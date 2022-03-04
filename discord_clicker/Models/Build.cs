using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using discord_clicker.ViewModels;

namespace discord_clicker.Models
{
    public class Build
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new ();
        public decimal PassiveCoefficient { get; set; }
        public List<UserBuild> UserBuilds { get; set; } = new();
    }
    public static class BuildExtension {
        public static BuildModel ToBuildModel(this Build build) => new BuildModel {
            Id=build.Id,
            Name=build.Name,
            Cost=build.Cost,
            Description=build.Description,
            PassiveCoefficient=build.PassiveCoefficient
        };
    }
}
