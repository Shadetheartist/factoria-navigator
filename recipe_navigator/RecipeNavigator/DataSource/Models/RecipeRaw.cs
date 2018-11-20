namespace RecipeNavigator.DataSource.Models
{
    public class RecipeRaw
    {
        public int Id { get; set; }
        public int Tier { get; set; }
        public int Ticks { get; set; }
        public string RecipeName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductAmount { get; set; }
        
        public int IngredientAId { get; set; }
        public string IngredientAName { get; set; }
        public int IngredientAAmount { get; set; }
        
        public int IngredientBId { get; set; }
        public string IngredientBName { get; set; }
        public int IngredientBAmount { get; set; }

        public override string ToString()
        {
            return $"{ProductName} x {ProductAmount}: ({IngredientAName} x {IngredientAAmount}) + ({IngredientBName} x {IngredientBAmount})";
        }
    }
}
