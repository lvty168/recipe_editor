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
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace NewRecipeViewer.ViewModels
{
    class ProcessSequenceViewModle : ObservableRecipient, IRecipient<RecipeObject>
    {
        private RecipeObject current_recipe;
        private ObservableCollection<Process_SequenceObject> _recipe_sequence
            = new ObservableCollection<Process_SequenceObject>();
        public ObservableCollection<Process_SequenceObject> RecipeSequence
        {
            get { return _recipe_sequence; }
            set
            {
                _recipe_sequence = value;
                OnPropertyChanged();
            }
        }
        private Process_SequenceObject current_process;

        public Process_SequenceObject Current_process
        {
            get { return current_process; }
            set 
            { 
                current_process = value;
                WeakReferenceMessenger.Default.Send<Process_SequenceObject>(Current_process);
                OnPropertyChanged(); 
                
            }
        }

        public void Receive(RecipeObject recipe)
        {
            current_recipe = recipe;
            GetRecipeSequenceFromDB();
        }
        #region Command
        public ICommand AddStepCommand { get; } =new RelayCommand(() => { });
        public ICommand InsertStepCommand { get; }
        public ICommand ReplaceStepCommand { get; }
        public ICommand RemoveStepCommand { get; }
        public ICommand RepetitionSetCommand { get; }
        public ICommand RepetitionRemoveCommand { get; }
        #endregion

        public ProcessSequenceViewModle()
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<String, String>(this, "flash_process_sequeue", FlashProcessSequeue);
        }

        private void GetRecipeSequenceFromDB()
        {
            if (current_recipe == null) 
            {
                return;
            }
            String command_str = "SELECT SEQ_NO,Block,Step_Name,Name,Step_type FROM "
                + "配方排序" + " where Name = '" + current_recipe.Rev_name.Trim()
                + "' order by SEQ_NO Asc;";
            OdbcConnection connection = new OdbcConnection("DSN=配方排序;");
            OdbcCommand command = new OdbcCommand(command_str, connection);
            //OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);

            //DataTable sequeue_table = new DataTable();
            connection.Open();
            RecipeSequence.Clear();
            //var reader = await command.ExecuteReaderAsync();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                RecipeSequence.Add(new Process_SequenceObject()
                {
                    Seq_no = Convert.ToInt16(reader[0].ToString()),
                    Rep_blok = Convert.ToInt16(reader[1].ToString()),
                    Step_name = reader[2].ToString(),
                    Rev_name = reader[3].ToString(),
                    Step_type = Convert.ToInt16(reader[4].ToString())

                });
            }
            //dataAdapter.Fill(sequeue_table);

            connection.Close();
            WeakReferenceMessenger.Default.Send<string, string>(RecipeSequence.Count.ToString(), "ProcessCount");
            WeakReferenceMessenger.Default.Send<ObservableCollection<Process_SequenceObject>, string>(
                RecipeSequence,"ProcessSequence");
            
            
        }
        private void FlashProcessSequeue(Object obj, String message)
        {
            //Task.Run();
            GetRecipeSequenceFromDB();
        }

    }
}
