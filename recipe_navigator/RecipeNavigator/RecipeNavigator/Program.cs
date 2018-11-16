using System;
using Terminal.Gui;

namespace RecipeNavigator
{
    class Program
    {
        public static ProductSelectorView productSelectorView;
        public static RecipeView recipeView;

        static void Main(string[] args)
        { 
            //Application.UseSystemConsole = true;
            Application.Init ();

            var top = Application.Top;

            var win = new Window ("Recipe Navigator"){
                X = 0,
                Y = 0,
                Width = Dim.Fill (),
                Height = Dim.Fill ()
            };			

            var dataSource = new SqliteDataSource();

            productSelectorView = new ProductSelectorView(dataSource)
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(25),
                Height = Dim.Fill()
            };
            
            recipeView = new RecipeView(dataSource)
            {
                X = Pos.Right(productSelectorView),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            productSelectorView.ProductChanged += productId =>
            {
                recipeView.SetProduct(productId);
            };
            
            win.Add(productSelectorView);
            win.Add(recipeView);

            top.Add (win);

            Application.Run();
        }
    }
}
