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
    class HeatingViewModle:ObservableRecipient, IRecipient<HeatingParameters>
    {
        #region Select Options
        private Dictionary<string,float> pump_spd= new Dictionary<string, float>() { { "100",3}, { "66", 2 }, { "50", 1 }, { "3", 0 } };

        public Dictionary<string,float> Pump_spd
        {
            get { return pump_spd; }
            set
            { 
                pump_spd = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<string, float> water_sys_mode = new Dictionary<string, float>() { { "Cold", 0 }, { "Warm", 1 }};

        public Dictionary<string, float> WaterSysMode
        {
            get { return water_sys_mode; }
            set
            {
                water_sys_mode = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region VisibleControl
        private Visibility htr_max;

        public Visibility Htr_Max_Visibility
        {
            get { return htr_max; }
            set 
            {
                htr_max = value;
                OnPropertyChanged();
            }
        }
        private Visibility htr_min;

        public Visibility Htr_Min_Visibility
        {
            get { return htr_min; }
            set
            {
                htr_min = value;
                OnPropertyChanged();
            }
        }
        private Visibility water_system_mode;

        public Visibility Water_System_Mode_Visibility
        {
            get { return water_system_mode; }
            set
            {
                water_system_mode = value;
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
        private float act_heating_temperature;

        public float Act_heating_temperature
        {
            get { return act_heating_temperature; }
            set { act_heating_temperature = value; OnPropertyChanged(); }
        }
        private float act_pump_speed;

        public float Act_pump_speed
        {
            get { return act_pump_speed; }
            set { act_pump_speed = value; OnPropertyChanged(); }
        }
        #endregion
        private HeatingParameters heating_parameters = new HeatingParameters();

        public HeatingParameters Heating_parameters
        {
            get { return heating_parameters; }
            set 
            { 
                heating_parameters = value;
                OnPropertyChanged();
            }
        }


        public HeatingViewModle()
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            //WeakReferenceMessenger.Default.Register<HeatingParameters, string>(this, "HeatingParameterChanged", ParameterChanged);
        }

        private void ParameterChanged(object recipient, HeatingParameters parameter)
        {
            
            Heating_parameters = parameter;
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
                    Htr_Max_Visibility = Visibility.Hidden;
                    Htr_Min_Visibility = Visibility.Hidden;
                    Water_System_Mode_Visibility = Visibility.Visible;
                    
                    break;
                case 2://glow_discharge
                    Htr_Max_Visibility = Visibility.Visible;
                    Htr_Min_Visibility = Visibility.Visible;
                    Water_System_Mode_Visibility = Visibility.Hidden;
                    break;
                case 3://ion_etching
                    Htr_Max_Visibility = Visibility.Visible;
                    Htr_Min_Visibility = Visibility.Visible;
                    Water_System_Mode_Visibility = Visibility.Hidden;
                    break;
                case 4://coating
                    Htr_Max_Visibility = Visibility.Hidden;
                    Htr_Min_Visibility = Visibility.Hidden;
                    Water_System_Mode_Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }

        public void Receive(HeatingParameters parameters)
        {
            Heating_parameters = parameters;
        }
    }
}
