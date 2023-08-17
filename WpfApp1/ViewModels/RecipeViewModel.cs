using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using NewRecipeViewer.Views.Dialog;

namespace NewRecipeViewer.ViewModels
{
    class RecipeViewModel:ObservableRecipient
    {
        private OdbcConnection connection = new OdbcConnection("DSN=工艺名称;");
        private ObservableCollection<RecipeObject> _recipes = new ObservableCollection<RecipeObject>();
        public Dictionary<String, int> select_items { get; set; } = new Dictionary<String, int>() { { "No", 0 }, { "Yes", 1 } };
        private RecipeObject currentRecipe;
        public RecipeObject CurrentRecipe
        {
            get { return currentRecipe; }
            set
            {
                if (value != null)  
                {
                    currentRecipe = value;
                    WeakReferenceMessenger.Default.Send<RecipeObject>(CurrentRecipe);
                    WeakReferenceMessenger.Default.Send<string, string>(currentRecipe.IDrelease.ToString(), "IDreleaseChanged");
                    if (currentRecipe.IDrelease == 6)
                    {
                        Edit_enable = true;
                    }
                    else
                    {
                        Edit_enable = false;
                    }
                }
                
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RecipeObject> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged();
            }
        }
        #region 事件
        public ICommand NewRecipeCommand { get; }
        public ICommand RecipeHandelEditCommand { get; }
        public ICommand DeleteRecipeCommand { get; }
        public ICommand SaveAsRecipeCommand { get; }
        public ICommand PrintRecipeCommand { get; }
        public ICommand SaveActualRecipeCommand { get; }

        #endregion
        public RecipeViewModel()
        {
            this.IsActive = true;
            NewRecipeCommand = new RelayCommand(New_recipe_Action);
            RecipeHandelEditCommand = new RelayCommand(EditRecipeHandle);
            DeleteRecipeCommand = new RelayCommand(Delete_recipe_Action);
            SaveAsRecipeCommand = new RelayCommand(Save_recipe_as_Action);
            //PrintRecipeCommand = new RelayCommand(New_recipe_Action);
            //SaveActualRecipeCommand = new RelayCommand(New_recipe_Action);
            WeakReferenceMessenger.Default.Register<String, String>(this, "flash_recipe", flash_recipe);
            loadRecipesFromDB();
        }
        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }   
        }

        #region Action
        private void loadRecipesFromDB()
        {

            String SelectCommandString = "Select Rev,Name,Date_Time,Releasr_level,Venting_tenperature,Cathode_uniformity,Substrate_cooling,User "
                + "from " + "工艺名称" + ";";
            OdbcCommand select_command = new OdbcCommand(SelectCommandString, connection);
            //OdbcDataAdapter dataAdapter = new OdbcDataAdapter(select_command);
            //DataTable recipes_table = new DataTable();

            connection.Open();
            Recipes.Clear();
            var reader = select_command.ExecuteReader();
            while (reader.Read())
            {
                Recipes.Add(new RecipeObject()
                {
                    Rcp_rev = Convert.ToInt16(reader[0].ToString()),
                    Rev_name = reader[1].ToString(),
                    Rev_date = reader[2].ToString(),
                    IDrelease = CustomerConverts.ConvertIdRelease(reader[3].ToString()),
                    VentingTemp = Convert.ToInt16(reader[4].ToString()),
                    Cath_unif = Convert.ToInt16(reader[5].ToString()),
                    Coldtrap = Convert.ToInt16(reader[6].ToString()),
                    UseEmRcp = Convert.ToInt16(reader[7].ToString()),
                });
            }
            //dataAdapter.Fill(recipes_table);
            connection.Close();
            
        }
        private void EditRecipeHandle()
        {
            RecipeObject Edit_recipe = Copy.DeepCopyByReflect<RecipeObject>(CurrentRecipe);
            EditRecipeDialog dialog = new EditRecipeDialog(Edit_recipe);
            try
            {
                Nullable<bool> dialogResult = dialog.ShowDialog();
                if (dialogResult == true)
                {
                    //将配方更改写入
                    WeakReferenceMessenger.Default.Send<RecipeObject, String>(Edit_recipe, "EditRecipe");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void New_recipe_Action()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
            RecipeObject new_recipe = Copy.DeepCopyByReflect<RecipeObject>(CurrentRecipe);
            if (CurrentRecipe != null)
            {
                new_recipe.Rev_name = CurrentRecipe.Rev_name.Trim() + "_1";
            }
            else
            {
                new_recipe = new RecipeObject() { Rev_name = "new_recipe" };
            }
            new_recipe.Rev_date = DateTime.Now.ToString();
            new_recipe.IDrelease = 6;
            new_recipe.Cath_unif = 1;
            new_recipe.Coldtrap = 1;
            try
            {
                
                NewRecipeDialog dialog = new NewRecipeDialog(new_recipe);
                Nullable<bool> dialogResult = dialog.ShowDialog();
                if (dialogResult == true)
                {
                    //检查是否有同名配方
                    bool ret = DuplicateNameDetection(new_recipe.Rev_name);
                    //发送创建新配方请求
                    if (ret)
                    {
                        WeakReferenceMessenger.Default.Send<RecipeObject, String>(new_recipe, "AddRecipe");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 新增配方的重名检测
        /// </summary>
        /// <param name="newRecipeName">新的配方名称</param>
        /// <returns></returns>
        private bool DuplicateNameDetection(String newRecipeName)
        {
            String CheckString = "Select * From 工艺名称 Where Name=" + String.Format("'{0}';", newRecipeName.Trim());
            OdbcCommand command = new OdbcCommand(CheckString, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            
            reader.ReadAsync();
            
            //int ret = command.ExecuteNonQuery();
            if (reader.HasRows)
            {
                MessageBox.Show("Recipe with the same name exists.", "Warming", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }
            connection.Close();
            return true;
        }

        private void Save_recipe_as_Action()
        {
            //RecipeObject new_recipe = Copy.DeepCopyByReflect<RecipeObject>(CurrentRecipe);
            
            String new_recipe_name = CurrentRecipe.Rev_name.Trim() + "_1";
            SaveRecipeAsDialog dialog = new SaveRecipeAsDialog(CurrentRecipe, new_recipe_name);
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                new_recipe_name = dialog.new_recipe_name;
                //判断配方是否重名
                bool ret =  DuplicateNameDetection(new_recipe_name);
                //发送另存为配方请求
                if (ret)
                {
                    WeakReferenceMessenger.Default.Send<String, String>(new_recipe_name, "SaveAsRecipe");
                }



            }
        }

        private void Delete_recipe_Action()
        {

            MessageBoxResult result = MessageBox.Show("All reversion and step data will be delete!\n "
                + "this action cannon be undone. Do you want to Proceed?", "Delete Recipe",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                WeakReferenceMessenger.Default.Send<RecipeObject, String>(CurrentRecipe, "DeleteRecipe");
            }
        }

        public void flash_recipe(object recipient, String actionString)
        {
           
            loadRecipesFromDB();
            switch (actionString)
            {
                case "delete":

                    this.CurrentRecipe = Recipes[0];
                    WeakReferenceMessenger.Default.Send<RecipeObject>(Recipes[0]);
                    break;
                default:
                    foreach (RecipeObject item in Recipes)
                    {
                        String str = item.Rev_name.Trim();
                        if (str == actionString)
                        {
                            this.CurrentRecipe = item;
                            WeakReferenceMessenger.Default.Send<RecipeObject>(item);
                            return;
                        }
                    }
                    break;
            }
            
        }
        #endregion


    }
}
