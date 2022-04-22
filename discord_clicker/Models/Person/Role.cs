using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace discord_clicker.Models.Person;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users = new List<User>();
}