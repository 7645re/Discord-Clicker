using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace discord_clicker.Models
{
    public class Build
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new ();
        public decimal PassiveCoefficient { get; set; }
        public List<UserBuild> UserBuilds { get; set; } = new();

    }
}
