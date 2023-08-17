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
    class PressureControlViewModel:ObservableRecipient,IRecipient<GasPressureControlParameters>
    {
        #region 公共属性
        private Dictionary<string, float> gasChannel = new Dictionary<string, float>() { { "N2", 1 }, { "Ar", 2 }, { "H2", 3 }, { "Ar-plm", 4 }, { "Kr-H2", 5 } };
        #endregion
        #region Select Options
        private Dictionary<string, float> press = new Dictionary<string, float>() { { "No", 0 }, { "Yes", 1 } };

        public Dictionary<string, float> Press
        {
            get { return press; }
            set
            {
                press = value;
                OnPropertyChanged();
            }
        }
        
        private Dictionary<string, float> press_control_gas;

        public Dictionary<string, float> Press_Control_Gas
        {
            get { return press_control_gas; }
            set
            {
                press_control_gas = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region visible
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region EditEnable
        private bool edit_enable;

        public bool EditEnable
        {
            get { return edit_enable; }
            set
            {
                edit_enable = value;
                OnPropertyChanged();
            }
        }
        private bool Id_edit_enable;

        public bool ID_Edit_Enable
        {
            get { return Id_edit_enable; }
            set
            {
                Id_edit_enable = value;
                OnPropertyChanged();
            }
        }
        #endregion
        private GasPressureControlParameters pressure_control_parameter = new GasPressureControlParameters();

        public GasPressureControlParameters Pressure_control_parameter
        {
            get { return pressure_control_parameter; }
            set 
            { 
                pressure_control_parameter = value;
                
                Pressure_control_operation = pressure_control_parameter.pressure_control;
                Pressure_control_gas = pressure_control_parameter.pressure_control_gas;
                
                
                OnPropertyChanged();
            }
        }
        public float Pressure_control_operation
        {
            get
            {
                return pressure_control_parameter.pressure_control;
            }
            set
            {
                if (value == 0) 
                {
                    pressure_control_parameter = new GasPressureControlParameters();
                    Visible = false;
                    Pressure_control_gas = -1;
                }
                else
                {
                    pressure_control_parameter.pressure_control = value;
                    Visible = true;
                }
                OnPropertyChanged();
            }

        }

        public float Pressure_control_gas
        {
            get
            {
                return pressure_control_parameter.pressure_control_gas;
            }
            set
            {
                pressure_control_parameter.pressure_control_gas = value;
                OnPropertyChanged();
            }

        }

        public float PressureAttribute
        {
            get { return Pressure_control_parameter.pressure_control; }
            set
            {
                Pressure_control_parameter.pressure_control = value;
                if (value == 0)
                {
                    Visible = false;
                }
                else
                {
                    Visible = true;
                }
                OnPropertyChanged();
            }
        }
        public PressureControlViewModel()
        {
            this.IsActive = true;
            Press_Control_Gas = gasChannel;
            PressureAttribute = 0;

            //WeakReferenceMessenger.Default.Register<string, string>(this, "PressSelectChanged", PressSelectChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            //WeakReferenceMessenger.Default.Register<GasPressureControlParameters, string>(this, "GasPressureControlParameterChanged", ParameterChanged);
        }

        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1:
                    EditEnable = false;
                    break;
                case 2:
                    EditEnable = true;
                    break;
                case 3:
                    EditEnable = true;
                    break;
                case 4:
                    EditEnable = true;
                    break;
                default:
                    break;
            }
        }

        private void ParameterChanged(object recipient, GasPressureControlParameters parameters)
        {
            
            Pressure_control_parameter = parameters;
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
                ID_Edit_Enable = true;
            }
            else
            {
                ID_Edit_Enable = false;
            }
        }

        public void Receive(GasPressureControlParameters parameters)
        {
            Pressure_control_parameter = parameters;
        }
    }
}
