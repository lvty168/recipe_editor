using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace NewRecipeViewer.ViewModels
{
    class GasChannelsViewModel:ObservableRecipient, IRecipient<List<GasChannelParameters>>
    {
        #region gas channel 属性
        private GasChannelParameters gas_channel_1= new GasChannelParameters();

        public GasChannelParameters Gas_Channel_1
        {
            get { return gas_channel_1; }
            set
            {
                gas_channel_1 = value;

                Gas_T_Ramp_1 = gas_channel_1.t_ramp;


                OnPropertyChanged();
            }
        }
        private GasChannelParameters gas_channel_2 = new GasChannelParameters();

        public GasChannelParameters Gas_Channel_2
        {
            get { return gas_channel_2; }
            set
            {
                gas_channel_2 = value;
                Gas_T_Ramp_2 = gas_channel_2.t_ramp;
                OnPropertyChanged();
            }
        }
        private GasChannelParameters gas_channel_3 = new GasChannelParameters();

        public GasChannelParameters Gas_Channel_3
        {
            get { return gas_channel_3; }
            set
            {
                gas_channel_3 = value;
                Gas_T_Ramp_3 = gas_channel_3.t_ramp;
                OnPropertyChanged();
            }
        }
        private GasChannelParameters gas_channel_4 = new GasChannelParameters();

        public GasChannelParameters Gas_Channel_4
        {
            get { return gas_channel_4; }
            set
            {
                gas_channel_4 = value;
                Gas_T_Ramp_4 = gas_channel_4.t_ramp;
                OnPropertyChanged();
            }
        }
        private GasChannelParameters gas_channel_5 = new GasChannelParameters();

        public GasChannelParameters Gas_Channel_5
        {
            get { return gas_channel_5; }
            set
            {
                gas_channel_5 = value;
                Gas_T_Ramp_5 = gas_channel_5.t_ramp;
                OnPropertyChanged();
            }
        }
        #endregion
        #region t_ramp 操作属性

        #region Actual gas flow
        private float act_channel_1_flow;

        public float Act_channel_1_flow
        {
            get { return act_channel_1_flow; }
            set { act_channel_1_flow = value; OnPropertyChanged(); }
        }
        private float act_channel_2_flow;

        public float Act_channel_2_flow
        {
            get { return act_channel_2_flow; }
            set { act_channel_2_flow = value; OnPropertyChanged(); }
        }

        private float act_channel_3_flow;

        public float Act_channel_3_flow
        {
            get { return act_channel_3_flow; }
            set { act_channel_3_flow = value; OnPropertyChanged(); }
        }
        private float act_channel_4_flow;

        public float Act_channel_4_flow
        {
            get { return act_channel_4_flow; }
            set { act_channel_4_flow = value; OnPropertyChanged(); }
        }
        private float act_channel_5_flow;

        public float Act_channel_5_flow
        {
            get { return act_channel_5_flow; }
            set { act_channel_5_flow = value; OnPropertyChanged(); }
        }
        #endregion

        #region Actual gas delay_time
        private float act_channel_1_delay_time;

        public float Act_channel_1_delay_time
        {
            get { return act_channel_1_delay_time; }
            set { act_channel_1_delay_time = value; OnPropertyChanged(); }
        }
        private float act_channel_2_delay_time;

        public float Act_channel_2_delay_time
        {
            get { return act_channel_2_delay_time; }
            set { act_channel_2_delay_time = value; OnPropertyChanged(); }
        }

        private float act_channel_3_delay_time;

        public float Act_channel_3_delay_time
        {
            get { return act_channel_3_delay_time; }
            set { act_channel_3_delay_time = value; OnPropertyChanged(); }
        }
        private float act_channel_4_delay_time;

        public float Act_channel_4_delay_time
        {
            get { return act_channel_4_delay_time; }
            set { act_channel_4_delay_time = value; OnPropertyChanged(); }
        }
        private float act_channel_5_delay_time;

        public float Act_channel_5_delay_time
        {
            get { return act_channel_5_delay_time; }
            set { act_channel_5_delay_time = value; OnPropertyChanged(); }
        }
        #endregion

        #region Actual gas delay_time
        private float act_channel_1_ramp_time;

        public float Act_channel_1_ramp_time
        {
            get { return act_channel_1_ramp_time; }
            set { act_channel_1_ramp_time = value; OnPropertyChanged(); }
        }
        private float act_channel_2_ramp_time;

        public float Act_channel_2_ramp_time
        {
            get { return act_channel_2_ramp_time; }
            set { act_channel_2_ramp_time = value; OnPropertyChanged(); }
        }

        private float act_channel_3_ramp_time;

        public float Act_channel_3_ramp_time
        {
            get { return act_channel_3_ramp_time; }
            set { act_channel_3_ramp_time = value; OnPropertyChanged(); }
        }
        private float act_channel_4_ramp_time;

        public float Act_channel_4_ramp_time
        {
            get { return act_channel_4_ramp_time; }
            set { act_channel_4_ramp_time = value; OnPropertyChanged(); }
        }
        private float act_channel_5_ramp_time;

        public float Act_channel_5_ramp_time
        {
            get { return act_channel_5_ramp_time; }
            set { act_channel_5_ramp_time = value; OnPropertyChanged(); }
        }
        #endregion
        public float Gas_T_Ramp_1
        {
            get { return gas_channel_1.t_ramp; }
            set
            {
                gas_channel_1.t_ramp = value;
                if (value>0)
                {
                    Gas_channel_1_ramp_on = true;
                    
                }
                else
                {
                    Gas_channel_1_ramp_on = false;
                    gas_channel_1.t_delay = 0;
                    gas_channel_1.end_flow = 0;
                    //Gas_Channel_1 = gas_channel_1;
                }
                OnPropertyChanged();
            }
        }
        public float Gas_T_Ramp_2
        {
            get { return gas_channel_2.t_ramp; }
            set
            {
                gas_channel_2.t_ramp = value;
                if (value > 0)
                {
                    Gas_channel_2_ramp_on = true;
                }
                else
                {
                    Gas_channel_2_ramp_on = false;
                    gas_channel_2.t_delay = 0;
                    gas_channel_2.end_flow = 0;
                    //Gas_Channel_2 = gas_channel_2;
                }
                OnPropertyChanged();
            }
        }
        public float Gas_T_Ramp_3
        {
            get { return gas_channel_3.t_ramp; }
            set
            {
                gas_channel_3.t_ramp = value;
                if (value > 0)
                {
                    Gas_channel_3_ramp_on = true;
                }
                else
                {
                    Gas_channel_3_ramp_on = false;
                    gas_channel_3.t_delay = 0;
                    gas_channel_3.end_flow = 0;
                    //Gas_Channel_3 = gas_channel_3;
                }
                OnPropertyChanged();
            }
        }
        public float Gas_T_Ramp_4
        {
            get { return gas_channel_4.t_ramp; }
            set
            {
                gas_channel_4.t_ramp = value;
                if (value > 0)
                {
                    Gas_channel_4_ramp_on = true;
                }
                else
                {
                    Gas_channel_4_ramp_on = false;
                    gas_channel_4.t_delay = 0;
                    gas_channel_4.end_flow = 0;
                    //Gas_Channel_4 = gas_channel_4;
                }
                OnPropertyChanged();
            }
        }
        public float Gas_T_Ramp_5
        {
            get { return gas_channel_5.t_ramp; }
            set
            {
                gas_channel_5.t_ramp = value;
                if (value > 0)
                {
                    Gas_channel_5_ramp_on = true;
                }
                else
                {
                    Gas_channel_5_ramp_on = false;
                    gas_channel_5.t_delay = 0;
                    gas_channel_5.end_flow = 0;
                    //Gas_Channel_5 = gas_channel_5;
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region 可编辑控制
        private bool gas_channel_enable;

        public bool Gas_channel_enable
        {
            get { return gas_channel_enable; }
            set { gas_channel_enable = value; OnPropertyChanged(); }
        }
        private bool edit_enable;

        public bool Edit_enable
        {
            get { return edit_enable; }
            set { edit_enable = value; OnPropertyChanged(); }   
        }


        private bool gas_channel_1_ramp_on;

        public bool Gas_channel_1_ramp_on
        {
            get { return gas_channel_1_ramp_on; }
            set 
            {
                gas_channel_1_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private bool gas_channel_2_ramp_on;

        public bool Gas_channel_2_ramp_on
        {
            get { return gas_channel_2_ramp_on; }
            set
            {
                gas_channel_2_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private bool gas_channel_3_ramp_on;

        public bool Gas_channel_3_ramp_on
        {
            get { return gas_channel_3_ramp_on; }
            set
            {
                gas_channel_3_ramp_on = value;
                OnPropertyChanged();
            }
        }

        private bool gas_channel_4_ramp_on;
        public bool Gas_channel_4_ramp_on
        {
            get { return gas_channel_4_ramp_on; }
            set
            {
                gas_channel_4_ramp_on = value;
                OnPropertyChanged();
            }
        }
        private bool gas_channel_5_ramp_on;

        public bool Gas_channel_5_ramp_on
        {
            get { return gas_channel_5_ramp_on; }
            set
            {
                gas_channel_5_ramp_on = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public GasChannelsViewModel()
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            //WeakReferenceMessenger.Default.Register<GasChannelParameters[], string>(this, "GasChannelParameterChanged", ParameterChanged);
        }

        private void StepTypeChanged(object recipient, string step_type)
        {
            switch (int.Parse(step_type))
            {
                case 1:
                    Gas_channel_enable = false;
                    break;
                case 2:
                    Gas_channel_enable = true;
                    break;
                case 3:
                    Gas_channel_enable = true;
                    break;
                case 4:
                    Gas_channel_enable = true;
                    break;
                default:
                    break;
            }
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
        private void ParameterChanged(object recipient,GasChannelParameters[] parameters)
        {
            Gas_Channel_1 = parameters[0];
            Gas_Channel_2 = parameters[1];
            Gas_Channel_3 = parameters[2];
            Gas_Channel_4 = parameters[3];
            Gas_Channel_5 = parameters[4];
        }

        

        public void Receive(List<GasChannelParameters> gasChannels)
        {
            Gas_Channel_1 = gasChannels[0];
            Gas_Channel_2 = gasChannels[1];
            Gas_Channel_3 = gasChannels[2];
            Gas_Channel_4 = gasChannels[3];
            Gas_Channel_5 = gasChannels[4];
        }

        //参数发生更新时

    }
}
