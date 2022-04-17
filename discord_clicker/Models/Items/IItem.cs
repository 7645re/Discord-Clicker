using System.Collections.Generic;
using discord_clicker.Models.Person;

namespace discord_clicker.Models.Items;

public interface IItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public (bool, string, User) Get(User user, decimal money);
}