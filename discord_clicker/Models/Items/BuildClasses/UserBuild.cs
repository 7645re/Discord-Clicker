using System.Numerics;
using discord_clicker.Models.Person;

namespace discord_clicker.Models.Items.BuildClasses;

public class UserBuild: IUserItem<Build>
{
    public int UserId { get; set; }
    #nullable enable
    public User? User { get; set; }

    public int ItemId { get; set; }
    #nullable enable
    public Build? Item { get; set; }

    public uint Count { get; set; }
    public long PassiveCoefficient { get; set; }
}