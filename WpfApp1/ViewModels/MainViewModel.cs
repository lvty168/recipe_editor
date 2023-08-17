using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NewRecipeViewer.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using NewRecipeViewer.Service;
using System.Windows;

namespace NewRecipeViewer.ViewModels
{
    class MainViewModel : ObservableRecipient, IRecipient<RecipeObject>,IRecipient<StepObject>
    {
        private object pageContent ;

        public object PageContent
        {
            get { return pageContent; }
            set
            {
                pageContent = value;
                OnPropertyChanged();
            }
        }
        private RecipeObject current_recipe;

        public RecipeObject Current_recipe
        {
            get { return current_recipe; }
            set { current_recipe = value; OnPropertyChanged(); }
        }

        private StepObject current_step;

        public StepObject Current_step
        {
            get { return current_step; }
            set { current_step = value; OnPropertyChanged(); }
        }
        private string step_type;

        public string Step_type
        {
            get { return step_type; }
            set { step_type = value; OnPropertyChanged(); } 
        }

        private bool parameter_loaded;

        public bool Parameter_loaded
        {
            get { return parameter_loaded; }
            set { parameter_loaded = value; OnPropertyChanged(); }
        }
        #region 页面控件
        private RecipePageControl recipe_control = new RecipePageControl();

        public RecipePageControl Recipe_Control
        {
            get { return recipe_control; }
            set
            {
                recipe_control = value;
                OnPropertyChanged();
            }
        }

        private BiasPageControl bias_control = new BiasPageControl();

        public BiasPageControl Bias_Control
        {
            get { return bias_control; }
            set
            {
                bias_control = value;
                OnPropertyChanged();
            }
        }
        private CathodePageControl cathodes_control = new CathodePageControl();

        public CathodePageControl Cathodes_control
        {
            get { return cathodes_control; }
            set
            {
                cathodes_control = value;
                OnPropertyChanged();
            }
        }
        
        private GasFlowPageControl gas_flow_control = new GasFlowPageControl();

        public GasFlowPageControl Gas_flow_Control
        {
            get { return gas_flow_control; }
            set
            {
                gas_flow_control = value;
                OnPropertyChanged();
            }
        }
        private DurationPageControl duration_control = new DurationPageControl();

        public DurationPageControl Duration_control
        {
            get { return duration_control; }
            set
            { 
                duration_control = value;
                OnPropertyChanged();
            }
        }
        private HeatingPageControl heating_control = new HeatingPageControl();

        public HeatingPageControl Heating_control
        {
            get { return heating_control; }
            set { heating_control = value; OnPropertyChanged(); }
        }
        private StepPageControl step_control =new StepPageControl();

        public StepPageControl Step_control
        {
            get { return step_control; }
            set { step_control = value; OnPropertyChanged(); }
        }

        private UbmPlmControl ubm_plm_control = new UbmPlmControl();

        public UbmPlmControl Ubm_plm_control
        {
            get { return ubm_plm_control; }
            set { ubm_plm_control = value; OnPropertyChanged(); }
        }
        #endregion
        private EditActions db_action = new EditActions();

        public EditActions DbAction
        {
            get { return db_action = new EditActions(); }
            set { db_action = value; }
        }

        #region Enable control
        private bool load_step_enable;

        public bool Load_step_enable
        {
            get { return load_step_enable; }
            set { load_step_enable = value; OnPropertyChanged(); }  
        }
        private bool save_parameter_enable;

        public bool Save_parameter_enable
        {
            get { return save_parameter_enable; }
            set { save_parameter_enable = value; OnPropertyChanged(); }
        }

        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }   
        }
        private string edit_status; 

        public string Edit_status
        {
            get { return edit_status; }
            set { edit_status = value; OnPropertyChanged(); }
        }

        #endregion

        //private ParameterViewModel2 parameterViewModel2 =new ParameterViewModel2();
        public ICommand ExitCommand { get; }
        public ICommand SaveParameterCommand { get; }
        public ICommand RecipePageSelectedCommand { get; }
        
        public ICommand ComparePageSelectedCommand { get; }
        public ICommand StepPageSelectedCommand { get; }
        public ICommand LoadStepParameterFromDBCommand { get; }
        public ICommand LoadPlcActProcessCommand { get; }

        public ICommand BiasPageSelectedCommand { get; }
        public ICommand CathodePageSelectedCommand { get; }
        public ICommand GasFlowPageSelectedCommand { get; }
        public ICommand DurationPageSelectedCommand { get; }
        public ICommand HeatingPageSelectedCommand { get; }
        public ICommand Ubm_PlmPageSelectedCommand { get; }

        public MainViewModel()
        {
            ExitCommand = new RelayCommand(()=> {
                Application.Current.Shutdown();
            });
            SaveParameterCommand = new RelayCommand(
                ()=> {
                    WeakReferenceMessenger.Default.Send<string, string>(Current_step.Step_name.Trim(), "SaveParameter");
                });
            
            LoadStepParameterFromDBCommand = new RelayCommand(() => {
                WeakReferenceMessenger.Default.Send<string, string>("", "LoadStepParameterFromDB");
            });

            LoadPlcActProcessCommand = new RelayCommand(() => {
                WeakReferenceMessenger.Default.Send<string, string>("", "LoadPlcActProcess");
            });
            RecipePageSelectedCommand  = new RelayCommand(() => { PageContent = Recipe_Control; });
            
            StepPageSelectedCommand = new RelayCommand(() => { PageContent = Step_control; });
            DurationPageSelectedCommand = new RelayCommand(() => { PageContent = Duration_control; });
            HeatingPageSelectedCommand = new RelayCommand(() => { PageContent = Heating_control; });
            BiasPageSelectedCommand = new RelayCommand(()=> { PageContent = Bias_Control; });
            GasFlowPageSelectedCommand = new RelayCommand(() => { PageContent = Gas_flow_Control; });
            CathodePageSelectedCommand = new RelayCommand(()=> { PageContent = Cathodes_control; });
            Ubm_PlmPageSelectedCommand = new RelayCommand(()=> { PageContent = Ubm_plm_control; });
            
            PageContent = Recipe_Control;
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<string, string>(this,"StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this,"SaveEnableControl", SaveEnableControl);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);

        }

        private void SaveEnableControl(object recipient, string message)
        {
            Task.Run(() =>
            {
                if (int.Parse(message) == 0)
                {
                    Save_parameter_enable = false;
                }
                else
                {
                    Save_parameter_enable = true;
                }
            });

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
                Edit_status = "Edit enable";
            }
            else
            {
                Edit_enable = false;
                Edit_status = "Read only";
            }
        }


        private void StepTypeChanged(object recipient, string message)
        {
            
            Parameter_loaded = true;
            //WeakReferenceMessenger.Default.Register<string, string>(this, "IsParameterChanged", IsParameterChanged);
        }

        public void Receive(RecipeObject recipe)
        {
            Current_recipe = recipe;
            Parameter_loaded = false;
            Save_parameter_enable = false;
            Load_step_enable = false;
        }

        public void Receive(StepObject step)
        {
            Current_step = step;
            Save_parameter_enable = false;
            Load_step_enable = true;
        }
    }
}
