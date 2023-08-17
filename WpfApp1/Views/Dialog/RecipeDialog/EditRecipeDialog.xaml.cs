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
    /// EditRecipeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditRecipeDialog : Window
    {


        public RecipeObject CurrentRecipe { get; set; }

        // Using a DependencyProperty as the backing store for CurrentRecipe.  This enables animation, styling, binding, etc...


        public Dictionary<String, int> select_items { get; set; } = new Dictionary<String, int>() { { "No", 0 }, { "Yes", 1 } };
        //public Dictionary<String, Double> Release_items { get; set; } = new Dictionary<String, Double>() { { "operator", 1 }, { "supervisor", 2 }, { "technician", 3 }, { "developer", 4 }, { "administrator", 5 } };
        public Dictionary<String, int> Release_items { get; set; } = new Dictionary<String, int>() { { "Operator", 1 }, { "Supervisor", 2 }, { "Technician", 3 }, { "Developer", 4 }, { "Administrator", 5 }, { "Design Mode", 6 } };
        public EditRecipeDialog(RecipeObject recipe)
        {
            InitializeComponent();
            CurrentRecipe = recipe;
        }

        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {

            //关闭窗口
            this.DialogResult = true;
            this.Close();
        }        
    }
}
