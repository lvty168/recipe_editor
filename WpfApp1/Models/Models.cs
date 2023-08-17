using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace NewRecipeViewer
{
    public class ParameterValue
    {
        public float value { get; set; }
        public bool status { get; set; }

    }
    
    public class RecipeObject
    {
        /*
         * 配方对象
         */
        private int recipe_id;
        public int Recipe_ID 
        {
            get {return recipe_id; } 
            set { recipe_id = value; 
                //WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                //Tuple.Create<string, string>("Recipe_ID", recipe_id.ToString()), "RecipeHandelChanged");
            } 
        }
        [Description("Rev")]
        private int recipe_reversion;
        public int Rcp_rev
        {
            get { return recipe_reversion; }
            set { recipe_reversion = value;
                //WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                //Tuple.Create<string, string>("Rcp_rev", recipe_reversion.ToString()), "RecipeHandelChanged");
            }
        }
        [Description("Name")]
        private string recipe_name;
        public String Rev_name
        {
            get { return recipe_name; }
            set { recipe_name = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Rev_name", recipe_name.Trim()), "RecipeHandelChanged");
            }
        }
        [Description("Date")]
        private string recipe_datetime;
        public String Rev_date
        {
            get { return recipe_datetime; }
            set { recipe_datetime = value; }
        }
        private int id_release;
        public int IDrelease
        {
            get { return id_release; }
            set { id_release = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("IDrelease", id_release.ToString()), "RecipeHandelChanged");
            }
        }
        //public String IDrelease { get; set; }
        private int emergency_recipe;
        public int EmRecipe
        {
            get { return emergency_recipe; }
            set { emergency_recipe = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("EmRecipe", emergency_recipe.ToString()), "RecipeHandelChanged");
            }
        }
        [Description("Use")]
        private int use_emergency_recipe;
        public int UseEmRcp
        {
            get { return use_emergency_recipe; }
            set { use_emergency_recipe = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("UseEmRcp", use_emergency_recipe.ToString()), "RecipeHandelChanged");
            }
        }
        [Description("Note")]
        private string reversion_note;
        public String Rev_note
        {
            get { return reversion_note; }
            set { reversion_note = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Rev_note", reversion_note.Trim()), "RecipeHandelChanged");
            }
        }
        private int cathode_uniform;
        public int Cath_unif
        {
            get { return cathode_uniform; }
            set { cathode_uniform = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Cath_unif", cathode_uniform.ToString()), "RecipeHandelChanged");
            }
        }
        private int venting;
        public int Venting
        {
            get { return venting; }
            set { venting = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Venting", venting.ToString()), "RecipeHandelChanged");
            }
        }
        private int cold_trap;
        public int Coldtrap
        {
            get { return cold_trap; }
            set { cold_trap = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Coldtrap", cold_trap.ToString()), "RecipeHandelChanged");
            }
        }
        private float venting_temperature;
        public float VentingTemp
        {
            get { return venting_temperature; }
            set { venting_temperature = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("VentingTemp", venting_temperature.ToString()), "RecipeHandelChanged");
            }
        }
        private float leak_test;
        public float Leaktest
        {
            get { return leak_test; }
            set { leak_test = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("Leaktest", leak_test.ToString()), "RecipeHandelChanged");
            }
        }
        private float forced_cooling;
        public float ForcedCooling
        {
            get { return forced_cooling; }
            set { forced_cooling = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string>, string>(
                Tuple.Create<string, string>("ForcedCooling", forced_cooling.ToString()), "RecipeHandelChanged");
            }
        }

    }
    public class RepetitionObject
    {
        /*
         循环块对象
         */
        public int start { get; set; }
        public int end { get; set; }
        public int repetition { get; set; }

    }

    public class StepObject
    {
        /*
         * 工艺步对象
         */
        public int Recipe_ID
        {
            get;
            set;
        }
        public int Rcp_rev { get; set; }
        public int Step_ID { get; set; }
        private string step_name;
        public string Step_name
        {
            get { return step_name; }
            set
            {
                step_name = value;
            }
        }
        public int Step_type { get; set; }
        public string Step_date { get; set; }
        public int UseEmRcp { get; set; }
        public string Rev_name { get; set; }
    }
    public class Process_SequenceObject
    {
        public int Recipe_ID { get; set; }
        public int Rcp_rev { get; set; }
        public int Step_ID { get; set; }
        public int Seq_no { get; set; }
        public int Rep_blok { get; set; }
        public int Repetition { get; set; }
        public String Step_name { get; set; }
        public String Rev_name { get; set; }
        public int Step_type { get; set; }

    }

    public class Process_ParameterObject
    {
        public int Recipe_ID { get; set; }
        public int Rcp_rev { get; set; }
        public int Step_ID { get; set; }
        public int PS_id { get; set; }
        public Double Param_value { get; set; }
    }
    public class ParameterBase
    {
        public String name { get; set; }
        public Double value { get; set; }
        public bool beused { get; set; }
    }
    public class DurationParameters
    {
        private float _total_step_time;
        private float _total_ah;//9803
        private float _cathode_on_time;//9804
        private float _cathode_off_time;//9805
        private float _base_pressure;//9806
        private float _base_temperature;//9808
        private float _minimum_ampMinutes_CARC;//9809
        public float total_step_time
        {
            get { return _total_step_time; }
            set { _total_step_time = value; 
                WeakReferenceMessenger.Default.Send<Tuple<string,float>, string>(
                    Tuple.Create<string, float>("total_step_time", _total_step_time), "DurationParameterChanged"); }
        }//9800
         //public Double total_step_time_minute { get; set; }//9801
         //public Double total_step_time_second { get; set; }//9802

        public float total_ah
        {
            get { return _total_ah; }
            set { _total_ah = value; 
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("total_ah", _total_ah), "DurationParameterChanged");
            }
        }//9803

        public float cathode_on_time
        {
            get { return _cathode_on_time; }
            set { _cathode_on_time = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("cathode_on_time", _cathode_on_time), "DurationParameterChanged"); }
        }//9804
        public float cathode_off_time
        {
            get { return _cathode_off_time; }
            set { _cathode_off_time = value; WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("cathode_off_time", _cathode_off_time), "DurationParameterChanged");
            }
        }//9805
        public float base_pressure
        {
            get { return _base_pressure; }
            set { _base_pressure = value; WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("base_pressure", _base_pressure), "DurationParameterChanged");
            }
        }//9806
        public float base_temperature
        {
            get { return _base_temperature; }
            set { _base_temperature = value; WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("base_temperature", _base_temperature), "DurationParameterChanged");
            }
        }//9808
        public float minimum_ampMinutes_CARC
        {
            get { return _minimum_ampMinutes_CARC; }
            set { _minimum_ampMinutes_CARC = value; WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("minimum_ampMinutes_CARC", _minimum_ampMinutes_CARC), "DurationParameterChanged");
            }
        }//9809


    }
    public class ParameterObject : ObservableObject
    {
        public int PS_id { get; set; }
        public int PS_type { get; set; }
        public int PS_cat { get; set; }
        public int PS_sort { get; set; }
        public float Param_default { get; set; }
        public string PL_caption { get; set; }
        public string PL_desc { get; set; }
        public float MINRANGE { get; set; }
        public float MAXRANGE { get; set; }
        public float SMALLSTEP { get; set; }
        public float BIGSTEP { get; set; }
        public string UNIT { get; set; }
        public float Param_value { get; set; }
    }
    public class ParameterForDB
    {
        public string key { get; set; }
        public float value { get; set; }
        
    }

    public class ParameterAlias
    {
        public string Db_key { get; set; }
        public string PS_name { get; set; }
    }

    public class HeatingParameters
    {
        private float _heating_power;//9810
        private float _heating_temperature;//9811
        private float _max_temperature;//9812
        private float _min_temperature;//9813
        private float _pump_speed;//9814
        private float _water_system_mode;//9815

        public float heating_power
        {
            get { return _heating_power; }
            set { _heating_power = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                        Tuple.Create<string, float>("heating_power", _heating_power), "HeatingParameterChanged");
            }
        }
        public float heating_temperature
        {
            get { return _heating_temperature; }
            set { _heating_temperature = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("heating_temperature", _heating_temperature), "HeatingParameterChanged");
            }
        }//9811
        public float max_temperature
        {
            get { return _max_temperature; }
            set { _max_temperature = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("max_temperature", _max_temperature), "HeatingParameterChanged");
            }
        }//9812
        public float min_temperature
        {
            get { return _min_temperature; }
            set { _min_temperature = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("min_temperature", _min_temperature), "HeatingParameterChanged");
            }
        }//9813
        public float pump_speed
        {
            get { return _pump_speed; }
            set { _pump_speed = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("pump_speed", _pump_speed), "HeatingParameterChanged");
            }
        }//9814
        public float water_system_mode
        {
            get { return _water_system_mode; }
            set { _water_system_mode = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("water_system_mode", _water_system_mode), "HeatingParameterChanged");
            }
        }//9815
    }
    public class BiasParameters
    {
        private float _select_voltage_mode { get; set; }//9820
        private float _Start_voltage { get; set; }//9821
        private float _Start_current { get; set; }//9822
        private float _bias_delay_time { get; set; }//9823-9825

        private float _bias_ramp_time { get; set; }//9826

        private float _arc_detection_I { get; set; }//9829
        private float _plasma_detection_I { get; set; }//9830
        private float _plasma_detection_U { get; set; }//9831
        private float _pulse_frequency { get; set; }//9832
        private float _pulse_rrt { get; set; }//9833
        private float _rotation_speed { get; set; }//9834
        private float _bias_control_mode { get; set; }//9835
        private float _end_voltage { get; set; }//9836
        private float _end_current { get; set; }//9837
        private float _U_arc_detection { get; set; }//9838
        private float _I_arc_detection { get; set; }//9839
        private float _arc_frequency_limit { get; set; }//9840

        public float select_voltage_mode
        {
            get { return _select_voltage_mode; }
            set { _select_voltage_mode = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("select_voltage_mode", _select_voltage_mode), "BiasParameterChanged");
            }
        }//9820
        public float Start_voltage
        {
            get { return _Start_voltage; }
            set { _Start_voltage = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                Tuple.Create<string, float>("Start_voltage", _Start_voltage), "BiasParameterChanged");
            }
        }//9821
        public float Start_current
        {
            get { return _Start_current; }
            set { _Start_current = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("Start_current", _Start_current), "BiasParameterChanged");
            }
        }//9822
        public float bias_delay_time
        {
            get { return _bias_delay_time; }
            set { _bias_delay_time = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("bias_delay_time", _bias_delay_time), "BiasParameterChanged");
            }
        }//9823-9825
         //public float bias_delay_time_minute { get; set; }//9824
         //public float bias_delay_time_second { get; set; }//9825
        public float bias_ramp_time
        {
            get { return _bias_ramp_time; }
            set { _bias_ramp_time = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("bias_ramp_time", _bias_ramp_time), "BiasParameterChanged");
            }
        }//9826
         //public float bias_ramp_time_minute { get; set; }//9827
         //public float bias_ramp_time_second { get; set; }//9828
        public float arc_detection_I
        {
            get { return _arc_detection_I; }
            set { _arc_detection_I = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("arc_detection_I", _arc_detection_I), "BiasParameterChanged");
            }
        }//9829
        public float plasma_detection_I
        {
            get { return _plasma_detection_I; }
            set { _plasma_detection_I = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("plasma_detection_I", _plasma_detection_I), "BiasParameterChanged");
            }
        }//9830
        public float plasma_detection_U
        {
            get { return _plasma_detection_U; }
            set { _plasma_detection_U = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("plasma_detection_U", _plasma_detection_U), "BiasParameterChanged");
            }
        }//9831
        public float pulse_frequency
        {
            get { return _pulse_frequency; }
            set { _pulse_frequency = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("pulse_frequency", _pulse_frequency), "BiasParameterChanged");
            }
        }//9832
        public float pulse_rrt
        {
            get { return _pulse_rrt; }
            set { _pulse_rrt = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("pulse_rrt", _pulse_rrt), "BiasParameterChanged");
            }
        }//9833
        public float rotation_speed
        {
            get { return _rotation_speed; }
            set { _rotation_speed = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("rotation_speed", _rotation_speed), "BiasParameterChanged");
            }
        }//9834
        public float bias_control_mode
        {
            get { return _bias_control_mode; }
            set { _bias_control_mode = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("bias_control_mode", _bias_control_mode), "BiasParameterChanged");
            }
        }//9835
        public float end_voltage
        {
            get { return _end_voltage; }
            set { _end_voltage = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("end_voltage", _end_voltage), "BiasParameterChanged");
            }
        }//9836
        public float end_current
        {
            get { return _end_current; }
            set { _end_current = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("end_current", _end_current), "BiasParameterChanged");
            }
        }//9837
        public float U_arc_detection
        {
            get { return _U_arc_detection; }
            set { _U_arc_detection = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("U_arc_detection", _U_arc_detection), "BiasParameterChanged");
            }
        }//9838
        public float I_arc_detection
        {
            get { return _I_arc_detection; }
            set { _I_arc_detection = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("I_arc_detection", _I_arc_detection), "BiasParameterChanged");
            }
        }//9839
        public float arc_frequency_limit
        {
            get { return _arc_frequency_limit; }
            set { _arc_frequency_limit = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("arc_frequency_limit", _arc_frequency_limit), "BiasParameterChanged");
            }
        }//9840
    }
    public class GasBlendControlParameters
    {
        private float _gas_blend_control=0;//9850
        private float _slaver_gas=-1;//9851
        private float _master_gas = -1;//9852
        private float _ratio_slaver = 0;//9853
        private float _ratio_master=0;//9854

        public float gas_blend_control
        {
            get { return _gas_blend_control; }
            set { _gas_blend_control = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                Tuple.Create<string, float>("gas_blend_control", _gas_blend_control), "BlendControlParameterChanged");
            }
        }//9850
        public float slaver_gas
        {
            get { return _slaver_gas; }
            set { _slaver_gas = value; 
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("slaver_gas", _slaver_gas), "BlendControlParameterChanged");
            }
        }//9851
        public float master_gas
        {
            get { return _master_gas; }
            set { _master_gas = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                Tuple.Create<string, float>("master_gas", _master_gas), "BlendControlParameterChanged");
            }
        }//9852
        public float ratio_slaver
        {
            get { return _ratio_slaver; }
            set { _ratio_slaver = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                Tuple.Create<string, float>("ratio_slaver", _ratio_slaver), "BlendControlParameterChanged");
            }
        }//9853
        public float ratio_master
        {
            get { return _ratio_master; }
            set { _ratio_master = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                Tuple.Create<string, float>("ratio_master", _ratio_master), "BlendControlParameterChanged");
            }
        }//9854
    }

    public class GasPressureControlParameters
    {
        private float _pressure_control=0;//9855
        private float _pressure_control_gas=-1;//9856
        private float _max_deviation_pressure=0;//9857
        private float _process_pressure=0;//9859
        private float _corrrction_flow=0;//9861

        public float pressure_control
        {
            get { return _pressure_control; }
            set { _pressure_control = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                                    Tuple.Create<string, float>("pressure_control", _pressure_control), "PressureControlParameterChanged");
            }
        }//9855
        public float pressure_control_gas
        {
            get { return _pressure_control_gas; }
            set { _pressure_control_gas = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("pressure_control_gas", _pressure_control_gas), "PressureControlParameterChanged");
            }
        }//9856
        public float max_deviation_pressure
        {
            get { return _max_deviation_pressure; }
            set { _max_deviation_pressure = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("max_deviation_pressure", _max_deviation_pressure), "PressureControlParameterChanged");
            }
        }//9857
        public float process_pressure
        {
            get { return _process_pressure; }
            set { _process_pressure = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("process_pressure", _process_pressure), "PressureControlParameterChanged");
            }
        }//9859
        public float corrrction_flow
        {
            get { return _corrrction_flow; }
            set { _corrrction_flow = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                    Tuple.Create<string, float>("corrrction_flow", _corrrction_flow), "PressureControlParameterChanged");
            }
        }//9861
    }
    public class GasChannelParameters
    {
        //模块名称
        public string module_name { get; set; }


        private float _start_flow;//9862
        private float _t_delay;//9863

        private float _end_flow;//9866
        private float _t_ramp;//9867


        public float start_flow
        {
            get { return _start_flow; }
            set
            {
                _start_flow = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                    Tuple.Create<string, string, float>(module_name, "start_flow", start_flow), "GasChannelParameterChanged");
            }
        }//9862
        public float t_delay
        {
            get { return _t_delay; }
            set { _t_delay = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                    Tuple.Create<string, string, float>(module_name, "t_delay", t_delay), "GasChannelParameterChanged");
            }
        }//9863
         //public Double t_delay_minute { get; set; }//9864
         //public Double t_delay_second { get; set; }//9865
        public float end_flow
        {
            get { return _end_flow; }
            set { _end_flow = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                    Tuple.Create<string, string, float>(module_name, "end_flow", end_flow), "GasChannelParameterChanged");
            }
        }//9866
        public float t_ramp
        {
            get { return _t_ramp; }
            set { _t_ramp = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                    Tuple.Create<string, string, float>(module_name, "t_ramp", t_ramp), "GasChannelParameterChanged");
            }
        }//9867
         //public Double t_ramp_minute { get; set; }//9868
         //public Double t_ramp_second { get; set; }//9869
    }

    public class Parameter_T2
    {

        public float total_step_time_hour { get; set; }//9800
        public float total_step_time_minute { get; set; }//9801
        public float total_step_time_second { get; set; }//9802
        public float total_ah { get; set; }//9803
        public float cathode_on_time { get; set; }//9804
        public float cathode_off_time { get; set; }//9805
        public float base_pressure { get; set; }//9806
        public float base_temperature { get; set; }//9808
        public float minimum_ampMinutes_CARC { get; set; }//9809
        public float heating_power { get; set; }//9810
        public float heating_temperature { get; set; }//9811
        public float max_temperature { get; set; }//9812
        public float min_temperature { get; set; }//9813
        public float pump_speed { get; set; }//9814
        public float water_system_mode { get; set; }//9815
        public float select_voltage_mode { get; set; }//9820
        public float Start_voltage { get; set; }//9821
        public float Start_current { get; set; }//9822
        public float bias_delay_time_hour { get; set; }//9823
        public float bias_delay_time_minute { get; set; }//9824
        public float bias_delay_time_second { get; set; }//9825
        public float bias_ramp_time_hour { get; set; }//9826
        public float bias_ramp_time_minute { get; set; }//9827
        public float bias_ramp_time_second { get; set; }//9828
        public float arc_detection_I { get; set; }//9829
        public float plasma_detection_I { get; set; }//9830
        public float plasma_detection_U { get; set; }//9831
        public float pulse_frequency { get; set; }//9832
        public float pulse_rrt { get; set; }//9833
        public float rotation_speed { get; set; } = 3;//9834
        public float bias_control_mode { get; set; }//9835
        public float end_voltage { get; set; }//9836
        public float end_current { get; set; }//9837
        public float U_arc_detection { get; set; }//9838
        public float I_arc_detection { get; set; }//9839
        public float arc_frequency_limit { get; set; }//9840
        public float gas_blend_control { get; set; }//9850
        public float slaver_gas { get; set; }//9851
        public float master_gas { get; set; }//9852
        public float ratio_slaver { get; set; }//9853
        public float ratio_master { get; set; }//9854
        public float pressure_control { get; set; }//9855
        public float pressure_control_gas { get; set; }//9856
        public float max_deviation_pressure { get; set; }//9857
        public float process_pressure { get; set; }//9859
        public float corrrction_flow { get; set; }//9861
        public float gas_channel_1_start_flow { get; set; }//9862
        public float gas_channel_1_delay_hour { get; set; }//9863
        public float gas_channel_1_delay_minute { get; set; }//9864
        public float gas_channel_1_delay_second { get; set; }//9865
        public float gas_channel_1_end_flow { get; set; }//9866
        public float gas_channel_1_ramp_hour { get; set; }//9867
        public float gas_channel_1_ramp_minute { get; set; }//9868
        public float gas_channel_1_ramp_second { get; set; }//9869
        public float gas_channel_2_start_flow { get; set; }//9872
        public float gas_channel_2_delay_hour { get; set; }//9873
        public float gas_channel_2_delay_minute { get; set; }//9874
        public float gas_channel_2_delay_second { get; set; }//9875
        public float gas_channel_2_end_flow { get; set; }//9876
        public float gas_channel_2_ramp_hour { get; set; }//9877
        public float gas_channel_2_ramp_minute { get; set; }//9878
        public float gas_channel_2_ramp_second { get; set; }//9879
        public float gas_channel_3_start_flow { get; set; }//9882
        public float gas_channel_3_delay_hour { get; set; }//9883
        public float gas_channel_3_delay_minute { get; set; }//9884
        public float gas_channel_3_delay_second { get; set; }//9885
        public float gas_channel_3_end_flow { get; set; }//9886
        public float gas_channel_3_ramp_hour { get; set; }//9887
        public float gas_channel_3_ramp_minute { get; set; }//9888
        public float gas_channel_3_ramp_second { get; set; }//9889
        public float gas_channel_4_start_flow { get; set; }//9892
        public float gas_channel_4_delay_hour { get; set; }//9893
        public float gas_channel_4_delay_minute { get; set; }//9894
        public float gas_channel_4_delay_second { get; set; }//9895
        public float gas_channel_4_end_flow { get; set; }//9896
        public float gas_channel_4_ramp_hour { get; set; }//9897
        public float gas_channel_4_ramp_minute { get; set; }//9898
        public float gas_channel_4_ramp_second { get; set; }//9899
        public float gas_channel_5_start_flow { get; set; }//9902
        public float gas_channel_5_delay_hour { get; set; }//9903
        public float gas_channel_5_delay_minute { get; set; }//9904
        public float gas_channel_5_delay_second { get; set; }//9905
        public float gas_channel_5_end_flow { get; set; }//9906
        public float gas_channel_5_ramp_hour { get; set; }//9907
        public float gas_channel_5_ramp_minute { get; set; }//9908
        public float gas_channel_5_ramp_second { get; set; }//9909
        public float cath1_operation { get; set; }//9920
        public float cath1_control { get; set; }//9921
        public float cath1_shutter { get; set; }//9922
        public float cath1_delay_hour { get; set; }//9923
        public float cath1_delay_minute { get; set; }//9924
        public float cath1_delay_second { get; set; }//9925
        public float cath1_ramp_hour { get; set; }//9926
        public float cath1_ramp_minute { get; set; }//9927
        public float cath1_ramp_second { get; set; }//9928
        public float cath1_gas_valve { get; set; }//9929
        public float cath1_1_arc_start { get; set; }//9930
        public float cath1_1_arc_end { get; set; }//9931
        public float cath1_2_arc_start { get; set; }//9933
        public float cath1_2_arc_end { get; set; }//9934
        public float cath1_3_arc_start { get; set; }//9935
        public float cath1_3_arc_end { get; set; }//9936
        public float cath4_operation { get; set; }//9940
        public float cath4_control { get; set; }//9941
        public float cath4_shutter { get; set; }//9942
        public float cath4_delay_hour { get; set; }//9943
        public float cath4_delay_minute { get; set; }//9944
        public float cath4_delay_second { get; set; }//9945
        public float cath4_ramp_hour { get; set; }//9946
        public float cath4_ramp_minute { get; set; }//9947
        public float cath4_ramp_second { get; set; }//9948
        public float cath4_gas_valve { get; set; }//9949
        public float cath4_1_arc_start { get; set; }//9950
        public float cath4_1_arc_end { get; set; }//9951
        public float cath4_2_arc_start { get; set; }//9952
        public float cath4_2_arc_end { get; set; }//9953
        public float cath4_3_arc_start { get; set; }//9954
        public float cath4_3_arc_end { get; set; }//9955
        public float plasma_current { get; set; }//9960

        public float cath6_operation { get; set; }//9970
        public float cath6_control { get; set; }//9971
        public float cath6_shutter { get; set; }//9972
        public float cath6_delay { get; set; }//9974

        public float cath6_ramp { get; set; }//9975

        public float cath6_gas_valve { get; set; }//9973
        public float cath6_1_arc_high_start { get; set; }//9976
        public float cath6_1_arc_high_end { get; set; }//9978
        public float cath6_2_arc_high_start { get; set; }//9980
        public float cath6_2_arc_high_end { get; set; }//9982
        public float cath6_3_arc_high_start { get; set; }//9984
        public float cath6_3_arc_high_end { get; set; }//9986
        public float cath6_1_arc_low_start { get; set; }//9977
        public float cath6_1_arc_low_end { get; set; }//9979
        public float cath6_2_arc_low_start { get; set; }//9981
        public float cath6_2_arc_low_end { get; set; }//9983
        public float cath6_3_arc_low_start { get; set; }//9985
        public float cath6_3_arc_low_end { get; set; }//9987
        public float cath6_frequency { get; set; }//9988
        public float cath6_rrt { get; set; }//9989
    }
    public class Select_Item
    {
        public String tag { get; set; }
        public Double value { get; set; }
    }
    public class DC_Cathode_Object
    {
        public string module_name { get; set; }

        private float _operation = 0;
        private float _control = -1;
        private float _shutter = 0;
        private float _t_delay=0;

        private float _t_ramp=0;

        private float _gas_valve=0;
        private float _arc_1_start=0;
        private float _arc_1_end = 0;
        private float _arc_2_start = 0;
        private float _arc_2_end = 0;
        private float _arc_3_start = 0;
        private float _arc_3_end = 0;

        public float operation
        {
            get { return _operation; }
            set { _operation = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "operation", _operation), "CathodeParameterChanged");
            }
        }
        public float control
        {
            get { return _control; }
            set { _control = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "control", _control), "CathodeParameterChanged");
            }
        }
        public float shutter
        {
            get { return _shutter; }
            set { _shutter = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "shutter", _shutter), "CathodeParameterChanged");
            }
        }
        public float t_delay
        {
            get { return _t_delay; }
            set { _t_delay = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "t_delay", _t_delay), "CathodeParameterChanged");
            }
        }
        //public Double delay_minute { get; set; }
        //public Double delay_second { get; set; }
        public float t_ramp
        {
            get { return _t_ramp; }
            set { _t_ramp = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "t_ramp", _t_ramp), "CathodeParameterChanged");
            }
        }
        //public Double ramp_minute { get; set; }
        //public Double ramp_second { get; set; }
        public float gas_valve
        {
            get { return _gas_valve; }
            set { _gas_valve = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "gas_valve", _gas_valve), "CathodeParameterChanged");
            }
        }
        public float arc_1_start
        {
            get { return _arc_1_start; }
            set { _arc_1_start = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_1_start", _arc_1_start), "CathodeParameterChanged");
            }
        }
        public float arc_1_end
        {
            get { return _arc_1_end; }
            set { _arc_1_end = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_1_end", _arc_1_end), "CathodeParameterChanged");
            }
        }
        public float arc_2_start
        {
            get { return _arc_2_start; }
            set { _arc_2_start = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_2_start", arc_2_start), "CathodeParameterChanged");
            }
        }
        public float arc_2_end
        {
            get { return _arc_2_end; }
            set { _arc_2_end = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_2_end", arc_2_end), "CathodeParameterChanged");
            }
        }
        public float arc_3_start
        {
            get { return _arc_3_start; }
            set { _arc_3_start = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_3_start", arc_3_start), "CathodeParameterChanged");
            }
        }
        public float arc_3_end
        {
            get { return _arc_3_end; }
            set { _arc_3_end = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                        Tuple.Create<string, string, float>(module_name, "arc_3_end", arc_3_end), "CathodeParameterChanged");
            }
        }
    }
    public class DC_Pls_Cathode_Object
    {
        private float _operation=0;
        private float _control=-1;
        private float _shutter=0;
        private float _t_delay=0;
        private float _t_ramp=0;
        private float _gas_valve=0;
        private float _arc_1_start_high=0;
        private float _arc_1_end_high=0;
        private float _arc_2_start_high=0;
        private float _arc_2_end_high=0;
        private float _arc_3_start_high = 0;
        private float _arc_3_end_high = 0;
        private float _arc_1_start_low = 0;
        private float _arc_1_end_low = 0;
        private float _arc_2_start_low = 0;
        private float _arc_2_end_low = 0;
        private float _arc_3_start_low = 0;
        private float _arc_3_end_low = 0;
        private float _frequency = 0;
        private float _rrt=500;

        public string module_name { get; set; }

        public float operation
        {
            get { return _operation; }
            set { _operation = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "operation", operation), "CathodeParameterChanged");
            }
        }
        public float control
        {
            get { return _control; }
            set { _control = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "control", control), "CathodeParameterChanged");
            }
        }
        public float shutter
        {
            get { return _shutter; }
            set { _shutter = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "shutter", shutter), "CathodeParameterChanged");
            }
        }
        public float t_delay
        {
            get { return _t_delay; }
            set { _t_delay = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "t_delay", t_delay), "CathodeParameterChanged");
            }
        }
        public float t_ramp
        {
            get { return _t_ramp; }
            set { _t_ramp = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "t_ramp", t_ramp), "CathodeParameterChanged");
            }
        }
        public float gas_valve
        {
            get { return _gas_valve; }
            set { _gas_valve = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "gas_valve", gas_valve), "CathodeParameterChanged");
            }
        }
        public float arc_1_start_high
        {
            get { return _arc_1_start_high; }
            set { _arc_1_start_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_1_start_high", arc_1_start_high), "CathodeParameterChanged");
            }
        }
        public float arc_1_end_high
        {
            get { return _arc_1_end_high; }
            set { _arc_1_end_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_1_end_high", arc_1_end_high), "CathodeParameterChanged");
            }
        }
        public float arc_2_start_high
        {
            get { return _arc_2_start_high; }
            set { _arc_2_start_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_2_start_high", arc_2_start_high), "CathodeParameterChanged");
            }
        }
        public float arc_2_end_high
        {
            get { return _arc_2_end_high; }
            set { _arc_2_end_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_2_end_high", arc_2_end_high), "CathodeParameterChanged");
            }
        }
        public float arc_3_start_high
        {
            get { return _arc_3_start_high; }
            set { _arc_3_start_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_3_start_high", arc_3_start_high), "CathodeParameterChanged");
            }
        }
        public float arc_3_end_high
        {
            get { return _arc_3_end_high; }
            set { _arc_3_end_high = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_3_end_high", arc_3_end_high), "CathodeParameterChanged");
            }
        }
        public float arc_1_start_low
        {
            get { return _arc_1_start_low; }
            set { _arc_1_start_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_1_start_low", arc_1_start_low), "CathodeParameterChanged");
            }
        }
        public float arc_1_end_low
        {
            get { return _arc_1_end_low; }
            set { _arc_1_end_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_1_end_low", arc_1_end_low), "CathodeParameterChanged");
            }
        }
        public float arc_2_start_low
        {
            get { return _arc_2_start_low; }
            set { _arc_2_start_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_2_start_low", arc_2_start_low), "CathodeParameterChanged");
            }
        }
        public float arc_2_end_low
        {
            get { return _arc_2_end_low; }
            set { _arc_2_end_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_2_end_low", arc_2_end_low), "CathodeParameterChanged");
            }
        }
        public float arc_3_start_low
        {
            get { return _arc_3_start_low; }
            set { _arc_3_start_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_3_start_low", arc_3_start_low), "CathodeParameterChanged");
            }
        }
        public float arc_3_end_low
        {
            get { return _arc_3_end_low; }
            set { _arc_3_end_low = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "arc_3_end_low", arc_3_end_low), "CathodeParameterChanged");
            }
        }
        public float frequency
        {
            get { return _frequency; }
            set { _frequency = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "frequency", frequency), "CathodeParameterChanged");
            }
        }
        public float rrt
        {
            get { return _rrt; }
            set { _rrt = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, string, float>, string>(
                            Tuple.Create<string, string, float>(module_name, "rrt", rrt), "CathodeParameterChanged");
            }
        }

    }
    public class Plasma_Parameter
    {
        private float _plasma_current;
        public float plasma_current
        {
            get { return _plasma_current; }
            set { _plasma_current = value;
                WeakReferenceMessenger.Default.Send<Tuple<string, float>, string>(
                            Tuple.Create<string, float>("plasma_current", plasma_current), "PlasmaParameterChanged");
            }
        }
    }
    public class ActualModuleStatus
    {
        #region duration
        public float act_step_time { get; set; }
        public float act_total_ah { get; set; }
        public float act_cath_on_time { get; set; }
        public float act_cath_off_time { get; set; }
        public float act_penning { get; set; }
        public float act_base_temperature { get; set; }
        public float act_AmpMinutes_CARC { get; set; }
        #endregion
        #region heating
        public float act_heating_temperature { get; set; }
        public float act_pump_speed { get; set; }
        public float act_water_sys_mode { get; set; }
        #endregion
        #region bias
        public float act_bias_voltage { get; set; }
        public float act_bias_current { get; set; }
        public float act_bias_delay_time { get; set; }
        public float act_bias_ramp_time { get; set; }
        public float act_rotation { get; set; }
        #endregion
        #region gas flow

        public float act_baratron { get; set; }
        public float act_channel_1_flow { get; set; }
        public float act_channel_1_delay_time { get; set; }
        public float act_channel_1_ramp_time { get; set; }
        public float act_channel_2_flow { get; set; }
        public float act_channel_2_delay_time { get; set; }
        public float act_channel_2_ramp_time { get; set; }
        public float act_channel_3_flow { get; set; }
        public float act_channel_3_delay_time { get; set; }
        public float act_channel_3_ramp_time { get; set; }
        public float act_channel_4_flow { get; set; }
        public float act_channel_4_delay_time { get; set; }
        public float act_channel_4_ramp_time { get; set; }
        public float act_channel_5_flow { get; set; }
        public float act_channel_5_delay_time { get; set; }
        public float act_channel_5_ramp_time { get; set; }

        #endregion
        #region cathode
        public float act_cath1_operation { get; set; }
        public float act_cath1_shutter { get; set; }
        public float act_cath1_gas_valve { get; set; }
        public float act_cath1_delay_time { get; set; }
        public float act_cath1_ramp_time { get; set; }
        public float act_cath1_1_voltage { get; set; }
        public float act_cath1_1_current { get; set; }
        public float act_cath1_2_voltage { get; set; }
        public float act_cath1_2_current { get; set; }
        public float act_cath1_3_voltage { get; set; }
        public float act_cath1_3_current { get; set; }

        public float act_cath4_operation { get; set; }
        public float act_cath4_shutter { get; set; }
        public float act_cath4_gas_valve { get; set; }
        public float act_cath4_delay_time { get; set; }
        public float act_cath4_ramp_time { get; set; }
        public float act_cath4_1_voltage { get; set; }
        public float act_cath4_1_current { get; set; }
        public float act_cath4_2_voltage { get; set; }
        public float act_cath4_2_current { get; set; }
        public float act_cath4_3_voltage { get; set; }
        public float act_cath4_3_current { get; set; }

        public float act_cath6_operation { get; set; }
        public float act_cath6_shutter { get; set; }
        public float act_cath6_gas_valve { get; set; }
        public float act_cath6_delay_time { get; set; }
        public float act_cath6_ramp_time { get; set; }
        public float act_cath6_1_voltage { get; set; }
        public float act_cath6_1_current { get; set; }
        public float act_cath6_2_voltage { get; set; }
        public float act_cath6_2_current { get; set; }
        public float act_cath6_3_voltage { get; set; }
        public float act_cath6_3_current { get; set; }
        public float act_cath6_frequency { get; set; }
        public float act_cath6_rrt { get; set; }

        #endregion
        #region plasma
        public float act_plasma_current { get; set; }

        #endregion

    }
    public class SelectOptionClass
    {
        public string name { get; set; }
        public Dictionary<string, Single> optionsDict { get; set; }
    }
}

