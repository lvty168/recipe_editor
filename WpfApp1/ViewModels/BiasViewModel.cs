using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace NewRecipeViewer.ViewModels
{
    class BiasViewModel: ObservableRecipient,IRecipient<BiasParameters>
    {
        #region 下拉选项字典
        public Dictionary<string, float> bias_level_glow_discharge = new Dictionary<string, float>()
        { { "DC High", 0 }, { "DC Low", 1 }, { "PLS High", 2 }, { "PLS Low", 3 }, { "PLS High Bipolar", 4 }, { "PLS Low Bipolar", 5} };
        public Dictionary<string, float> bias_level_ion_etching = new Dictionary<string, float>()
        { { "DC High", 0 }, { "PLS High", 2 }};
        public Dictionary<string, float> bias_level_coating = new Dictionary<string, float>()
        { { "DC High", 0 }, { "DC Low", 1 }, { "PLS High", 2 }, { "PLS Low", 3 } };
        public Dictionary<string, float> bias_mode_glow_discharge = new Dictionary<string, float>()
        { { "Voltage", 0 }, { "Current", 1 } };
        #endregion
        #region select
        private Dictionary<string, float> bias_level;

        public Dictionary<string, float> Bias_level_dict
        {
            get { return bias_level; }
            set
            {
                bias_level = value;
                OnPropertyChanged();
            }
        }
        private Dictionary<string, float> bias_mode= new Dictionary<string, float>()
        { { "Voltage", 0 }, { "Current", 1 } };

        public Dictionary<string, float> Bias_mode_dict
        {
            get { return bias_mode; }
            set
            {
                bias_mode = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 编辑控制
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
        private bool bias_enable;

        public bool Bias_enable
        {
            get { return bias_enable; }
            set { bias_enable = value; OnPropertyChanged(); }
        }

        private bool voltageMode;

        public bool VoltageMode
        {
            get { return voltageMode; }
            set
            {
                voltageMode = value;
                OnPropertyChanged();
            }
        }
        private bool currentMode;

        public bool CurrentMode
        {
            get { return currentMode; }
            set
            {
                currentMode = value;
                OnPropertyChanged();
            }
        }

        private bool rampOn;

        public bool RampOn
        {
            get { return rampOn; }
            set
            { 
                rampOn = value;
                OnPropertyChanged();
            }
        }
        private bool pls_mode;

        public bool PlsMode
        {
            get { return pls_mode; }
            set
            {
                pls_mode = value;
                OnPropertyChanged();
            }
        }


        #endregion
        #region Actual
        private float act_bias_voltage;

        public float Act_bias_voltage
        {
            get { return act_bias_voltage; }
            set { act_bias_voltage = value; OnPropertyChanged(); }
        }

        private float act_bias_current;

        public float Act_bias_current
        {
            get { return act_bias_current; }
            set { act_bias_current = value; OnPropertyChanged(); }
        }

        private float act_bias_delay_time;

        public float Act_bias_delay_time
        {
            get { return act_bias_delay_time; }
            set { act_bias_delay_time = value; OnPropertyChanged(); }
        }
        private float act_bias_ramp_time;

        public float Act_bias_ramp_time
        {
            get { return act_bias_ramp_time; }
            set { act_bias_ramp_time = value; OnPropertyChanged(); }
        }

        private float act_rotation_speed;

        public float Act_rotation_speed
        {
            get { return act_rotation_speed; }
            set { act_rotation_speed = value; OnPropertyChanged(); }
        }
        #endregion
        private BiasParameters bias_parameter = new BiasParameters();

        public BiasParameters BiasParameter
        {
            get { return bias_parameter; }
            set
            { 
                bias_parameter = value;
                BiasLevel = BiasParameter.select_voltage_mode;
                BiasMode = BiasParameter.bias_control_mode;
                BiasRamp = BiasParameter.bias_ramp_time;
                OnPropertyChanged();
            }
        }

        public float BiasLevel
        {
            get { return BiasParameter.select_voltage_mode; }
            set
            {
                BiasParameter.select_voltage_mode = value;
                BiasLevelChanged();
                OnPropertyChanged();
            }
        }

        public float BiasMode
        {
            get { return BiasParameter.bias_control_mode; }
            set
            {
                BiasParameter.bias_control_mode = value;
                BiasModeChanged();
                OnPropertyChanged();
            }
        }
        public float BiasRamp
        {
            get { return BiasParameter.bias_ramp_time; }
            set
            {
                BiasParameter.bias_ramp_time = value;
                RampChanged();
                OnPropertyChanged();
            }
        }
        private string high_low_mode;

        public string HighLowMode
        {
            get { return high_low_mode; }
            set 
            { 
                high_low_mode = value;
                OnPropertyChanged();
            }
        }
        private bool glow_dischargeMode;

        public bool GlowDischargeMode
        {
            get { return glow_dischargeMode; }
            set
            {
                glow_dischargeMode = value;
                OnPropertyChanged();
            }
        }
        #region 命令请求
        public IRelayCommand BiasLevelChangedCommand { get; }
        public IRelayCommand BiasModeChangedCommand { get; }
        public IRelayCommand RampChangedCommand { get; }
        #endregion

        public BiasViewModel()
        {
            this.IsActive = true;
            Bias_level_dict = bias_level_glow_discharge;
            WeakReferenceMessenger.Default.Register<string, string>(this, "StepTypeChanged", StepTypeChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", IDreleaseChanged);
            BiasLevelChangedCommand = new RelayCommand(BiasLevelChanged);
            BiasModeChangedCommand = new RelayCommand(BiasModeChanged);
            RampChangedCommand = new RelayCommand(RampChanged);
            //WeakReferenceMessenger.Default.Register<BiasParameters, string>(this, "BiasParameterChanged", ParameterChanged);
            //WeakReferenceMessenger.Default.Register<string, string>(this, "BiasLevelChanged", BiasLevelChanged);
            //WeakReferenceMessenger.Default.Register<string, string>(this, "IDreleaseChanged", BiasModeChanged);
        }

        private void ParameterChanged(object recipient, BiasParameters parameters)
        {
            
            BiasParameter = parameters;
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
                    Bias_level_dict = new Dictionary<string, float>();
                    Bias_mode_dict = new Dictionary<string, float>();
                    GlowDischargeMode = false;
                    Bias_enable = false;
                    break;
                case 2://glow_discharge
                    Bias_level_dict = bias_level_glow_discharge;
                    Bias_mode_dict = bias_mode_glow_discharge;
                    GlowDischargeMode = true;
                    Bias_enable = true;
                    break;
                case 3://ion_etching
                    Bias_level_dict = bias_level_ion_etching;
                    Bias_mode_dict = new Dictionary<string, float>();
                    GlowDischargeMode = false;
                    Bias_enable = true;
                    break;
                case 4://coating
                    Bias_level_dict = bias_level_coating;
                    Bias_mode_dict = new Dictionary<string, float>();
                    GlowDischargeMode = false;
                    Bias_enable = true;
                    break;
                default:
                    break;
            }
        }

        private void BiasLevelChanged()
        {
            
            switch (BiasParameter.select_voltage_mode)
            {
                case 0://DC High
                    HighLowMode = "(high)";
                    PlsMode = false;
                    BiasParameter.pulse_frequency = 0;
                    BiasParameter.pulse_rrt = 0;
                    break;
                case 1://DC Low
                    HighLowMode = "(low)";
                    PlsMode = false;
                    BiasParameter.pulse_frequency = 0;
                    BiasParameter.pulse_rrt = 0;
                    break;
                case 2://PLS High
                    HighLowMode = "(high)";
                    
                    PlsMode = true;
                    break;
                case 3://PLS Low
                    HighLowMode = "(low)";
                    PlsMode = true;
                    break;
                case 4://PLS High Bipolar 双极脉冲模式
                    HighLowMode = "(high)";
                    PlsMode = true;
                    break;
                case 5://PLS Low Bipolar 双极脉冲模式
                    HighLowMode = "(low)";
                    PlsMode = true;
                    break;
                default:
                    break;
            }
        }
        private void BiasModeChanged()
        {
            if (BiasParameter.bias_control_mode ==0)
            {
                if (BiasParameter.bias_ramp_time > 0)
                {
                    VoltageMode = true;
                    CurrentMode = false;
                }
                else
                {
                    VoltageMode = false;
                    CurrentMode = false;
                }
                
            }
            else if (BiasParameter.bias_control_mode == 1)
            {
                if (BiasParameter.bias_ramp_time > 0)
                {
                    VoltageMode = false;
                    CurrentMode = true;
                }
                else
                {
                    VoltageMode = false;
                    CurrentMode = false;
                }
            }
        }

        private void RampChanged()
        {
            
            if (BiasParameter.bias_ramp_time > 0)
            {
                RampOn = true;
                if (BiasParameter.bias_control_mode ==0)
                {
                    VoltageMode = true;
                    CurrentMode = false;
                }
                else if (BiasParameter.bias_control_mode == 1)
                {
                    VoltageMode = false;
                    CurrentMode = true;
                }
            }
            else
            {
                RampOn = false;
                VoltageMode = false;
                CurrentMode = false;
                BiasParameter.bias_delay_time = 0;
                BiasParameter.end_current = 0;
                BiasParameter.end_voltage = 0;
            }
        }

        public void Receive(BiasParameters parameters)
        {
            BiasParameter = parameters;
        }
    }
}
