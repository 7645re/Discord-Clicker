using discord_clicker.Models;
using discord_clicker.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace discord_clicker.Services
{
    /// <summary>
    /// A class designed to interact with various items, to buy them and display the assortment
    /// </summary>
    /// <typeparam name="T"> Type of item </typeparam>
    /// <typeparam name="VT">  Type of Viewitem for mapping item </typeparam>
    public class ItemHandler<T, VT> : IItemHandler<T, VT> where T : class, IItem<T, VT>, new()
    {
        private readonly DatabaseContext _db;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        private static readonly T StaticItem = new T();

        public ItemHandler(DatabaseContext context, IMemoryCache memoryCache, ILogger<ItemHandler<T, VT>> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
        }

        public async Task<List<VT>> GetItemsList(int userId, DbSet<T> items)
        {
            _logger.LogInformation("фыв");
            List<T> itemsListLinks = await items.Where(p => p.Name != null).ToListAsync();
            List<VT> itemsList = new List<VT>();
            foreach (T item in itemsListLinks)
            {
                itemsList.Add(item.ToViewModel());
            }
            return itemsList;
        }

        public VT CreateItem(Dictionary<string, object> parameters, DbSet<T> itemsContext)
        {
            T item = StaticItem.Create(parameters);
            itemsContext.Add(item);
            return item.ToViewModel();
        }

        public async Task<Dictionary<bool, string>> BuyItem(int userId, int itemId, decimal money,
            DbSet<T> itemsContext)
        {
            User user;
            T item;
            _cache.TryGetValue(userId.ToString(), out user);
            _cache.TryGetValue(userId.ToString() + $".{typeof(T).FullName}s.{itemId}", out item);

            if (item == null)
            {
                return new Dictionary<bool, string>
                {
                    {false, "item doesnt exist"}
                };
            }

            decimal userInterval = Convert.ToDecimal((DateTime.Now - user.LastRequestDate).TotalMilliseconds);
            bool verifyMoney = userInterval * (user.PassiveCoefficient + 20 * user.ClickCoefficient) / 1000 >= money;

            if (!verifyMoney)
            {
                return new Dictionary<bool, string>
                {
                    {false, "unnormal coint farm"}
                };
            }

            (bool transactionFlag, string message, User user) transactionResult = item.Get(user, money);
            if (!transactionResult.transactionFlag)
            {
                return new Dictionary<bool, string>
                {
                    {false, transactionResult.message}
                };
            }

            transactionResult.user.LastRequestDate = DateTime.Now;
            user = transactionResult.user;
            return new Dictionary<bool, string>
            {
                {true, "succes"}
            };
        }
    }
}