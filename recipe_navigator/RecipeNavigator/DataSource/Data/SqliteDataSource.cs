using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Dapper;
using RecipeNavigator.DataSource.Models;

namespace RecipeNavigator.DataSource.Data
{
    public class SqliteDataSource : IDataSource, IDisposable
    {
	    private readonly IDbConnection _connection;

	    public SqliteDataSource()
	    {
		    _connection = BuildConnection();
	    }
	    
        public IEnumerable<Product> GetAllProducts()
        {
	        const string sql = @"select rowid as Id, ProductName from Products where rowid > 0";
			return _connection.Query<Product>(sql);
        }

	    public IEnumerable<Upgrade> GetProductUpgrades(int productId)
	    {
		    const string sql = @"
select 
	u.* 
from 
	ProductUpgrades pu
	left join Upgrades u on u.rowid = pu.UpgradeId
where 
	pu.ProductId = @Id";
		    
		    return _connection.Query<Upgrade>(sql, new {Id = productId});
	    }

	    public IEnumerable<Upgrade> GetRequiredUpgradesForRecipe(int recipeId)
	    {
		    const string sql = @"
select 
	u.* 
from 
	RecipeRequiredUpgrades rru
	join Upgrades u on u.rowid = rru.UpgradeId
where
	rru.RecipeId = @Id";
		    
		    return _connection.Query<Upgrade>(sql, new {Id = recipeId});
	    }

	    public IEnumerable<Recipe> GetRecipesForProduct(int productId, int amount = 1)
        {
	        var recipes = new List<Recipe>();
	        
	        const string sql = @"select rowid as Id from Recipes where Product_Id = @Id";
	    
			var recipeIds = _connection.Query<int>(sql, new {Id = productId});

			foreach (var recipeId in recipeIds)
			{
				recipes.Add(BuildRecipe(recipeId, amount));   
			}

	        return recipes;
        }

	    public IEnumerable<Recipe> GetRecipesForIngredient(int ingredientId, int amount = 1)
	    {
		    var recipes = new List<Recipe>();
	        
		    const string sql = @"select rowid as Id from Recipes where IngredientA_Id = @Id or IngredientB_Id = @Id";
		    
			    var recipeIds = _connection.Query<int>(sql, new {Id = ingredientId});

			    foreach (var recipeId in recipeIds)
			    {
				    recipes.Add(BuildRecipe(recipeId, amount));   
			    }

		    return recipes;
	    }
	    
	    public Recipe BuildRecipe(int recipeId, int amount = 1)
        {
            const string sql = @"
select 
	r.rowid as Id,
	r.Tier,
	r.Ticks,
	r.RecipeName,
	pP.rowid as ProductId,
	pP.ProductName as ProductName,
	r.Product_Amount as ProductAmount,

	aP.rowid as IngredientAId,
	aP.ProductName as IngredientA,
	r.IngredientA_Amount as IngredientAAmount,

	bP.rowid as IngredientBId,
	bP.ProductName as IngredientB,
	r.IngredientB_Amount as IngredientBAmount
from 
	Recipes as r
	join Products as pP on pP.rowid = r.Product_Id
	join Products as aP on aP.rowid = r.IngredientA_Id
	join Products as bP on bP.rowid = r.IngredientB_Id
where 
	r.rowid = @Id";

           
                var rootRecipe = _connection.QueryFirstOrDefault<RecipeRaw>(sql, new {Id = recipeId});
	        
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

				recipe.RequiredUpgrades = GetRequiredUpgradesForRecipe(recipeId);

                recipe.TotalChildrenTicks =
                    (recipe.IngredientA?.TotalTicks ?? 0) +
                    (recipe.IngredientB?.TotalTicks ?? 0);

                recipe.TicksPerProduct = recipe.Ticks / recipe.ProductAmount;

                recipe.TotalTicks = recipe.TotalChildrenTicks + (recipe.TicksPerProduct * amount);

                return recipe;
        }

        private IDbConnection BuildConnection()
        {
	        var connectionStringBuilder = new SQLiteConnectionStringBuilder();
	        connectionStringBuilder.DataSource = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "factoria_db.db");
	        connectionStringBuilder.Version = 3;
            return new SQLiteConnection(connectionStringBuilder.ToString());
        }

	    public void Dispose()
	    {
		    _connection?.Dispose();
	    }
    }
}
