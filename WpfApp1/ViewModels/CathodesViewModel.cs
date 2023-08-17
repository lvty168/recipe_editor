using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
namespace NewRecipeViewer.ViewModels
{
    class CathodesViewModel : ObservableRecipient, IRecipient<Tuple<DC_Cathode_Object, DC_Cathode_Object>>,IRecipient<DC_Pls_Cathode_Object>
    {
        #region ComboBox Dict
        public Dictionary<string, float> DC_Cathode_Dict { get; } = new Dictionary<string, float>() { { "OFF", 0 }, { "CARC", 1 } };
        public Dictionary<string, float> DC_Pls_Cathode_Dict { get; } = new Dictionary<string, float>() { { "OFF",0 }, { "CARC", 1 }, { "CARC_Pls", 2 } };
        public Dictionary<string, float> Gas_Valve_Dict { get; } = new Dictionary<string, float>() { { "Closed",0 }, { "Open", 1 } };
        #endregion
        #region Cathodes
        private DC_Cathode_Object cathode_1 = new DC_Cathode_Object();

        public DC_Cathode_Object Cathode_1
        {
            get { return cathode_1; }
            set 
            { 
                cathode_1 = value;
                
                Cath1_Operation = Cathode_1.operation;
                Cath1_T_Ramp = cathode_1.t_ramp;
                OnPropertyChanged();
            }
        }
        private DC_Cathode_Object cathode_4 = new DC_Cathode_Object();

        public DC_Cathode_Object Cathode_4
        {
            get { return cathode_4; }
            set
            {
                cathode_4 = value;
                Cath4_Operation = Cathode_4.operation;
                Cath4_T_Ramp = cathode_4.t_ramp;
                OnPropertyChanged();
            }
        }
        private DC_Pls_Cathode_Object cathode_6 = new DC_Pls_Cathode_Object();

        public DC_Pls_Cathode_Object Cathode_6
        {
            get { return cathode_6; }
            set
            {
                cathode_6 = value;
                Cath6_Operation = Cathode_6.operation;
                Cath6_T_Ramp = cathode_6.t_ramp;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 开关控制
        private bool cathode_enable;

        public bool Cathode_enable
        {
            get { return cathode_enable; }
            set { cathode_enable = value; OnPropertyChanged(); }
        }


        public float Cath1_Operation
        {
            get { return cathode_1.operation; }
            set
            {
                cathode_1.operation = value;
                //Cathode_1 = cathode_1;
                if (value==0)
                {
                    Cath1_On = Visibility.Hidden;
                }
                else
                {
                    Cath1_On = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }
        public float Cath4_Operation
        {
            get { return Cathode_4.operation; }
            set
            {
                Cathode_4.operation = value;
                if (value == 0)
                {
                    Cath4_On = Visibility.Hidden;
                }
                else
                {
                    Cath4_On = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }
        public float Cath6_Operation
        {
            get { return Cathode_6.operation; }
            set
            {
                Cathode_6.operation = value;
                if (value == 0)
                {
                    Cath6_On = Visibility.Hidden;
                    Cath6_pls_mode = Visibility.Hidden;
                }
                else if(value == 1) 
                {
                    Cath6_On = Visibility.Visible;
                    Cath6_pls_mode = Visibility.Hidden;
                    if (Cathode_6.rrt < 200)
                    {
                        Cathode_6.rrt = 500;
                    }
                }
                else//value ==2
                {
                    Cath6_On = Visibility.Visible;
                    Cath6_pls_mode = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }

        public float Cath1_T_Ramp
        {
            get { return Cathode_1.t_ramp; }
            set
            {
                Cathode_1.t_ramp = value;
                if (value>0)
                {
                    Cath1_t_ramp_on = true;
                }
                else
                {
                    Cath1_t_ramp_on = false;
                    cathode_1.t_delay = 0;
                    cathode_1.arc_1_end = 0;
                    cathode_1.arc_2_end = 0;
                    cathode_1.arc_3_end = 0;
                }
                OnPropertyChanged();
            }
        }
        public float Cath4_T_Ramp
        {
            get { return Cathode_4.t_ramp; }
            set
            {
                Cathode_4.t_ramp = value;
                if (value > 0)
                {
                    Cath4_t_ramp_on = true;
                }
                else
                {
                    Cath4_t_ramp_on = false;
                    cathode_4.t_delay = 0;
                    cathode_4.arc_1_end = 0;
                    cathode_4.arc_2_end = 0;
                    cathode_4.arc_3_end = 0;
                }
                OnPropertyChanged();
            }
        }
        public float Cath6_T_Ramp
        {
            get { return Cathode_6.t_ramp; }
            set
            {
                Cathode_6.t_ramp = value;
                if (value > 0)
                {
                    Cath6_t_ramp_on = true;
                }
                else
                {
                    Cath6_t_ramp_on = false;
                    cathode_6.t_delay = 0;
                    cathode_6.arc_1_end_high = 0;
                    cathode_6.arc_1_end_low = 0;
                    cathode_6.arc_2_end_high = 0;
                    cathode_6.arc_2_end_low = 0;
                    cathode_6.arc_3_end_high = 0;
                    cathode_6.arc_3_end_low = 0;

                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region 可见控制
        private Visibility cath1_on;

        public Visibility Cath1_On
        {
            get { return cath1_on; }
            set 
            { 
                cath1_on = value;
                OnPropertyChanged();
            }
        }
        private Visibility cath4_on;

        public Visibility Cath4_On
        {
            get { return cath4_on; }
            set
            {
                cath4_on = value;
                OnPropertyChanged();
            }
        }
        private Visibility cath6_on;

        public Visibility Cath6_On
        {
            get { return cath6_on; }
            set
            {
                cath6_on = value;
                OnPropertyChanged();
            }
        }

        private bool cath1_t_ramp_on;

        public bool Cath1_t_ramp_on
        {
            get { return cath1_t_ramp_on; }
            set
            {
                cath1_t_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private bool cath4_t_ramp_on;

        public bool Cath4_t_ramp_on
        {
            get { return cath4_t_ramp_on; }
            set
            {
                cath4_t_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private bool cath6_t_ramp_on;

        public bool Cath6_t_ramp_on
        {
            get { return cath6_t_ramp_on; }
            set
            {
                cath6_t_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private Visibility cath6_pls_mode;

        public Visibility Cath6_pls_mode
        {
            get { return cath6_pls_mode; }
            set
            {
                cath6_pls_mode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }    
        }

        #region cath1
        private float act_cath_1_operation;

        public float Act_cath_1_operation
        {
            get { return act_cath_1_operation; }
            set 
            {
                act_cath_1_operation = value; 
                
                OnPropertyChanged(); 
            }  
        }

        private float act_cath_1_shutter;

        public float Act_cath_1_shutter
        {
            get { return act_cath_1_shutter; }
            set { act_cath_1_shutter = value; OnPropertyChanged(); }
        }
        private float act_cath_1_gas_valve;

        public float Act_cath_1_gas_valve
        {
            get { return act_cath_1_gas_valve; }
            set { act_cath_1_gas_valve = value; OnPropertyChanged(); }
        }

        private float act_cath_1_1_voltage;

        public float Act_cath_1_1_voltage
        {
            get { return act_cath_1_1_voltage; }
            set { act_cath_1_1_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_1_2_voltage;

        public float Act_cath_1_2_voltage
        {
            get { return act_cath_1_2_voltage; }
            set { act_cath_1_2_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_1_3_voltage;

        public float Act_cath_1_3_voltage
        {
            get { return act_cath_1_3_voltage; }
            set { act_cath_1_3_voltage = value; OnPropertyChanged(); }
        }


        private float act_cath_1_1_current;

        public float Act_cath_1_1_current
        {
            get { return act_cath_1_1_current; }
            set { act_cath_1_1_current = value; OnPropertyChanged(); }
        }
        private float act_cath_1_2_current;

        public float Act_cath_1_2_current
        {
            get { return act_cath_1_2_current; }
            set { act_cath_1_2_current = value; OnPropertyChanged(); }
        }
        private float act_cath_1_3_current;

        public float Act_cath_1_3_current
        {
            get { return act_cath_1_3_current; }
            set { act_cath_1_3_current = value; OnPropertyChanged(); }
        }

        private float act_cath_1_delay_time;

        public float Act_cath_1_delay_time
        {
            get { return act_cath_1_delay_time; }
            set { act_cath_1_delay_time = value; OnPropertyChanged(); }
        }
        private float act_cath_1_ramp_time;

        public float Act_cath_1_ramp_time
        {
            get { return act_cath_1_ramp_time; }
            set { act_cath_1_ramp_time = value; OnPropertyChanged(); }
        }
        #endregion

        #region cath4
        private float act_cath_4_operation;

        public float Act_cath_4_operation
        {
            get { return act_cath_4_operation; }
            set { act_cath_4_operation = value; OnPropertyChanged(); }
        }

        private float act_cath_4_shutter;

        public float Act_cath_4_shutter
        {
            get { return act_cath_4_shutter; }
            set { act_cath_4_shutter = value; OnPropertyChanged(); }
        }
        private float act_cath_4_gas_valve;

        public float Act_cath_4_gas_valve
        {
            get { return act_cath_4_gas_valve; }
            set { act_cath_4_gas_valve = value; OnPropertyChanged(); }
        }

        private float act_cath_4_1_voltage;

        public float Act_cath_4_1_voltage
        {
            get { return act_cath_4_1_voltage; }
            set { act_cath_4_1_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_4_2_voltage;

        public float Act_cath_4_2_voltage
        {
            get { return act_cath_4_2_voltage; }
            set { act_cath_4_2_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_4_3_voltage;

        public float Act_cath_4_3_voltage
        {
            get { return act_cath_4_3_voltage; }
            set { act_cath_4_3_voltage = value; OnPropertyChanged(); }
        }

        private float act_cath_4_1_current;

        public float Act_cath_4_1_current
        {
            get { return act_cath_4_1_current; }
            set { act_cath_4_1_current = value; OnPropertyChanged(); }
        }
        private float act_cath_4_2_current;

        public float Act_cath_4_2_current
        {
            get { return act_cath_4_2_current; }
            set { act_cath_4_2_current = value; OnPropertyChanged(); }
        }
        private float act_cath_4_3_current;

        public float Act_cath_4_3_current
        {
            get { return act_cath_4_3_current; }
            set { act_cath_4_3_current = value; OnPropertyChanged(); }
        }
        private float act_cath_4_delay_time;

        public float Act_cath_4_delay_time
        {
            get { return act_cath_4_delay_time; }
            set { act_cath_4_delay_time = value; OnPropertyChanged(); }
        }
        private float act_cath_4_ramp_time;

        public float Act_cath_4_ramp_time
        {
            get { return act_cath_4_ramp_time; }
            set { act_cath_4_ramp_time = value; OnPropertyChanged(); }
        }
        #endregion

        #region cath6
        private float act_cath_6_operation;

        public float Act_cath_6_operation
        {
            get { return act_cath_6_operation; }
            set { act_cath_6_operation = value; OnPropertyChanged(); }
        }

        private float act_cath_6_shutter;

        public float Act_cath_6_shutter
        {
            get { return act_cath_6_shutter; }
            set { act_cath_6_shutter = value; OnPropertyChanged(); }
        }
        private float act_cath_6_gas_valve;

        public float Act_cath_6_gas_valve
        {
            get { return act_cath_6_gas_valve; }
            set { act_cath_6_gas_valve = value; OnPropertyChanged(); }
        }

        private float act_cath_6_1_voltage;

        public float Act_cath_6_1_voltage
        {
            get { return act_cath_6_1_voltage; }
            set { act_cath_6_1_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_6_2_voltage;

        public float Act_cath_6_2_voltage
        {
            get { return act_cath_6_2_voltage; }
            set { act_cath_6_2_voltage = value; OnPropertyChanged(); }
        }
        private float act_cath_6_3_voltage;

        public float Act_cath_6_3_voltage
        {
            get { return act_cath_6_3_voltage; }
            set { act_cath_6_3_voltage = value; OnPropertyChanged(); }
        }

        private float act_cath_6_frequency;

        public float Act_cath_6_frequency
        {
            get { return act_cath_6_frequency; }
            set { act_cath_6_frequency = value; OnPropertyChanged(); }
        }
        private float act_cath_6_rrt;

        public float Act_cath_6_rrt
        {
            get { return act_cath_6_rrt; }
            set { act_cath_6_rrt = value; OnPropertyChanged(); }
        }

        private float act_cath_6_1_current;

        public float Act_cath_6_1_current
        {
            get { return act_cath_6_1_current; }
            set { act_cath_6_1_current = value; OnPropertyChanged(); }
        }
        private float act_cath_6_2_current;

        public float Act_cath_6_2_current
        {
            get { return act_cath_6_2_current; }
            set { act_cath_6_2_current = value; OnPropertyChanged(); }
        }
        private float act_cath_6_3_current;

        public float Act_cath_6_3_current
        {
            get { return act_cath_6_3_current; }
            set { act_cath_6_3_current = value; OnPropertyChanged(); }
        }

        private float act_cath_6_delay_time;

        public float Act_cath_6_delay_time
        {
            get { return act_cath_6_delay_time; }
            set { act_cath_6_delay_time = value; OnPropertyChanged(); }
        }
        private float act_cath_6_ramp_time;

        public float Act_cath_6_ramp_time
        {
            get { return act_cath_6_ramp_time; }
            set { act_cath_6_ramp_time = value; OnPropertyChanged(); }
        }
        #endregion

        public CathodesViewModel()
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            //WeakReferenceMessenger.Default.Register<Tuple<DC_Cathode_Object, DC_Cathode_Object, DC_Pls_Cathode_Object>, string>(this, "CathodeParameterChanged", ParameterChanged);
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
        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1:
                    Cathode_enable = false;
                    break;
                case 2:
                    Cathode_enable = false;
                    break;
                case 3:
                    Cathode_enable = true;
                    break;
                case 4:
                    Cathode_enable = true;
                    break;
                default:
                    break;
            }
        }

        //private void ParameterChanged(object recipient, Tuple<DC_Cathode_Object,DC_Cathode_Object,DC_Pls_Cathode_Object> parameters)
        //{
            
        //    Cathode_1 = parameters.Item1;
        //    Cathode_4 = parameters.Item2;
        //    Cathode_6 = parameters.Item3;
        //}
        /// <summary>
        /// 更新实时信息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(ActualModuleStatus message)
        {
            Task.Run(()=>
            {
                Act_cath_1_operation = message.act_cath1_operation;
                Act_cath_1_shutter = message.act_cath1_shutter ;
                Act_cath_1_gas_valve = message.act_cath1_gas_valve;
                Act_cath_1_delay_time = message.act_cath1_delay_time;
                Act_cath_1_ramp_time = message.act_cath1_ramp_time;
                Act_cath_1_1_voltage = message.act_cath1_1_voltage;
                Act_cath_1_2_voltage = message.act_cath1_2_voltage;
                Act_cath_1_3_voltage = message.act_cath1_3_voltage;
                Act_cath_1_1_current = message.act_cath1_1_current;
                Act_cath_1_2_current = message.act_cath1_2_current;
                Act_cath_1_3_current = message.act_cath1_3_current;

                Act_cath_4_operation = message.act_cath4_operation;
                Act_cath_4_shutter = message.act_cath4_shutter;
                Act_cath_4_gas_valve = message.act_cath4_gas_valve;
                Act_cath_4_delay_time = message.act_cath4_delay_time;
                Act_cath_4_ramp_time = message.act_cath4_ramp_time;
                Act_cath_4_1_voltage = message.act_cath4_1_voltage;
                Act_cath_4_2_voltage = message.act_cath4_2_voltage;
                Act_cath_4_3_voltage = message.act_cath4_3_voltage;
                Act_cath_4_1_current = message.act_cath4_1_current;
                Act_cath_4_2_current = message.act_cath4_2_current;
                Act_cath_4_3_current = message.act_cath4_3_current;

                Act_cath_6_operation = message.act_cath6_operation;
                Act_cath_6_shutter = message.act_cath6_shutter;
                Act_cath_6_gas_valve = message.act_cath6_gas_valve;
                Act_cath_6_delay_time = message.act_cath6_delay_time;
                Act_cath_6_ramp_time = message.act_cath6_ramp_time;
                Act_cath_6_1_voltage = message.act_cath6_1_voltage;
                Act_cath_6_2_voltage = message.act_cath6_2_voltage;
                Act_cath_6_3_voltage = message.act_cath6_3_voltage;
                Act_cath_6_1_current = message.act_cath6_1_current;
                Act_cath_6_2_current = message.act_cath6_2_current;
                Act_cath_6_3_current = message.act_cath6_3_current;
                Act_cath_6_frequency = message.act_cath6_frequency;
                Act_cath_6_rrt = message.act_cath6_rrt;
            });
        }

        public void Receive(Tuple<DC_Cathode_Object, DC_Cathode_Object> cathodes_object)
        {
            Cathode_1 = cathodes_object.Item1;
            Cathode_4 = cathodes_object.Item2;
        }

        public void Receive(DC_Pls_Cathode_Object cath6)
        {
            Cathode_6 = cath6;
        }
    }
}
