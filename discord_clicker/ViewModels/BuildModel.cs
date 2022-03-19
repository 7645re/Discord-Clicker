namespace discord_clicker.ViewModels
{
    public class BuildModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        #nullable enable
        public string? Description { get; set; }
        public decimal PassiveCoefficient { get; set; }
    }
}
