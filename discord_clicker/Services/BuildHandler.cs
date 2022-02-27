using discord_clicker.Models;
using discord_clicker.ViewModels;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;


namespace discord_clicker.Services {
    public class BuildHandler : IItemHandler<BuildModel> {
        private UserContext _db;
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IConfiguration _сonfiguration;
        public BuildHandler(UserContext context, IMemoryCache memoryCache, IConfiguration configuration, ILogger<BuildHandler> logger)
        {
            _db = context;
            _logger = logger;
            _cache = memoryCache;
            _сonfiguration = configuration;
        }
        public async Task<List<BuildModel>> GetItemsList(int userId) {
            List<BuildModel> buildsList;
            /** Сhecking for data in the cache */
            bool availabilityСache = _cache.TryGetValue(userId.ToString() + ".buildsList", out buildsList);
            if (!availabilityСache)
            {
                buildsList = new List<BuildModel>();
                List<Build> buildsListLinks = await _db.Builds.Where(p => p.Name != null).Include(p => p.UserBuilds).ToListAsync();
                foreach (Build build in buildsListLinks)
                {
                    /** Function create new instance cuz i select data with relationships many-to-many.
                     *  Serializer work recursive and go to infinity loop cuz tables have link to each other.
                     *  if i need again recreate instance i will use automapper */
                    buildsList.Add(new BuildModel()
                    {
                        Id = build.Id,
                        Cost = build.Cost,
                        Name = build.Name,
                        Description = build.Description,
                        PassiveCoefficient = build.PassiveCoefficient,
                        Count = build.UserBuilds.Where(p => p.UserId == userId).ToList().Count > 0 ? build.UserBuilds.First().Count : 0
                    });
                }
                /** Set option to never remove user from cache */
                _cache.Set(userId.ToString() + ".buildsList", buildsList, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return buildsList;
        }
        // public Task<List<BuildModel>> BuyItem(uint userId) {
        //     List<BuildModel> buildsModelList = new List<BuildModel>();
        //     return buildsModelList;
        // }
    }
}