using AutoMapper;
using discord_clicker.Models.Items.AchievementClasses;
using discord_clicker.Models.Items.BuildClasses;
using discord_clicker.Models.Items.UpgradeClasses;

namespace discord_clicker.Models.Items;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Build, BuildViewModel>();
        CreateMap<Upgrade, UpgradeViewModel>();
        CreateMap<Achievement, AchievementViewModel>();
        CreateMap<BuildCreateModel, Build>();
        CreateMap<UpgradeCreateModel, Upgrade>();
        CreateMap<AchievementCreateModel, Achievement>();
        CreateMap<Build, BuildViewModel>();
        CreateMap<Upgrade, UpgradeViewModel>();
        CreateMap<Achievement, AchievementViewModel>();
    }
}