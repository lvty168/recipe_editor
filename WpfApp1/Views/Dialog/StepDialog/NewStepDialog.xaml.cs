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
    /// NewStepDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewStepDialog : Window
    {
        public Dictionary<String, int> step_type_dict { get; set; } = new Dictionary<String, int>() { { "Heating", 1}, { "Glow discharge", 2}, { "Ion etching", 3 }, { "Coating", 4 } };
        public StepObject new_step { get; set; }
        public NewStepDialog( StepObject newstep)
        {
            InitializeComponent();
            new_step = newstep;
        }
        /// <summary>
        /// OK Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Button_Click(object sender, RoutedEventArgs e)
        {

            
            this.DialogResult = true;
            this.Close();
        }
    }
}
