using System.Collections.Generic;
using RecipeNavigator.DataSource.Models;

namespace RecipeNavigator.DataSource.Data
{
    public interface IDataSource
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Upgrade> GetProductUpgrades(int productId);
        IEnumerable<Upgrade> GetRequiredUpgradesForRecipe(int recipeId);
        IEnumerable<Recipe> GetRecipesForProduct(int productId, int amount = 1);
        IEnumerable<Recipe> GetRecipesForIngredient(int ingredientId, int amount = 1);
    }
}
