using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace RecipeNavigator
{
    public class ProductSelectorView : FrameView
    {
        private readonly ListView _listView;
        private readonly List<Product> products;

        public event Action<int> ProductChanged;
        
        public ProductSelectorView(IDataSource dataSource) : base("Products")
        {
            products = dataSource.GetAllProducts().OrderBy(e => e.Id).ToList();

            _listView = new ListView(products);

            _listView.SelectedChanged += () =>
            {
                ProductChanged?.Invoke(products[_listView.SelectedItem].Id);
            };

            base.Add(_listView);
        }

        public override bool HasFocus {
            get
            {
                var hasFocus = base.HasFocus;
                
                if (hasFocus)
                {
                    Program.recipeView.UpdateOutputProduct();
                }

                return hasFocus;
            }
        }
    }
}
