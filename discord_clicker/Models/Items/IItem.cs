using System.Collections.Generic;
using discord_clicker.Models.Person;
using Microsoft.EntityFrameworkCore;

namespace discord_clicker.Models.Items;

public interface IItem<TItem, TItemCreateModel> where TItem : class, IItem<TItem, TItemCreateModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public (bool, string, User) Get(User user, decimal money);
    public TItem Create(TItemCreateModel itemCreateModel, DbSet<TItem> itemContext);
}