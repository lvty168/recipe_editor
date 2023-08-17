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

namespace NewRecipeViewer.Views.Dialog.StepDialog
{
    /// <summary>
    /// EditStepDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditStepDialog : Window
    {
        public StepObject current_step { get; set; }
        public EditStepDialog(StepObject step)
        {
            InitializeComponent();
            current_step = step; 
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
