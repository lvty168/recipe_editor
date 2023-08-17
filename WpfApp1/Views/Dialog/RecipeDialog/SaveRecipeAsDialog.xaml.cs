using NewRecipeViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewRecipeViewer.Views.Dialog
{
    /// <summary>
    /// SaveRecipeAsDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SaveRecipeAsDialog : Window
    {



        public String new_recipe_name { get; set; }
        public RecipeObject CurrentRecipe { get; set; } 
        public SaveRecipeAsDialog(RecipeObject recipe,String new_recipeName)
        {
            InitializeComponent();
            CurrentRecipe = recipe;
            new_recipe_name = new_recipeName;
        }

        
        private void Accept_Button(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
            this.Close();
        }
    }
}
