using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows.Input;
using NewRecipeViewer.Views.Dialog.StepDialog;
using Microsoft.Toolkit.Mvvm.Input;
using System.Globalization;
using System.Threading;

namespace NewRecipeViewer.ViewModels
{
    class StepViewModel : ObservableRecipient, IRecipient<RecipeObject>
    {
        private RecipeObject current_recipe;
        private ObservableCollection<StepObject> _steps = new ObservableCollection<StepObject>();
        public ObservableCollection<StepObject> steps
        {
            get { return _steps; }
            set
            {
                _steps = value;
                OnPropertyChanged();
            }
        }
        private StepObject copy_step;

        public StepObject Copy_step
        {
            get { return copy_step; }
            set { copy_step = value; OnPropertyChanged(); } 
        }
        private StepObject current_step;
        public StepObject Current_step
        {
            get { return current_step; }
            set 
            { 
                current_step = value;
                
                OnPropertyChanged();
                if (Current_step != null)
                {
                    WeakReferenceMessenger.Default.Send<StepObject>(Current_step);
                    WeakReferenceMessenger.Default.Send<string, string>(current_step.Step_type.ToString(), "StepTypeChanged");
                }
                

            }
        }
        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }   
        }

        #region Command
        public ICommand NewStepCommand { get; }
        public ICommand EditStepCommand { get; }
        public ICommand DeleteStepCommand { get; }
        public ICommand CopyStepCommand { get; }
        public ICommand PasteStepCommand { get; }
        public ICommand PrintStepCommand { get; }
        
        public ICommand AddStepToProcessSequenceCommand { get; }
        public ICommand InsertStepToProcessSequenceCommand { get; }
        public ICommand ReplaceStepToProcessSequenceCommand { get; }
        public ICommand RemoveStepToProcessSequenceCommand { get; }
        #endregion

        public StepViewModel()
        {
            this.IsActive = true;
            CopyStepCommand = new RelayCommand(() => { Copy_step = Current_step;  });
            EditStepCommand = new RelayCommand(EditStep);
            NewStepCommand = new RelayCommand(AddStep);
            PasteStepCommand = new RelayCommand(PasteStep);
            PrintStepCommand = new RelayCommand(PrintStep);
            DeleteStepCommand = new RelayCommand(DeleteStep);
            AddStepToProcessSequenceCommand = new RelayCommand(() => { WeakReferenceMessenger.Default.Send<String, String>("Add", "AddStepSequence"); });
            InsertStepToProcessSequenceCommand = new RelayCommand(() => { WeakReferenceMessenger.Default.Send<String, String>("Insert", "InsertStepSequence"); });
            ReplaceStepToProcessSequenceCommand = new RelayCommand(() => { WeakReferenceMessenger.Default.Send<String, String>("Replace", "ReplaceStepSequence"); });
            RemoveStepToProcessSequenceCommand = new RelayCommand(() => { WeakReferenceMessenger.Default.Send<String, String>("Remove", "RemoveStepSequence"); });
            WeakReferenceMessenger.Default.Register<String, String>(this, "flash_step", FlashStep);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
        }

        private void IDreleaseChanged(object recipient, string idrelease)
        {
            /*
                1	operator	operator
                2	supervisor	supervisor
                3	technician	technician
                4	developer	developer
                5	administrator	administrator
             */
            if (int.Parse(idrelease) == 6)
            {
                Edit_enable = true;
                
            }
            else
            {
                Edit_enable = false;
                
            }
        }

        private void loadStepsFromDB(RecipeObject recipe)
        {
            if (recipe ==null)  
            {
                return;
            }
            String commandstr = "SELECT Date_Time,Step_Name,Step_type,Name "
               + "FROM " + "工艺配方"
               + " where [Name] = '" + recipe.Rev_name.Trim() + "';";
            // String con_str = "DSN=" + "工艺配方" + ";";
            OdbcConnection connection = new OdbcConnection("DSN=工艺配方;");
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            connection.Open();
            steps.Clear();
            var reader =command.ExecuteReader();
            while(reader.Read())
            {
                steps.Add(new StepObject()
                {

                    Step_name = reader[1].ToString(),
                    Step_type = Convert.ToInt16(reader[2].ToString()),
                    Step_date = reader[0].ToString(),
                    Rev_name = reader[3].ToString()
                });
            }
            connection.Close();
            WeakReferenceMessenger.Default.Send<ObservableCollection<StepObject>>(steps);
        }

        /// <summary>
        /// 配方变更 消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(RecipeObject recipe)
        {
            current_recipe = recipe;
            loadStepsFromDB(recipe);
        }

        #region Action
        private void AddStep()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
            StepObject newstep = new StepObject()
            {
                Step_date = DateTime.Now.ToString(),
                Step_name = "new_step",
                Step_type = 1,
                Rev_name = current_recipe.Rev_name
            };

            NewStepDialog Dialog = new NewStepDialog(newstep);
            Nullable<Boolean> ret = Dialog.ShowDialog();
            if (ret == true)
            {
                WeakReferenceMessenger.Default.Send<StepObject, String>(newstep, "AddStep");
            }
        }
        private void EditStep()
        {
            StepObject step = Copy.DeepCopyByReflect(Current_step);
            step.Step_name = step.Step_name.Trim();
            EditStepDialog dialog = new EditStepDialog(step);
            Nullable<Boolean> ret = dialog.ShowDialog();
            if (ret == true)
            {
                WeakReferenceMessenger.Default.Send<StepObject, String>(step, "EditStep");
            }
        }
        private void DeleteStep()
        {
            //检查步骤是不是被工艺流程队列(ProcessSequence)调用
            OdbcConnection processConnection = new OdbcConnection("DSN=配方排序;");
            OdbcCommand command = new OdbcCommand();
            String commandStr = String.Format("Select * From 配方排序 Where Name='{0}' And Step_Name='{1}';",
                current_recipe.Rev_name.Trim(), Current_step.Step_name.Trim());
            command.Connection = processConnection;
            command.CommandText = commandStr;
            //OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            //DataTable data = new DataTable();
            
            processConnection.Open();
            var reader = command.ExecuteReader();
            reader.Read();
            //dataAdapter.Fill(data);
            if (reader.HasRows)
            {
                MessageBox.Show("It cannot be deleted. It is being referenced by the process queue./n " +
                    "Please remove it from the process queue first.",
                    "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //提醒操作者是否要删除配方步骤
            MessageBoxResult ret = MessageBox.Show(String.Format("{0} will be deleted.", Current_step.Step_name.Trim()),
                "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (ret == MessageBoxResult.Yes)
            {
                WeakReferenceMessenger.Default.Send<StepObject, String>(Current_step, "DeleteStep");
            }
        }
        private void PasteStep()
        {
            bool temp = true;
            var new_step = Copy.DeepCopyByReflect(Copy_step);
            new_step.Step_date = DateTime.Now.ToString();
            new_step.Step_name = new_step.Step_name.Trim();
            while (temp)
            {
                var connection = new OdbcConnection("DSN=工艺配方;");
                string command_str = string.Format("Select * From 工艺配方 Where Name='{0}' and Step_Name='{1}';",current_recipe.Rev_name,new_step.Step_name);
                var command =  new OdbcCommand(command_str,connection);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    var dialog = new NewStepDialog(new_step);
                    var ret = dialog.ShowDialog();
                    if (ret == false)
                    {
                        return;
                    }
                   
                }
                else
                {
                    temp = false;
                }
                connection.Close();
            }

            //配方步骤的粘贴
            WeakReferenceMessenger.Default.Send<Tuple<StepObject, StepObject>, String>(
                Tuple.Create<StepObject, StepObject>(Copy_step,new_step), "PasteStep");
        }
        private void PrintStep()
        {

        }
        private void FlashStep(object recipient, string message)
        {
            
            loadStepsFromDB(current_recipe);
            switch (message)
            {
                case "delete":
                    if (steps.Count > 0)
                    {
                        Current_step = steps[0];

                    }
                    break;
                default:
                    foreach (StepObject item in steps)
                    {
                        if (item.Step_name.Trim() == message)
                        {
                            Current_step = item;
                            break;
                        }
                    }
                    break;
            }
            
            //WeakReferenceMessenger.Default.Send<StepObject>(Current_step);
        }
           
        #endregion


    }
}
