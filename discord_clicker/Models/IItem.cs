namespace discord_clicker.Models {
    public interface IItem<VT>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VT ToViewModel();
        public (bool, string, User) Get(User user, decimal money);
    }
}