using System.Collections.Generic;

namespace RecipeNavigator.DataSource.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        
        public string RecipeName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductAmount { get; set; }
        public int TicksPerProduct { get; set; }

        public int Ticks { get; set; }

        public Recipe IngredientA { get; set; }
        public int IngredientAAmount { get; set; }

        public Recipe IngredientB { get; set; }
        public int IngredientBAmount { get; set; }
        
        public int TotalTicks { get; set; }
        public int TotalChildrenTicks { get; set; }
        
        public IEnumerable<Upgrade> RequiredUpgrades { get; set; }

        public override string ToString()
        {
            return ToString(0) + "\n";
        }

        public string ToString(int level)
        {
            string str = $"{ProductName} @{TicksPerProduct}t/p : {TotalTicks}t";

            if (IngredientA != null)
            {
                str += "\n " + new string(' ', level * 2) + $"{IngredientAAmount} x {IngredientA.ToString(level + 1)}";
            }

            if (IngredientB != null)
            {
                str += "\n " + new string(' ', level * 2) + $"{IngredientBAmount} x {IngredientB.ToString(level + 1)}";
            }

            if (IngredientA == null && IngredientB == null)
            {
                str += "";
            }

            return str;
        }
    }
}