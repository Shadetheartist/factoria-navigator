using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace RecipeNavigator
{
    public class SqlDataSource : IDataSource
    {
        private const string CONNECTION_STRING = "Server=DESKTOP-VPDUO0L\\SQLEXPRESS01;Database=Factoria;Trusted_Connection=True;MultipleActiveResultSets=true";

        public IEnumerable<Product> GetAllProducts()
        {
	        const string sql = @"select Id, ProductName from Products where Id > 0";

	        using (var conn = BuildConnection())
	        {
		        return conn.Query<Product>(sql);
	        }
        }

        public IEnumerable<Recipe> GetRecipesForProduct(int productId, int amount = 1)
        {
	        var recipes = new List<Recipe>();
	        
	        const string sql = @"select Id from Recipes where Product_Id = @Id";
	        using (var conn = BuildConnection())
	        {
		        var recipeIds = conn.Query<int>(sql, new {Id = productId});

		        foreach (var recipeId in recipeIds)
		        {
			        recipes.Add(BuildRecipe(recipeId, amount));   
		        }
	        }

	        return recipes;
        }

	    public IEnumerable<Recipe> GetRecipesForIngredient(int ingredientId, int amount = 1)
	    {
		    var recipes = new List<Recipe>();
	        
		    const string sql = @"select Id from Recipes where IngredientA_Id = @Id or IngredientB_Id = @Id";
		    using (var conn = BuildConnection())
		    {
			    var recipeIds = conn.Query<int>(sql, new {Id = ingredientId});

			    foreach (var recipeId in recipeIds)
			    {
				    recipes.Add(BuildRecipe(recipeId, amount));   
			    }
		    }

		    return recipes;
	    }

	    public static Recipe BuildRecipe(int recipeId, int amount = 1)
        {
            const string sql = @"
select 
	r.Id,
	r.Tier,
	r.Ticks,
	r.RecipeName,
	pP.Id as ProductId,
	pP.ProductName as ProductName,
	r.Product_Amount as ProductAmount,

	aP.Id as IngredientAId,
	aP.ProductName as IngredientA,
	r.IngredientA_Amount as IngredientAAmount,

	bP.Id as IngredientBId,
	bP.ProductName as IngredientB,
	r.IngredientB_Amount as IngredientBAmount
from 
	Recipes as r
	join Products as pP on pP.Id = r.Product_Id
	join Products as aP on aP.Id = r.IngredientA_Id
	join Products as bP on bP.Id = r.IngredientB_Id
where 
	r.Id = @Id";

            using (var conn = BuildConnection())
            {
                var rootRecipe = conn.QueryFirstOrDefault<RecipeRaw>(sql, new {Id = recipeId});
                if (rootRecipe == null)
                {
                    return null;
                }

                var recipe = new Recipe();
                recipe.Id = rootRecipe.Id;
                recipe.ProductId = rootRecipe.ProductId;
                recipe.Ticks = rootRecipe.Ticks;
                recipe.ProductName = rootRecipe.ProductName;
	            recipe.ProductAmount = rootRecipe.ProductAmount;
	            recipe.RecipeName = rootRecipe.RecipeName;

                recipe.IngredientAAmount = rootRecipe.IngredientAAmount * amount;
                recipe.IngredientBAmount = rootRecipe.IngredientBAmount * amount;
                recipe.IngredientA = BuildRecipe(rootRecipe.IngredientAId, recipe.IngredientAAmount);
                recipe.IngredientB = BuildRecipe(rootRecipe.IngredientBId, recipe.IngredientBAmount);


                recipe.TotalChildrenTicks =
                    (recipe.IngredientA?.TotalTicks ?? 0) +
                    (recipe.IngredientB?.TotalTicks ?? 0);

                recipe.TicksPerProduct = recipe.Ticks / recipe.ProductAmount;

                recipe.TotalTicks = recipe.TotalChildrenTicks + (recipe.TicksPerProduct * amount);

                return recipe;
            }
        }

        private static SqlConnection BuildConnection()
        {
            return new SqlConnection(CONNECTION_STRING);
        }
    }
}
