using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RecipeNavigator.DataSource.Data;
using RecipeNavigator.DataSource.Models;
using Terminal.Gui;

namespace RecipeNavigator.ConsoleInterface.Views
{
    public class RecipeView : FrameView
    {
        private readonly IDataSource _dataSource;
        
        private readonly RecipesView _recipesView;
        private readonly RecipesView _usedInView;
        private readonly ListView _upgradeView;

        public RecipeView(IDataSource dataSource) : base("Recipes")
        {
            _dataSource = dataSource;

            var recipesFrame = new FrameView("Recipe")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(35),
                Height = Dim.Percent(33),
            };
            
            var usedInFrame = new FrameView("Used In")
            {
                X = 0,
                Y = Pos.Bottom(recipesFrame),
                Width = Dim.Percent(35),
                Height = Dim.Percent(50),
            };
            
            var upgradeFrame = new FrameView("Upgrade")
            {
                X = 0,
                Y = Pos.Bottom(usedInFrame),
                Width = Dim.Percent(35),
                Height = Dim.Fill(),
            };
            
            var contentFrame = new FrameView("Content")
            {
                X = Pos.Right(recipesFrame),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            
            var contentView1 = new Label("")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            
            _recipesView = new RecipesView(new List<string>(), contentView1)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            
            _usedInView = new RecipesView(new List<string>(), contentView1)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            
            _upgradeView = new RecipesView(new List<string>(), contentView1)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            recipesFrame.Add(_recipesView);
            usedInFrame.Add(_usedInView);
            upgradeFrame.Add(_upgradeView);

            contentFrame.Add(contentView1);
            
            base.Add(recipesFrame);
            base.Add(usedInFrame);
            base.Add(upgradeFrame);
            base.Add(contentFrame);
           
        }

        public void SetProduct(int productId)
        {
            _recipesView.SetRecipies(_dataSource.GetRecipesForProduct(productId).ToList());
            _usedInView.SetRecipies(_dataSource.GetRecipesForIngredient(productId).ToList());
            _upgradeView.SetSource(_dataSource.GetProductUpgrades(productId).ToList());
            
            _recipesView.UpdateOutput();
        }

        public void UpdateOutputProduct()
        {
            _recipesView.UpdateOutput();
        }

    }

    class RecipeShort
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    class RecipesView : ListView
    {
        private IList<Recipe> recipes;
        private readonly Label _outputView;

        public RecipesView(IList source, Label outputView) : base(source)
        {
            _outputView = outputView;
            SelectedChanged += UpdateOutput;
        }

        public void SetRecipies(IList<Recipe> _recipes)
        {
            recipes = _recipes;
            
            SetSource(recipes.Select(e => new RecipeShort
            {
                Id = e.Id,
                Name = e.RecipeName
            }).ToList());
        }

        public override bool HasFocus {
            get
            {
                var hasFocus = base.HasFocus;
                
                if (hasFocus)
                {
                    UpdateOutput();
                }

                return hasFocus;
            }
        }

        public void UpdateOutput()
        {
            if (recipes?.Count > 0)
            {
                _outputView.Text = recipes[SelectedItem].ToString() ?? "";
            }
        }
    }
}
