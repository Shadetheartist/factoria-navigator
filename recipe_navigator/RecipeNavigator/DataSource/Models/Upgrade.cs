namespace RecipeNavigator.DataSource.Models
{
    public class Upgrade
    {
        public int Id { get; set; }
        public string UpgradeName { get; set; }
        public int UpgradeType { get; set; }
        public int Stage { get; set; }

        public override string ToString()
        {
            return UpgradeName;
        }
    }
}
