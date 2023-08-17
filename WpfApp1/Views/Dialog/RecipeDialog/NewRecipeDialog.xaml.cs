using System;
using System.Collections.Generic;
using System.Data.Odbc;
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
    /// NewRecipeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewRecipeDialog : Window
    {
        public RecipeObject new_recipe { get; set; }    
        public NewRecipeDialog(RecipeObject recipe)
        {
            InitializeComponent();
            new_recipe = recipe;
        }
        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            //RecipeObject new_recipe = new RecipeObject() { Cath_unif = 1,IDrelease = 5,Rev_name=new_recipe_name.Text,Rev_date=DateTime.Now.ToString()};
            this.DialogResult = true;
            this.Close();
        }
        
    }
}
