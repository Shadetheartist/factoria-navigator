using System.Collections.Generic;

namespace RecipeNavigator
{
    public interface IDataSource
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Recipe> GetRecipesForProduct(int productId, int amount = 1);
        IEnumerable<Recipe> GetRecipesForIngredient(int ingredientId, int amount = 1);
    }
}
