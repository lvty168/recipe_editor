using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
namespace NewRecipeViewer.ViewModels
{
    class DurationViewModel:ObservableRecipient,IRecipient<DurationParameters>
    {
        #region select

        #endregion

        #region visible parameter
        private bool total_Ah_visible;

        public bool Total_Ah_Visible_Control
        {
            get { return total_Ah_visible; }
            set 
            { 
                total_Ah_visible = value;
                OnPropertyChanged();
            }
        }

        private bool cath_on_time_visible;

        public bool Cath_On_Time_Visible_Control
        {
            get { return cath_on_time_visible; }
            set 
            {
                cath_on_time_visible = value;
                OnPropertyChanged();
            }
        }
        private bool cath_off_time_visible;

        public bool Cath_Off_Time_Visible_Control
        {
            get { return cath_off_time_visible; }
            set
            {
                cath_off_time_visible = value;
                OnPropertyChanged();
            }
        }
        private bool base_pressure_visible;
        public bool Base_Pressure_Visible_Control
        {
            get { return base_pressure_visible; }
            set
            {
                base_pressure_visible = value;
                OnPropertyChanged();
            }
        }
        private bool base_temperature_visible;
        public bool Base_Temperature_Visible_Control
        {
            get { return base_temperature_visible; }
            set
            {
                base_temperature_visible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region EditEnable
        private bool edit_enable;

        public bool Edit_Enable
        {
            get { return edit_enable; }
            set 
            { 
                edit_enable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Actual
        private float act_total_step_time;

        public float Act_total_step_time
        {
            get { return act_total_step_time; }
            set { act_total_step_time = value; OnPropertyChanged(); }   
        }
        private float act_total_ah;

        public float Act_total_ah
        {
            get { return act_total_ah; }
            set { act_total_ah = value; OnPropertyChanged(); }
        }
        private float act_cath_on_time;
        public float Act_cath_on_time
        {
            get { return act_cath_on_time; }
            set { act_cath_on_time = value; OnPropertyChanged(); }
        }
        private float act_cath_off_time;
        public float Act_cath_off_time
        {
            get { return act_cath_off_time; }
            set { act_cath_off_time = value; OnPropertyChanged(); }
        }
        private float act_penning;
        public float Act_penning
        {
            get { return act_penning; }
            set { act_penning = value; OnPropertyChanged(); }
        }
        private float act_base_temperature;
        public float Act_base_temperature
        {
            get { return act_base_temperature; }
            set { act_base_temperature = value; OnPropertyChanged(); }
        }
        #endregion
        private DurationParameters duration_parameters = new DurationParameters();

        public DurationParameters Duration_parameters
        {
            get { return duration_parameters; }
            set 
            {
                duration_parameters = value;
                OnPropertyChanged();
            }
        }

        public DurationViewModel()
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<string, string>(this,"StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this,"IDreleaseChanged", IDreleaseChanged);
            

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
                Edit_Enable = true;
            }
            else
            {
                Edit_Enable = false;
            }
        }

        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1://加热
                    Total_Ah_Visible_Control = false;
                    Cath_On_Time_Visible_Control = false;
                    Cath_Off_Time_Visible_Control = false;
                    Base_Pressure_Visible_Control = true;
                    Base_Temperature_Visible_Control = true;
                    break;
                case 2://glow_discharge
                    Total_Ah_Visible_Control = false;
                    Cath_On_Time_Visible_Control = false;
                    Cath_Off_Time_Visible_Control = false;
                    Base_Pressure_Visible_Control = false;
                    Base_Temperature_Visible_Control = false;
                    break;
                case 3://ion_etching
                    Total_Ah_Visible_Control = true;
                    Cath_On_Time_Visible_Control = true;
                    Cath_Off_Time_Visible_Control = true;
                    Base_Pressure_Visible_Control = false;
                    Base_Temperature_Visible_Control = true;
                    break;
                case 4://coating
                    Total_Ah_Visible_Control = true;
                    Cath_On_Time_Visible_Control = false;
                    Cath_Off_Time_Visible_Control = false;
                    Base_Pressure_Visible_Control = false;
                    Base_Temperature_Visible_Control = false;
                    break;
                default:
                    break;
                    
            }
        }

        public void Receive(DurationParameters parameters)
        {
            Duration_parameters = parameters;
        }
    }
}
