using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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


namespace NewRecipeViewer.Service
{
    /// <summary>
    /// 该类主要用于操作数据库的增删改除
    /// </summary>
    class EditActions:ObservableRecipient,IRecipient<RecipeObject>,IRecipient<StepObject>,IRecipient<Process_SequenceObject>
    {
        private RecipeObject currentRecipe;
        private StepObject currentStep;
        private Process_SequenceObject currentProcess;
        //数据库连接对象
        private OdbcConnection recipeConnection = new OdbcConnection("DSN=工艺名称");
        private OdbcConnection stepConnection = new OdbcConnection("DSN=工艺配方");
        private OdbcConnection heatingConnection = new OdbcConnection("DSN=heating");
        private OdbcConnection glowConnection = new OdbcConnection("DSN=glow");
        private OdbcConnection ionConnection = new OdbcConnection("DSN=ion");
        private OdbcConnection coatingConnection = new OdbcConnection("DSN=coating");
        private OdbcConnection processConnection = new OdbcConnection("DSN=配方排序");
        private Dictionary<string,float> changed_parameters =new Dictionary<string, float>();
        public ICommand SaveStepValueCommand { get; }

        public Dictionary<string,float> Changed_parameters
        {
            get { return changed_parameters; }
            set 
            { 
                changed_parameters = value;
                WeakReferenceMessenger.Default.Send<string, string>("1", "SaveEnableControl");
                OnPropertyChanged();
            }
        }

        public EditActions()
        {
            this.IsActive = true;
            
            //SaveStepValueCommand = new RelayCommand(SaveStepValueToDatabase);
            #region recipe 信息
            WeakReferenceMessenger.Default.Register<RecipeObject, string>(this, "DeleteRecipe", DeleteRecipe);
            WeakReferenceMessenger.Default.Register<RecipeObject, string>(this, "AddRecipe", AddRecipe);
            WeakReferenceMessenger.Default.Register<RecipeObject, string>(this, "EditRecipe", EditRecipe);
            WeakReferenceMessenger.Default.Register<string, string>(this, "SaveAsRecipe", SaveAsRecipe);
            WeakReferenceMessenger.Default.Register<StepObject, string>(this, "AddStep", AddStep);
            WeakReferenceMessenger.Default.Register<StepObject, string>(this, "EditStep", RenameStep);
            WeakReferenceMessenger.Default.Register<Tuple<StepObject,StepObject>, string>(this, "PasteStep", PasteStep);
            WeakReferenceMessenger.Default.Register<StepObject, string>(this, "DeleteStep", DeleteStep);
            WeakReferenceMessenger.Default.Register<string, string>(this, "AddStepSequence", AddProcess_stepToProcessSequence);
            WeakReferenceMessenger.Default.Register<string, string>(this, "InsertStepSequence", InsertProcess_stepToProcessSequence);
            WeakReferenceMessenger.Default.Register<string, string>(this, "ReplaceStepSequence", ReplaceProcess_stepFromProcessSequence);
            WeakReferenceMessenger.Default.Register<string, string>(this, "RemoveStepSequence", RemoveProcess_stepFromProcessSequence);
            #endregion
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "DurationParameterChanged", DurationParameterChanged);
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "HeatingParameterChanged", HeatingParameterChanged);
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "BiasParameterChanged", BiasParameterChanged);
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "BlendControlParameterChanged", BlendControlParameterChanged);
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "PressureControlParameterChanged", PressureControlParameterChanged);
            WeakReferenceMessenger.Default.Register <Tuple<string, string, float>, string> (this,"GasChannelParameterChanged" , GasChannelParameterChanged);
            WeakReferenceMessenger.Default.Register <Tuple<string, string, float>, string> (this, "CathodeParameterChanged", CathodeParameterChanged);
            WeakReferenceMessenger.Default.Register<Tuple<string, float>, string>(this, "PlasmaParameterChanged", PlasmaParameterChanged);
            WeakReferenceMessenger.Default.Register<string, string>(this,"SaveParameter", SaveStepValueToDatabase);
        }

        private void PlasmaParameterChanged(object recipient, Tuple<string, float> message)
        {
            //message    Tuple<string, float>  Parameter_Name    Value
            if (Changed_parameters.Keys.Contains("id9960"))
            {
                Changed_parameters["id9960"] = message.Item2;
            }
            else
            {
                Changed_parameters.Add("id9960", message.Item2);
            }
            Changed_parameters = changed_parameters;
        }

        private void CathodeParameterChanged(object recipient, Tuple<string, string, float> message)
        {
            #region DC Cathode
            /*
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
             */
            #endregion
            #region DC_pls_Cathode
            /*

             */
            #endregion
            Dictionary<string, float> add_dict = new Dictionary<string, float>();
            switch (message.Item1)
            {
                case "cathode_1":
                    switch (message.Item2)
                    {
                        case "operation":
                            add_dict.Add("id9920", message.Item3);
                            
                            break;
                        case "control":
                            add_dict.Add("id9921", message.Item3);
                            
                            break;
                        case "shutter":
                            add_dict.Add("id9922", message.Item3);
                            
                            break;
                        case "t_delay":
                            var temp =CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9923", temp.Item1);
                            add_dict.Add("id9924", temp.Item2);
                            add_dict.Add("id9925", temp.Item3);
                            
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9926", temp1.Item1);
                            add_dict.Add("id9927", temp1.Item2);
                            add_dict.Add("id9928", temp1.Item3);
                            break;
                        case "gas_valve":
                            add_dict.Add("id9929", message.Item3);
                            
                            break;
                        case "arc_1_start":
                            add_dict.Add("id9930", message.Item3);
                            
                            break;
                        case "arc_1_end":
                            add_dict.Add("id9931", message.Item3);
                            
                            break;
                        case "arc_2_start":
                            add_dict.Add("id9932", message.Item3);
                            
                            break;
                        case "arc_2_end":
                            add_dict.Add("id9933", message.Item3);
                            
                            break;
                        case "arc_3_start":
                            add_dict.Add("id9934", message.Item3);
                            
                            break;
                        case "arc_3_end":
                            add_dict.Add("id9935", message.Item3);
                            
                            break;
                        default:
                            break;
                    }
                    break;
                case "cathode_4":
                    switch (message.Item2)
                    {
                        case "operation":
                            add_dict.Add("id9940", message.Item3);
                            
                            break;
                        case "control":
                            add_dict.Add("id9941", message.Item3);
                            
                            break;
                        case "shutter":
                            add_dict.Add("id9942", message.Item3);
                            
                            break;
                        case "t_delay":
                            
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9943", temp.Item1); 
                            add_dict.Add("id9944", temp.Item2); 
                            add_dict.Add("id9945", temp.Item3);
                            
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9946", temp1.Item1);
                            add_dict.Add("id9947", temp1.Item2);
                            add_dict.Add("id9948", temp1.Item3);
                            
                            break;
                        case "gas_valve":
                            add_dict.Add("id9949", message.Item3);
                            
                            break;
                        case "arc_1_start":
                            add_dict.Add("id9950", message.Item3);
                            
                            break;
                        case "arc_1_end":
                            add_dict.Add("id9951", message.Item3);
                            
                            break;
                        case "arc_2_start":
                            add_dict.Add("id9952", message.Item3);
                            
                            break;
                        case "arc_2_end":
                            add_dict.Add("id9953", message.Item3);
                            
                            break;
                        case "arc_3_start":
                            add_dict.Add("id9954", message.Item3);
                            
                            break;
                        case "arc_3_end":
                            add_dict.Add("id9955", message.Item3);
                            
                            break;
                        
                        default:
                            break;
                    }
                    break;
                case "cathode_6":
                    switch (message.Item2)
                    {
                        case "operation":
                            add_dict.Add("Cath6_Operation", message.Item3);
                            
                            break;
                        case "control":
                            add_dict.Add("Cath6_Mode", message.Item3);
                            
                            break;
                        case "shutter":
                            add_dict.Add("Cath6_Shuttle", message.Item3);
                            
                            break;
                        case "t_delay":
                            add_dict.Add("Cath6_T_Delay", message.Item3);
                            
                            break;
                        case "t_ramp":
                            add_dict.Add("Cath6_T_Ramp", message.Item3);
                            
                            break;
                        case "gas_valve":
                            add_dict.Add("Cath6_GasValve", message.Item3);
                            
                            break;
                        case "arc_1_start_high":
                            add_dict.Add("Cath6_1_HI_Current_Start", message.Item3);
                            
                            break;
                        case "arc_1_end_high":
                            add_dict.Add("Cath6_1_HI_Current_End", message.Item3);
                            
                            break;
                        case "arc_2_start_high":
                            add_dict.Add("Cath6_2_HI_Current_Start", message.Item3);
                            
                            break;
                        case "arc_2_end_high":
                            add_dict.Add("Cath6_2_HI_Current_End", message.Item3);
                            
                            break;
                        case "arc_3_start_high":
                            add_dict.Add("Cath6_3_HI_Current_Start", message.Item3);
                            
                            break;
                        case "arc_3_end_high":
                            add_dict.Add("Cath6_3_HI_Current_End", message.Item3);
                            
                            break;
                        case "arc_1_start_low":
                            add_dict.Add("Cath6_1_Lo_Current_Start", message.Item3);
                            
                            break;
                        case "arc_1_end_low":
                            add_dict.Add("Cath6_1_Lo_Current_End", message.Item3);
                            
                            break;
                        case "arc_2_start_low":
                            add_dict.Add("Cath6_2_Lo_Current_Start", message.Item3);
                            
                            break;
                        case "arc_2_end_low":
                            add_dict.Add("Cath6_2_Lo_Current_End", message.Item3);
                            
                            break;
                        case "arc_3_start_low":
                            add_dict.Add("Cath6_3_LO_Current_Start", message.Item3);
                            
                            break;
                        case "arc_3_end_low":
                            add_dict.Add("Cath6_3_LO_Current_End", message.Item3);
                            
                            break;
                        case "frequency":
                            add_dict.Add("Cath6_Freq", message.Item3);
                            
                            break;
                        case "rrt":
                            add_dict.Add("Cath6_RRT", message.Item3);
                            
                            break;
                        default:
                            break;
                    }
                                                  

                    break;
                default:
                    break;
            }
            foreach (var item in add_dict)
            {
                if (Changed_parameters.Keys.Contains(item.Key))
                {
                    Changed_parameters[item.Key] = item.Value;
                }
                else
                {
                    Changed_parameters.Add(item.Key,item.Value);
                }
            }
            Changed_parameters = changed_parameters;
        }

        private void GasChannelParameterChanged(object recipient, Tuple<string, string, float> message)
        {
            /*
                private float _start_flow;//9862
                private float _t_delay;//9863

                private float _end_flow;//9866
                private float _t_ramp;//9867
             */
            Dictionary<string, float> add_dict = new Dictionary<string, float>();
            switch (message.Item1)
            {
                case "gas_channel_1":
                    switch (message.Item2)
                    {
                        case "start_flow":
                            add_dict.Add("id9862", message.Item3);
                            break;
                        case "t_delay":
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9863", temp.Item1);
                            add_dict.Add("id9864", temp.Item2);
                            add_dict.Add("id9865", temp.Item3);
                            break;
                        case "end_flow":
                            add_dict.Add("id9866", message.Item3);
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9867", temp1.Item1);
                            add_dict.Add("id9868", temp1.Item2);
                            add_dict.Add("id9869", temp1.Item3);
                            break;
                        default:
                            break;
                    }
                    break;
                case "gas_channel_2":
                    switch (message.Item2)
                    {
                        case "start_flow":
                            add_dict.Add("id9872", message.Item3);
                            break;
                        case "t_delay":
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9873", temp.Item1);
                            add_dict.Add("id9874", temp.Item2);
                            add_dict.Add("id9875", temp.Item3);
                            break;
                        case "end_flow":
                            add_dict.Add("id9876", message.Item3);
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9877", temp1.Item1);
                            add_dict.Add("id9878", temp1.Item2);
                            add_dict.Add("id9879", temp1.Item3);
                            break;
                        default:
                            break;
                    }
                    break;
                case "gas_channel_3":
                    switch (message.Item2)
                    {
                        case "start_flow":
                            add_dict.Add("id9882", message.Item3);
                            break;
                        case "t_delay":
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9883", temp.Item1);
                            add_dict.Add("id9884", temp.Item2);
                            add_dict.Add("id9885", temp.Item3);
                            break;
                        case "end_flow":
                            add_dict.Add("id9886", message.Item3);
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9887", temp1.Item1);
                            add_dict.Add("id9888", temp1.Item2);
                            add_dict.Add("id9889", temp1.Item3);
                            break;
                        default:
                            break;
                    }
                    break;
                case "gas_channel_4":
                    switch (message.Item2)
                    {
                        case "start_flow":
                            add_dict.Add("id9892", message.Item3);
                            break;
                        case "t_delay":
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9893", temp.Item1);
                            add_dict.Add("id9894", temp.Item2);
                            add_dict.Add("id9895", temp.Item3);
                            break;
                        case "end_flow":
                            add_dict.Add("id9896", message.Item3);
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9897", temp1.Item1);
                            add_dict.Add("id9898", temp1.Item2);
                            add_dict.Add("id9899", temp1.Item3);
                            break;
                        default:
                            break;
                    }
                    break;
                case "gas_channel_5":
                    switch (message.Item2)
                    {
                        case "start_flow":
                            add_dict.Add("id9902", message.Item3);
                            break;
                        case "t_delay":
                            var temp = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9903", temp.Item1);
                            add_dict.Add("id9904", temp.Item2);
                            add_dict.Add("id9905", temp.Item3);
                            break;
                        case "end_flow":
                            add_dict.Add("id9906", message.Item3);
                            break;
                        case "t_ramp":
                            var temp1 = CustomerConverts.ConvertTimeValue(message.Item3);
                            add_dict.Add("id9907", temp1.Item1);
                            add_dict.Add("id9908", temp1.Item2);
                            add_dict.Add("id9909", temp1.Item3);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            foreach (var item in add_dict)
            {
                
                if (Changed_parameters.Keys.Contains(item.Key))
                {
                    Changed_parameters[item.Key] = item.Value;
                }
                else
                {
                    Changed_parameters.Add(item.Key, item.Value);
                }
            }
            Changed_parameters = changed_parameters;
        }
        
        private void PressureControlParameterChanged(object recipient, Tuple<string, float> message)
        {
            /*
                private float _pressure_control;//9855
                private float _pressure_control_gas;//9856
                private float _max_deviation_pressure;//9857
                private float _process_pressure;//9859
                private float _corrrction_flow;//9861
             */
            Tuple<string, float> temp = new Tuple<string, float>("",0);
            switch (message.Item1)
            {
                case "pressure_control":
                    temp = Tuple.Create<string, float>("id9855", message.Item2);
                    
                    break;
                case "pressure_control_gas":
                    temp = Tuple.Create<string, float>("id9856", message.Item2);
                    break;
                case "max_deviation_pressure":
                    temp = Tuple.Create<string, float>("id9857", message.Item2);
                    break;
                case "process_pressure":
                    temp = Tuple.Create<string, float>("id9859", message.Item2);
                    break;
                case "corrrction_flow":
                    temp = Tuple.Create<string, float>("id9861", message.Item2);
                    break;

                default:
                    break;
            }
            if (temp.Item1 == "")   
            {
                return; 
            }
            if (Changed_parameters.Keys.Contains(temp.Item1) )
            {
                Changed_parameters[temp.Item1] = temp.Item2;
            }
            else
            {
                Changed_parameters.Add(temp.Item1, temp.Item2);
            }
            Changed_parameters = changed_parameters;
        }

        private void BlendControlParameterChanged(object recipient, Tuple<string, float> message)
        {
            /*
                private float _gas_blend_control;//9850
                private float _slaver_gas;//9851
                private float _master_gas;//9852
                private float _ratio_slaver;//9853
                private float _ratio_master;//9854
             */
            Tuple<string, float> temp = new Tuple<string, float>("", 0);
            switch (message.Item1)
            {
                case "gas_blend_control":
                    temp = Tuple.Create<string, float>("id9850", message.Item2);
                    break;
                case "slaver_gas":
                    temp = Tuple.Create<string, float>("id9851", message.Item2);
                    break;
                case "master_gas":
                    temp = Tuple.Create<string, float>("id9852", message.Item2);
                    break;
                case "ratio_slaver":
                    temp = Tuple.Create<string, float>("id9853", message.Item2);
                    break;
                case "ratio_master":
                    temp = Tuple.Create<string, float>("id9854", message.Item2);
                    break;
                
                default:
                    break;
            }
            if (temp.Item1 == "")
            {
                return;
            }
            if (Changed_parameters.Keys.Contains(temp.Item1))
            {
                Changed_parameters[temp.Item1] = temp.Item2;
            }
            else
            {
                Changed_parameters.Add(temp.Item1, temp.Item2);
            }
            Changed_parameters = changed_parameters;
        }

        private void BiasParameterChanged(object recipient, Tuple<string, float> message)
        {
            #region
            /*
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
             */
            #endregion
            Dictionary<string, float> add_dict = new Dictionary<string, float>();
            switch (message.Item1)
            {
                case "select_voltage_mode":
                    add_dict.Add("id9820", message.Item2);
                    break;
                case "Start_voltage":
                    add_dict.Add("id9821", message.Item2);
                    break;
                case "Start_current":
                    add_dict.Add("id9822", message.Item2);
                    break;
                case "bias_delay_time":
                    var temp = CustomerConverts.ConvertTimeValue(message.Item2);
                    add_dict.Add("id9823", temp.Item1);
                    add_dict.Add("id9824", temp.Item2);
                    add_dict.Add("id9825", temp.Item3);
                    break;
                case "bias_ramp_time":
                    var temp1 = CustomerConverts.ConvertTimeValue(message.Item2);
                    add_dict.Add("id9826", temp1.Item1);
                    add_dict.Add("id9827", temp1.Item2);
                    add_dict.Add("id9828", temp1.Item3);
                    break;
                case "arc_detection_I":
                    add_dict.Add("id9829", message.Item2);
                    break;
                case "plasma_detection_I":
                    add_dict.Add("id9830", message.Item2);
                    break;
                case "plasma_detection_U":
                    add_dict.Add("id9831", message.Item2);
                    break;
                case "pulse_frequency":
                    add_dict.Add("id9832", message.Item2);
                    break;
                case "pulse_rrt":
                    add_dict.Add("id9833", message.Item2);
                    break;
                case "rotation_speed":
                    add_dict.Add("id9834", message.Item2);
                    break;
                case "bias_control_mode":
                    add_dict.Add("id9835", message.Item2);
                    break;
                case "end_voltage":
                    add_dict.Add("id9836", message.Item2);
                    break;
                case "end_current":
                    add_dict.Add("id9837", message.Item2);
                    break;
                case "U_arc_detection":
                    add_dict.Add("id9838", message.Item2);
                    break;
                case "I_arc_detection":
                    add_dict.Add("id9839", message.Item2);
                    break;
                case "arc_frequency_limit":
                    add_dict.Add("id9840", message.Item2);
                    break;

                default:
                    break;
            }
            foreach (var item in add_dict)
            {
                if (Changed_parameters.Keys.Contains(item.Key))
                {
                    Changed_parameters[item.Key] = item.Value;
                }
                else
                {
                    Changed_parameters.Add(item.Key, item.Value);
                }
            }
            Changed_parameters = changed_parameters;
        }

        private void HeatingParameterChanged(object recipient, Tuple<string, float> message)
        {
            #region
            /*
                private float _heating_power;//9810
                private float _heating_temperature;//9811
                private float _max_temperature;//9812
                private float _min_temperature;//9813
                private float _pump_speed;//9814
                private float _water_system_mode;//9815
             */
            #endregion
            Tuple<string, float> temp = new Tuple<string, float>("", 0);
            switch (message.Item1)
            {
                case "heating_power":
                    temp = Tuple.Create<string, float>("id9810", message.Item2);
                    break;
                case "heating_temperature":
                    temp = Tuple.Create<string, float>("id9811", message.Item2);
                    break;
                case "max_temperature":
                    temp = Tuple.Create<string, float>("id9812", message.Item2);
                    break;
                case "min_temperature":
                    temp = Tuple.Create<string, float>("id9813", message.Item2);
                    break;
                case "pump_speed":
                    temp = Tuple.Create<string, float>("id9814", message.Item2);
                    break;
                case "water_system_mode":
                    temp = Tuple.Create<string, float>("id9815", message.Item2);
                    break;
 
                default:
                    break;
            }
            if (temp.Item1 == "")
            {
                return;
            }
            if (Changed_parameters.Keys.Contains(temp.Item1))
            {
                Changed_parameters[temp.Item1] = temp.Item2;
            }
            else
            {
                Changed_parameters.Add(temp.Item1, temp.Item2);
            }
            Changed_parameters = changed_parameters;
        }

        private void DurationParameterChanged(object recipient, Tuple<string, float> message)
        {
            /*
                private float _total_step_time;
                private float _total_ah;//9803
                private float _cathode_on_time;//9804
                private float _cathode_off_time;//9805
                private float _base_pressure;//9806
                private float _base_temperature;//9808
                private float _minimum_ampMinutes_CARC;//9809
        
             */
            Dictionary<string, float> add_dict = new Dictionary<string, float>();
            switch (message.Item1)
            {
                case "total_step_time":
                    var temp = CustomerConverts.ConvertTimeValue(message.Item2);
                    add_dict.Add("id9800", temp.Item1);
                    add_dict.Add("id9801", temp.Item2);
                    add_dict.Add("id9802", temp.Item3);
                    break;
                case "total_ah":
                    add_dict.Add("id9803", message.Item2);
                    break;
                case "cathode_on_time":
                    add_dict.Add("id9804", message.Item2);
                    break;
                case "cathode_off_time":
                    add_dict.Add("id9804", message.Item2);
                    break;
                case "base_pressure":
                    add_dict.Add("id9806", message.Item2);
                    break;
                case "base_temperature":
                    add_dict.Add("id9808", message.Item2);
                    break;
                case "minimum_ampMinutes_CARC":
                    add_dict.Add("id9809", message.Item2);
                    break;
                default:
                    break;
            }
            foreach (var item in add_dict)
            {
                if (Changed_parameters.Keys.Contains(item.Key))
                {
                    Changed_parameters[item.Key] = item.Value;
                }
                else
                {
                    Changed_parameters.Add(item.Key, item.Value);
                }
            }
            Changed_parameters = changed_parameters;
        }

        private void SaveStepValueToDatabase(object recipient,string step_name)
        {
            if (step_name != currentStep.Step_name.Trim())  
            {
                MessageBox.Show("保存配方步骤和编辑配方步骤,不同名", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            string db;
            string command_str;
            OdbcCommand command = new OdbcCommand();
            switch (currentStep.Step_type)
            {
                case 1:
                    db = "heating";
                    command_str = CustomerConverts.ConvertDictionaryToUpdateString(db, Changed_parameters, 
                        currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim())+";";
                    command.CommandText = command_str;
                    command.Connection = heatingConnection;
                    heatingConnection.Open();
                    command.ExecuteNonQuery();
                    heatingConnection.Close();
                    break;
                case 2:
                    db = "glow";
                    command_str = CustomerConverts.ConvertDictionaryToUpdateString(db, Changed_parameters,
                        currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim()) + ";";
                    command.CommandText = command_str;
                    command.Connection = glowConnection;
                    glowConnection.Open();
                    command.ExecuteNonQuery();
                    glowConnection.Close();
                    break;
                case 3:
                    db = "ion";
                    command_str = CustomerConverts.ConvertDictionaryToUpdateString(db, Changed_parameters,
                        currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim()) + ";";
                    command.Connection = ionConnection;
                    command.CommandText = command_str;
                    ionConnection.Open();
                    command.ExecuteNonQuery();
                    ionConnection.Close();
                    break;
                case 4:
                    db = "coating";
                    command_str = CustomerConverts.ConvertDictionaryToUpdateString(db, Changed_parameters,
                        currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim()) + ";";
                    command.Connection = coatingConnection;
                    command.CommandText = command_str;
                    coatingConnection.Open();
                    command.ExecuteNonQuery();
                    coatingConnection.Close();
                    break;
                default:
                    break;
            }
            WeakReferenceMessenger.Default.Send<string, string>("0", "SaveEnableControl");

        }
        #region Recipe
        private void DeleteRecipe(object recipient, RecipeObject recipe)
        {
            //这个recipe 是需要被删除的配方
            //删除工艺名称表
            Task.Run(()=> { DeleteRecipeFromRecipeTable(recipe.Rev_name.Trim()); });

            //删除配方步骤表
            Task.Run(() => { DeleteRecipeFromStepTable(recipe.Rev_name.Trim()); });
            //删除配方流程表
            Task.Run(() => {DeleteRecipeFromProcessTable(recipe.Rev_name.Trim()); });
            //删除配方加热参数
            Task.Run(() => {DeleteRecipeFromHeatingTable(recipe.Rev_name.Trim()); });
            //删除配方辉光刻蚀参数
            Task.Run(() => {DeleteRecipeFromGlow_dischargeTable(recipe.Rev_name.Trim()); });
            //删除配方离子刻蚀参数
            Task.Run(() => {DeleteRecipeFromIon_etchingTable(recipe.Rev_name.Trim()); });
            //删除配方涂层工艺参数
            Task.Run(() => {DeleteRecipeFromCoatingTable(recipe.Rev_name.Trim()); });


            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>("delete", "flash_recipe");

        }
        /// <summary>
        /// 删除配方名称表中数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromRecipeTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM 工艺名称 WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, recipeConnection);
            recipeConnection.Open();
            command.ExecuteNonQuery();
            recipeConnection.Close();
        }
        /// <summary>
        /// 删除配方步骤表中数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromStepTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM 工艺配方 WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, stepConnection);
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }
        /// <summary>
        /// 删除配方加热工艺表中配方数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromHeatingTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM heating WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }
        /// <summary>
        /// 删除配方工艺辉光刻蚀表中配方数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromGlow_dischargeTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM glow WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }
        /// <summary>
        /// 删除配方工艺,离子刻蚀表中配方数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromIon_etchingTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM ion WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }
        /// <summary>
        /// 删除配方工艺涂层工艺表中配方数据
        /// </summary>
        /// <param name="recipeName"></param>
        private void DeleteRecipeFromCoatingTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM coating WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }
        private void DeleteRecipeFromProcessTable(String recipeName)
        {
            String commandStr = String.Format("DELETE * FROM 配方排序 WHERE Name='{0}';", recipeName);
            OdbcCommand command = new OdbcCommand(commandStr, processConnection);
            processConnection.Open();
            command.ExecuteNonQuery();
            processConnection.Close();
        }

        private void AddRecipe(object recipient, RecipeObject recipe)
        {
            //这个recipe是需要增加的配方 名称可用其他参数初始化
            OdbcCommand command = new OdbcCommand();
            String CommandStr = "Insert Into 工艺名称 (Rev,Name,Date_Time,Releasr_level,Venting_tenperature," +
                "Cathode_uniformity,Substrate_cooling,User) VALUES "
                + String.Format("({0},'{1}','{2}','{3}',{4},{5},{6},{7})",
                recipe.Rcp_rev,
                recipe.Rev_name,
                recipe.Rev_date,
                User_Convert.ConvertIdReleaseToString(recipe.IDrelease),
                recipe.VentingTemp,
                recipe.Cath_unif,
                recipe.ForcedCooling,
                recipe.UseEmRcp);
            command.Connection = recipeConnection;
            command.CommandText = CommandStr;
            recipeConnection.Open();
            command.ExecuteNonQuery();
            recipeConnection.Close();

            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(recipe.Rev_name, "flash_recipe");
        }
        /// <summary>
        /// 修改配方的设置参数
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="recipe"></param>
        private void EditRecipe(object recipient, RecipeObject recipe)
        {
            //用于更新当前配方的配方recipe 是复制更改后的值
            String CommandStr = "Update 工艺名称 Set "
                    + String.Format("Releasr_level='{0}',Cathode_uniformity={1},Venting_tenperature={2},Substrate_cooling={3}",
                    User_Convert.ConvertIdReleaseToString(recipe.IDrelease),
                    recipe.Cath_unif,
                    recipe.VentingTemp,
                    recipe.Coldtrap)
                    + String.Format(" Where Name='{0}';", recipe.Rev_name.Trim());

            OdbcCommand command = new OdbcCommand(CommandStr, recipeConnection);
            //OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            recipeConnection.Open();
            command.ExecuteNonQuery();
            recipeConnection.Close();
            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(recipe.Rev_name, "flash_recipe");
        }
        /// <summary>
        /// 另存配方
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="newRecipeName"></param>
        private void SaveAsRecipe(object recipient, String newRecipeName)
        {

            //另存配方版本表
            Task.Run(() => { 
                SaveAsToRecipeTable(newRecipeName);
            });
            //另存配方步骤表
            Task.Run(() => {
                SaveAsToStepTable(newRecipeName);
            });
            //另存配方流程表
            Task.Run(() => {
                SaveAsToProcessTable(newRecipeName);
            });
            //另存加热表
            Task.Run(() => {
                SaveAsToHeatingTable(newRecipeName);
            });
            //另存辉光刻蚀
            Task.Run(() => {
                SaveAsToGlow_discharge_table(newRecipeName);
            });
            //另存离子刻蚀表
            Task.Run(() => {
                SaveAsToIon_etching_table(newRecipeName);
            });
            //另存涂层工艺表
            Task.Run(() => {
                SaveAsToCoatingTable(newRecipeName);
            });

            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(newRecipeName, "flash_recipe");
        }

        /// <summary>
        /// 另存配方版本表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToRecipeTable(String newRecipeName)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone(); 
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss"; 
            culture.DateTimeFormat.LongTimePattern = ""; 
            Thread.CurrentThread.CurrentCulture = culture;
            String commandText = " insert into 工艺名称 (Name,Date_Time,Rev,Releasr_level,Venting_tenperature,Cathode_uniformity,Substrate_cooling,User) " +
                    String.Format("Select '{0}','{1}',0,'Design Mode',Venting_tenperature,Cathode_uniformity,Substrate_cooling," +
                    "User From 工艺名称", newRecipeName, DateTime.Now.ToString())
                    + String.Format(" Where Name='{0}';", currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandText, recipeConnection);
            recipeConnection.Open();
            command.ExecuteNonQuery();
            recipeConnection.Close();
            
        }
        /// <summary>
        /// 另存配方步骤表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToStepTable(String newRecipeName)
        {
            String commandStr = "insert into 工艺配方(Name,Date_Time,User,Step_Name,Step_type,Recipe_Name) " +
               String.Format("Select '{0}',Date_Time,0,Step_Name,Step_type,'' From 工艺配方 Where Name='{1}';",
               newRecipeName.Trim(),currentRecipe.Rev_name);
            OdbcCommand command = new OdbcCommand(commandStr, stepConnection);
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }
        /// <summary>
        /// 另存配方流程表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToProcessTable(String newRecipeName)
        {
            String commandStr = "insert into 配方排序(SEQ_NO,Block,Step_Name,Name,Step_type,Recipe_Name,Date_Time	,User) " +
               String.Format("Select SEQ_NO,Block,Step_Name,'{0}',Step_type,'',Date_Time,User From 配方排序 Where Name='{1}';",
               newRecipeName.Trim(), currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, processConnection);
            processConnection.Open();
            command.ExecuteNonQuery();
            processConnection.Close();
        }
        /// <summary>
        /// 另存配方加热参数表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToHeatingTable(String newRecipeName)
        {
            string commandStr = "insert into heating(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9806,id9808,id9810," +
                "id9811,id9814,id9815,id9834,id9922,id9942) " +
               string.Format("Select Date_Time,'{0}',Step_Name,id9800,id9801,id9802,id9806,id9808,id9810,id9811,id9814,id9815," +
               "id9834,id9922,id9942 From heating Where Name='{1}';",
               newRecipeName.Trim(), 
               currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }
        /// <summary>
        /// 另存配方辉光刻蚀参数表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToGlow_discharge_table(String newRecipeName)
        {
            String commandStr = "insert into glow(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                "id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859," +
                "id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882," +
                "id9883,id9884,id9885,id9886,id9887,id9888,id9889,id9892,id9893,id9894," +
                "id9895,id9896,id9897,id9898,id9899,id9902,id9903,id9904,id9905,id9906," +
                "id9907,id9908,id9909,id9922,id9923,id9942,id9943,id9960) " +
               String.Format("Select Date_Time,'{0}',Step_Name,id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                "id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859," +
                "id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882," +
                "id9883,id9884,id9885,id9886,id9887,id9888,id9889,id9892,id9893,id9894," +
                "id9895,id9896,id9897,id9898,id9899,id9902,id9903,id9904,id9905,id9906," +
                "id9907,id9908,id9909,id9922,id9923,id9942,id9943,id9960 From glow Where Name='{1}';",
               newRecipeName.Trim(),
               currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }
        /// <summary>
        /// 另存配方离子刻蚀参数表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToIon_etching_table(String newRecipeName)
        {
            String commandStr = "insert into ion(Date_Time,Step_Name,Name,id9800,id9801,id9802,id9803,id9804,id9805,id9808,id9809," +
                "id9810,id9811,id9812,id9813	,id9814," +
                "id9820,id9821,id9829,id9832,id9833,id9834,id9872,id9873,	id9874,	id9875,	id9876,	id9877,	id9878,	id9879," +
                "id9920,	id9921,	id9922,	id9923,	id9924,	id9925,	id9926,	id9927,	id9928,id9929,	id9930,	id9931,	id9932,	id9933," +
                "id9934,	id9935,	id9940,	id9941,	id9942,	id9943,	id9944,	id9945,	id9946,	id9947,	id9948,	id9949,	id9950,	id9951," +
                "id9952,	id9953,	id9954,	id9955,	id9960) " +
               String.Format("Select Date_Time,Step_Name,'{0}',id9800,id9801,id9802,id9803,id9804,id9805,id9808,id9809," +
                "id9810,id9811,id9812,id9813	,id9814," +
                "id9820,id9821,id9829,id9832,id9833,id9834,id9872,id9873,	id9874,	id9875,	id9876,	id9877,	id9878,	id9879," +
                "id9920,	id9921,	id9922,	id9923,	id9924,	id9925,	id9926,	id9927,	id9928,id9929,	id9930,	id9931,	id9932,	id9933," +
                "id9934,	id9935,	id9940,	id9941,	id9942,	id9943,	id9944,	id9945,	id9946,	id9947,	id9948,	id9949,	id9950,	id9951," +
                "id9952,	id9953,	id9954,	id9955,	id9960 From ion Where Name='{1}';",
               newRecipeName.Trim(),
               currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }
        /// <summary>
        /// 另存配方涂层工艺参数表
        /// </summary>
        /// <param name="newRecipeName"></param>
        private void SaveAsToCoatingTable(String newRecipeName)
        {
            String commandStr = "insert into coating(Date_Time,Step_Name,Name,id9800,id9801,id9802,id9803,id9808,id9810,id9811," +
                "id9814,id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829,id9832," +
                "id9833,id9834,id9836,id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857," +
                "id9859,id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869,id9872," +
                "id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882,id9883,id9884,id9885,id9886," +
                "id9887,id9888,id9889,id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899,id9902," +
                "id9903,id9904,id9905,id9906,id9907,id9908,id9909,id9920,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940,id9942," +
                "id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951,id9952,id9953,id9954," +
                "id9955,Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                "Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End,Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End," +
                "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_LO_Current_Start,Cath6_3_LO_Current_End," +
                "Cath6_Freq,Cath6_RRT) " +
               String.Format("Select Date_Time, Step_Name,'{0}', id9800, id9801, id9802, id9803, id9808, id9810, id9811, " +
                "id9814,id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829,id9832," +
                "id9833,id9834,id9836,id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857," +
                "id9859,id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869,id9872," +
                "id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882,id9883,id9884,id9885,id9886," +
                "id9887,id9888,id9889,id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899,id9902," +
                "id9903,id9904,id9905,id9906,id9907,id9908,id9909,id9920,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940,id9942," +
                "id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951,id9952,id9953,id9954," +
                "id9955,Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                "Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End,Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End," +
                "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_LO_Current_Start,Cath6_3_LO_Current_End," +
                "Cath6_Freq,Cath6_RRT From coating Where Name='{1}';",
               newRecipeName.Trim(),
               currentRecipe.Rev_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }
        #endregion
        #region  Step 
        /// <summary>
        /// 新增工艺步骤
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="step"></param>
        private void AddStep(object recipient, StepObject step)
        {
            //新增 步骤 到配方步骤 表
            Add_StepToStep_table(step);
            //新增初始化参数
            switch (step.Step_type)
            {
                case 1://加热步骤
                    Add_heatingParameters(step.Step_name.Trim());
                    break;
                case 2://辉光刻蚀步骤
                    Add_glowParameters(step.Step_name.Trim());
                    break;
                case 3://离子刻蚀步骤
                    Add_ionParameters(step.Step_name.Trim());
                    break;
                case 4://涂层工艺步骤
                    Add_coatingParameters(step.Step_name.Trim());
                    break;
                default:
                    break;
            }

            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(step.Step_name, "flash_step");
        }
        private void Add_StepToStep_table(StepObject step)
        {
            String commandStr = "insert into 工艺配方(Name,Date_Time,User,Step_Name,Step_type,Recipe_Name) VALUES" +
                String.Format("('{0}','{1}',0,'{2}',{3},'');", currentRecipe.Rev_name.Trim(), DateTime.Now.ToString(), step.Step_name, step.Step_type);
            OdbcCommand command = new OdbcCommand();
            command.Connection = stepConnection;
            command.CommandText = commandStr;
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }

        private void Add_heatingParameters(String newStepName)
        {
            String commandStr = "INSERT INTO heating(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9806,id9808,id9810,id9811,id9814,id9815,id9834,id9922,id9942) Values" +
                String.Format("('{0}','{1}','{2}',1,0,0,4e-05,380,100,400,3,0,1,0,0);", DateTime.Now.ToString(), currentRecipe.Rev_name, newStepName);
            
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }
        private void Add_glowParameters(String newStepName)
        {
            String commandStr = "INSERT INTO glow(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                "id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859," +
                "id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882," +
                "id9883,id9884,id9885,id9886,id9887,id9888,id9889,id9892,id9893,id9894," +
                "id9895,id9896,id9897,id9898,id9899,id9902,id9903,id9904,id9905,id9906," +
                "id9907,id9908,id9909,id9922,id9923,id9942,id9943,id9960) Values" +
                String.Format("('{0}','{1}','{2}',1,30,0,450,100,400,450,440,3,3,200,10,0,0,0,0,0,0,10," +
                "100,50,40,5,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,60,0,0,0" +
                ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,80);", DateTime.Now.ToString(), currentRecipe.Rev_name, newStepName);
            
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }

        private void Add_ionParameters(String newStepName)
        {
            String commandStr = "INSERT INTO ion(" + "Date_Time,Step_Name,Name," +
                "id9800,id9801,id9802,id9803,id9804,id9805,id9808,id9809,id9810,id9811,id9812," +
                "id9813,id9814,id9820,id9821,id9829,id9832,id9833,id9834,id9872,id9873,id9874," +
                "id9875,id9876,id9877,id9878,id9879,id9920,id9921,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940," +
                "id9941,id9942,id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951," +
                "id9952,id9953,id9954,id9955,id9960)VALUES(" + String.Format("'{0}','{1}','{2}',1,0,0," +
                "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                "0,0,0,0,0,0,0,0,0,0)", DateTime.Now.ToString(), currentRecipe.Rev_name.Trim(), newStepName.Trim());
            
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }

        private void Add_coatingParameters(String newStepName)
        {
            String commandStr = "INSERT INTO coating(" +
            "Date_Time,Step_Name,Name,id9800,id9801,id9802,id9803,id9808,id9810,id9811," +
                "id9814,id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829,id9832," +
                "id9833,id9834,id9836,id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857," +
                "id9859,id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869,id9872," +
                "id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882,id9883,id9884,id9885,id9886," +
                "id9887,id9888,id9889,id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899,id9902," +
                "id9903,id9904,id9905,id9906,id9907,id9908,id9909,id9920,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940,id9942," +
                "id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951,id9952,id9953,id9954," +
                "id9955,Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                "Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End,Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End," +
                "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_LO_Current_Start,Cath6_3_LO_Current_End," +
                "Cath6_Freq,Cath6_RRT" + ")VALUES" + String.Format("('{0}','{1}','{2}',0,0,10,5000,280,100,50,2,1,50,0,0,0," +
                "0,0,0,30,40,5,3,0,0,0,0,0,0,1,1,9.99999974737875e-05,0.0399999991059303,8,1000,0,0,0,0,0,0,0,0,0,0,0,0,0" +
                ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0" +
                ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);", DateTime.Now.ToString(), newStepName, currentRecipe.Rev_name);
            
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }

        /// <summary>
        /// 编辑工艺步骤--对工艺步骤进行重新命名
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="step"></param>
        private void RenameStep(object recipient, StepObject step)
        {
            //修改 step 表
            RenameStepToStep_table(step.Step_name.Trim());
            //修改参数表
            switch (step.Step_type)
            {
                case 1://加热步骤
                    RenameStepToHeating_table(step.Step_name.Trim());
                    break;
                case 2://辉光刻蚀步骤
                    RenameStepToGlow_discharge_table(step.Step_name.Trim());
                    break;
                case 3://离子刻蚀步骤
                    RenameStepToIon_etching_table(step.Step_name.Trim());
                    break;
                case 4://涂层工艺步骤
                    RenameStepToCoating_table(step.Step_name.Trim());
                    break;
                default:
                    break;
            }
            //修改流程表
            RenameStepToProcess_table(step.Step_name.Trim());

            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(step.Step_name, "flash_step");
            WeakReferenceMessenger.Default.Send<String, String>("Replace", "flash_process_sequeue");
        }
        /// <summary>
        /// 更新配方步骤中的配方步名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToStep_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE 工艺配方 SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName,currentRecipe.Rev_name,currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, stepConnection);
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }
        /// <summary>
        /// 更新配方流程步骤中的配方步骤名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToProcess_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE 配方排序 SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName, currentRecipe.Rev_name, currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, processConnection);
            processConnection.Open();
            command.ExecuteNonQuery();
            processConnection.Close();
        }
        /// <summary>
        /// 更新加热表中配方步骤名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToHeating_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE heating SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName, currentRecipe.Rev_name, currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }
        /// <summary>
        /// 更新辉光刻蚀表中配方步骤名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToGlow_discharge_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE glow SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName, currentRecipe.Rev_name, currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }
        /// <summary>
        /// 更新离子刻蚀表中配方步骤名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToIon_etching_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE ion SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName, currentRecipe.Rev_name, currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }
        /// <summary>
        /// 更新涂层工艺表中配方步骤名称
        /// </summary>
        /// <param name="newStepName"></param>
        private void RenameStepToCoating_table(String newStepName)
        {
            String commandStr = String.Format("UPDATE ion SET Step_Name ='{0}' WHERE Name='{1}' And Step_Name='{2}';",
                newStepName, currentRecipe.Rev_name, currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }

        private void PasteStep(object recipient, Tuple<StepObject,StepObject> step_tuple)
        {
            var copy_step = step_tuple.Item1;
            var new_step = step_tuple.Item2;
            //新增到step表
            PasteStepToStepTable(copy_step, new_step);
            //复制参数到参数表
            switch (copy_step.Step_type)
            {
                case 1://加热步骤
                    PasteHeatingStep(copy_step, new_step);
                    break;
                case 2://辉光刻蚀步骤
                    PasteGlow_dischargeStep(copy_step, new_step);
                    break;
                case 3://离子刻蚀步骤
                    PasteIon_etchingStep(copy_step, new_step);
                    break;
                case 4://涂层工艺步骤
                    PasteCoatingStep(copy_step, new_step);
                    break;
                default:
                    break;
            }
            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>(new_step.Step_name, "flash_step");
        }
        private void PasteStepToStepTable(StepObject copy_step,StepObject new_step)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
            String commandStr = "insert into 工艺配方(Name,Date_Time,User,Step_Name,Step_type,Recipe_Name) " +
               String.Format("Select '{0}','{1}',0,'{4}',Step_type,'' From 工艺配方 Where Name='{2}' And Step_Name='{3}';",
               currentRecipe.Rev_name.Trim(), DateTime.Now.ToString(),copy_step.Rev_name.Trim(), copy_step.Step_name,new_step.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, stepConnection);
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }

        private void PasteHeatingStep(StepObject copy_step, StepObject new_step)
        {
            String commandStr = "Insert into heating(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9806,id9808,id9810," +
                "id9811,id9814,id9815,id9834,id9922,id9942) " +
               String.Format("Select '{0}','{1}','{4}',id9800,id9801,id9802,id9806,id9808,id9810,id9811,id9814,id9815," +
               "id9834,id9922,id9942 From heating Where Name='{2}' And Step_Name='{3}';",
               DateTime.Now.ToString(),
               currentRecipe.Rev_name.Trim(),
               copy_step.Rev_name.Trim(),
               copy_step.Step_name.Trim(),new_step.Step_name);
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }
        private void PasteGlow_dischargeStep(StepObject copy_step, StepObject new_step)
        {
            String commandStr = "insert into glow(Date_Time,Name,Step_Name,id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                "id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859," +
                "id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882," +
                "id9883,id9884,id9885,id9886,id9887,id9888,id9889,id9892,id9893,id9894," +
                "id9895,id9896,id9897,id9898,id9899,id9902,id9903,id9904,id9905,id9906," +
                "id9907,id9908,id9909,id9922,id9923,id9942,id9943,id9960) " +
               String.Format("Select '{0}','{1}','{4}',id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                "id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859," +
                "id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882," +
                "id9883,id9884,id9885,id9886,id9887,id9888,id9889,id9892,id9893,id9894," +
                "id9895,id9896,id9897,id9898,id9899,id9902,id9903,id9904,id9905,id9906," +
                "id9907,id9908,id9909,id9922,id9923,id9942,id9943,id9960 From glow Where Name='{2}' And Step_Name='{3}';",
                DateTime.Now.ToString(),
               currentRecipe.Rev_name.Trim(),
               copy_step.Rev_name.Trim(),
               copy_step.Step_name.Trim(),
               new_step.Step_name.Trim()
               );
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }
        private void PasteIon_etchingStep(StepObject copy_step, StepObject new_step)
        {
            String commandStr = "Insert into glow(Date_Time,Step_Name,Name," +
                "id9800,id9801,id9802,id9803,id9804,id9805,id9808,id9809,id9810,id9811,id9812," +
                "id9813,id9814,id9820,id9821,id9829,id9832,id9833,id9834,id9872,id9873,id9874," +
                "id9875,id9876,id9877,id9878,id9879,id9920,id9921,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940," +
                "id9941,id9942,id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951," +
                "id9952,id9953,id9954,id9955,id9960) " +
               String.Format("Select '{0}','{4}','{1}'," +
                "id9800,id9801,id9802,id9803,id9804,id9805,id9808,id9809,id9810,id9811,id9812," +
                "id9813,id9814,id9820,id9821,id9829,id9832,id9833,id9834,id9872,id9873,id9874," +
                "id9875,id9876,id9877,id9878,id9879,id9920,id9921,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940," +
                "id9941,id9942,id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951," +
                "id9952,id9953,id9954,id9955,id9960 From glow Where Name='{2}' And Step_Name='{3}';",
               currentRecipe.Rev_name.Trim(),
               copy_step.Rev_name.Trim(),
               copy_step.Step_name.Trim(), new_step.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }
        private void PasteCoatingStep(StepObject copy_step, StepObject new_step)
        {
            String commandStr = "insert into coating(Date_Time,Step_Name,Name,id9800,id9801,id9802,id9803,id9808,id9810,id9811," +
                "id9814,id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829,id9832," +
                "id9833,id9834,id9836,id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857," +
                "id9859,id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869,id9872," +
                "id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882,id9883,id9884,id9885,id9886," +
                "id9887,id9888,id9889,id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899,id9902," +
                "id9903,id9904,id9905,id9906,id9907,id9908,id9909,id9920,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940,id9942," +
                "id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951,id9952,id9953,id9954," +
                "id9955,Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                "Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End,Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End," +
                "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_LO_Current_Start,Cath6_3_LO_Current_End," +
                "Cath6_Freq,Cath6_RRT) " +
               String.Format("Select '{0}', '{4}','{1}', id9800, id9801, id9802, id9803, id9808, id9810, id9811, " +
                "id9814,id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829,id9832," +
                "id9833,id9834,id9836,id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857," +
                "id9859,id9861,id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869,id9872," +
                "id9873,id9874,id9875,id9876,id9877,id9878,id9879,id9882,id9883,id9884,id9885,id9886," +
                "id9887,id9888,id9889,id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899,id9902," +
                "id9903,id9904,id9905,id9906,id9907,id9908,id9909,id9920,id9922,id9923,id9924,id9925," +
                "id9926,id9927,id9928,id9929,id9930,id9931,id9932,id9933,id9934,id9935,id9940,id9942," +
                "id9943,id9944,id9945,id9946,id9947,id9948,id9949,id9950,id9951,id9952,id9953,id9954," +
                "id9955,Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                "Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End,Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End," +
                "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_LO_Current_Start,Cath6_3_LO_Current_End," +
                "Cath6_Freq,Cath6_RRT From coating Where Name='{2}' And Step_Name='{3}';",
                DateTime.Now.ToString(),
               currentRecipe.Rev_name.Trim(),
               copy_step.Rev_name.Trim(),
               copy_step.Step_name.Trim(), new_step.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }

        private void DeleteStep(object recipient, StepObject step)
        {
            //删除Step表
            DeleteStepFromStep_table();
            //删除参数表
            switch (currentStep.Step_type)
            {
                case 1://加热步骤
                    DeleteStepFromHeating_table();
                    break;
                case 2://辉光刻蚀步骤
                    DeleteStepFromGlow_discharge_table();
                    break;
                case 3://离子刻蚀步骤
                    DeleteStepFromIon_etching_table();
                    break;
                case 4://涂层工艺步骤
                    DeleteStepFromCoating_table();
                    break;
                default:
                    break;
            }
            //刷新配方表
            WeakReferenceMessenger.Default.Send<String, String>("delete", "flash_step");
        }
        private void DeleteStepFromStep_table()
        {
            String commandStr = String.Format("DELETE * FROM 工艺配方 WHERE Name='{0}' And Step_Name='{1}';",
                currentRecipe.Rev_name.Trim(),currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, stepConnection);
            stepConnection.Open();
            command.ExecuteNonQuery();
            stepConnection.Close();
        }
        private void DeleteStepFromHeating_table()
        {
            String commandStr = String.Format("DELETE * FROM heating WHERE Name='{0}' And Step_Name='{1}';",
                currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, heatingConnection);
            heatingConnection.Open();
            command.ExecuteNonQuery();
            heatingConnection.Close();
        }

        private void DeleteStepFromGlow_discharge_table()
        {
            String commandStr = String.Format("DELETE * FROM glow WHERE Name='{0}' And Step_Name='{1}';",
                currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, glowConnection);
            glowConnection.Open();
            command.ExecuteNonQuery();
            glowConnection.Close();
        }
        private void DeleteStepFromIon_etching_table()
        {
            String commandStr = String.Format("DELETE * FROM ion WHERE Name='{0}' And Step_Name='{1}';",
                currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, ionConnection);
            ionConnection.Open();
            command.ExecuteNonQuery();
            ionConnection.Close();
        }
        private void DeleteStepFromCoating_table()
        {
            String commandStr = String.Format("DELETE * FROM coating WHERE Name='{0}' And Step_Name='{1}';",
                currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            OdbcCommand command = new OdbcCommand(commandStr, coatingConnection);
            coatingConnection.Open();
            command.ExecuteNonQuery();
            coatingConnection.Close();
        }
        #endregion
        #region Process Step 
        private void AddProcess_stepToProcessSequence(Object obi,String message)
        {
            String command_str = "SELECT SEQ_NO,Block,Step_Name,Name FROM "
                + "配方排序" + " where [Name] = '" + currentRecipe.Rev_name.Trim()
                + "' order by SEQ_NO DESC;";
           
            OdbcCommand command = new OdbcCommand(command_str, processConnection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable sequeue_table = new DataTable();
            processConnection.Open();
            dataAdapter.Fill(sequeue_table);
            
            int max_id = 0;
            if (sequeue_table.Rows.Count==0)
            {
                max_id = 0;
            }
            else
            {
                max_id = Convert.ToInt16(sequeue_table.Rows[0][0].ToString());
                //Process_SequenceObject max_process = new Process_SequenceObject()
                //{
                //    Seq_no = Convert.ToInt16(sequeue_table.Rows[0][0].ToString()),
                //    Rep_blok = Convert.ToInt16(sequeue_table.Rows[0][1].ToString()),
                //    Step_name = sequeue_table.Rows[0][2].ToString(),
                //    Rev_name = sequeue_table.Rows[0][3].ToString()
                //};
            }
            //新增流程步骤
            String CommandStr = "Insert into 配方排序(SEQ_NO,Block,Step_Name,Name,Step_type,Recipe_Name,Date_Time," +
                "User)Values" +
                String.Format("({0},0,'{1}','{2}',{3},'','{4}',0)",
                max_id+1,
                currentStep.Step_name.Trim(),
                currentRecipe.Rev_name.Trim(),
                currentStep.Step_type,
                DateTime.Now.ToString()
                );
            command.CommandText = CommandStr;
            
            command.ExecuteNonQuery();
            processConnection.Close();
            WeakReferenceMessenger.Default.Send<String, String>("Add", "flash_process_sequeue");
        }
        private void InsertProcess_stepToProcessSequence(Object obj,String message)
        {
            if (currentStep is null)
            {
                MessageBox.Show("请先选择要插入工艺步骤(Step)!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (currentStep is null)
            {
                MessageBox.Show("请先选择要插入的工艺流程位置(Recipe Seq)!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //倒序查询
            String command_str = "SELECT SEQ_NO,Block,Step_Name,Name FROM "
                + "配方排序" + " where [Name] = '" + currentRecipe.Rev_name.Trim()
                + String.Format("' And SEQ_NO >={0} order by SEQ_NO DESC;",currentProcess.Seq_no);
           
            OdbcCommand command = new OdbcCommand(command_str, processConnection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable sequeue_table = new DataTable();
            processConnection.Open();
            dataAdapter.Fill(sequeue_table);
            
            ObservableCollection<Process_SequenceObject> recipe_sequence = new ObservableCollection<Process_SequenceObject>();
            recipe_sequence.Clear();
            //将序号后移提供控件给当前步骤
            foreach (DataRow row in sequeue_table.Rows)
            {
                int item_no = Convert.ToInt16(row[0].ToString());
                String commandStr = String.Format("UPDATE 配方排序 SET SEQ_NO={0} WHERE Name='{1}' And SEQ_NO={2};"
                    , item_no + 1, currentRecipe.Rev_name.Trim(), item_no);
                command.CommandText = commandStr;
                command.ExecuteNonQuery();
            }

            
           
            //插入当前步骤
            String commandString = "Insert into 配方排序(SEQ_NO,Block,Step_Name,Name,Step_type,Recipe_Name,Date_Time,User)Values" +
                String.Format("({0},0,'{1}','{2}',{3},'','{4}',0)",
                currentProcess.Seq_no,
                currentStep.Step_name.Trim(),
                currentRecipe.Rev_name.Trim(),
                currentStep.Step_type,
                DateTime.Now.ToString()
                );
            command.CommandText = commandString;
            command.ExecuteNonQuery();
            processConnection.Close();
            WeakReferenceMessenger.Default.Send<String, String>("Insert", "flash_process_sequeue");
        }
        private void RemoveProcess_stepFromProcessSequence(Object obj, String message)
        {
            OdbcCommand command = new OdbcCommand();
            command.Connection = processConnection;
            if (currentProcess is null)
            {
                MessageBox.Show("请先选择要删除的，工艺流程步骤", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //先删除相关步骤
            String commandString = String.Format("DELETE * FROM 配方排序 WHERE Name = '{0}' And SEQ_NO = {1};",
                currentRecipe.Rev_name.Trim(),currentProcess.Seq_no);
            command.CommandText = commandString;
            processConnection.Open();
            command.ExecuteNonQuery();

            //将后面的步骤序号向前移动  ---正序查询
            String command_str = "SELECT SEQ_NO,Block,Step_Name,Name FROM "
                + "配方排序" + " where [Name] = '" + currentRecipe.Rev_name.Trim()
                + String.Format("' And SEQ_NO >{0} order by SEQ_NO Asc;", currentProcess.Seq_no);

            command.CommandText = command_str;
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable sequeue_table = new DataTable();
            dataAdapter.Fill(sequeue_table);
            ObservableCollection<Process_SequenceObject> recipe_sequence = new ObservableCollection<Process_SequenceObject>();
            recipe_sequence.Clear();
            foreach (DataRow row in sequeue_table.Rows)
            {
                int item_no = Convert.ToInt16(row[0].ToString());
                String commandStr = String.Format("UPDATE 配方排序 SET SEQ_NO={0} WHERE Name='{1}' And SEQ_NO={2};"
                    , item_no - 1, currentRecipe.Rev_name.Trim(), item_no);
                command.CommandText = commandStr;
                command.ExecuteNonQuery();
            }

            processConnection.Close();
            WeakReferenceMessenger.Default.Send<String, String>("Remove", "flash_process_sequeue");
        }

        private void ReplaceProcess_stepFromProcessSequence(Object obj, String message)
        {
            if (currentStep is null)
            {
                MessageBox.Show("请先选择要替换到工艺流程的工艺步骤!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (currentStep is null)
            {
                MessageBox.Show("请先选择要替换的工艺流程步骤!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            String commandStr = String.Format("UPDATE 配方排序 SET Step_Name='{0}',Step_type={1} WHERE Name='{2}' And SEQ_NO={3};",
                currentStep.Step_name.Trim(),currentStep.Step_type, currentRecipe.Rev_name.Trim(),currentProcess.Seq_no);
            OdbcCommand command = new OdbcCommand(commandStr, processConnection);
            processConnection.Open();
            command.ExecuteNonQuery();
            processConnection.Close();
            WeakReferenceMessenger.Default.Send<String, String>("Replace", "flash_process_sequeue");
        }
        #endregion
        #region Step Value
        private void LoadHeatingStepValue()
        {
            String commandstr = "SELECT id9800,id9801,id9802,id9806,id9808,id9810,id9811,id9814,id9815,id9834,id9922,id9942 "
                        + "From heating" + " Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            OdbcConnection connection_heating = new OdbcConnection("DSN=heating;");
            OdbcCommand command = new OdbcCommand(commandstr, connection_heating);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection_heating.Open();
            dataAdapter.Fill(param_table);
            connection_heating.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];
            var duration_param = new DurationParameters();
            var heating_param = new HeatingParameters();
            var bias_param = new BiasParameters();
            var blend_control_param = new GasBlendControlParameters();
            var pressure_control_param = new GasPressureControlParameters();
            var gas_channel_1_param = new GasChannelParameters() { module_name = "gas_channel_1"};
            var gas_channel_2_param = new GasChannelParameters() { module_name = "gas_channel_2"};
            var gas_channel_3_param = new GasChannelParameters() { module_name = "gas_channel_3"};
            var gas_channel_4_param = new GasChannelParameters() { module_name = "gas_channel_4"};
            var gas_channel_5_param = new GasChannelParameters() { module_name = "gas_channel_5"};
            var cathode_1_param = new DC_Cathode_Object() { module_name = "cathode_1"};
            var cathode_4_param = new DC_Cathode_Object() { module_name = "cathode_4"};
            var cathode_6_param = new DC_Pls_Cathode_Object() { module_name = "cathode_6"};
            var plasma_param = new Plasma_Parameter();

            duration_param.total_step_time = (float)row[0]*3600+ (float)row[1]*60+(float)row[2];
            duration_param.base_pressure = (float)row[3];
            duration_param.base_temperature = (float)row[4];
            heating_param.heating_power = (float)row[5];
            heating_param.heating_temperature = (float)row[6];
            heating_param.pump_speed = (float)row[7];
            heating_param.water_system_mode = (float)row[8];
            bias_param.rotation_speed = (float)row[9];
            cathode_1_param.shutter = (float)row[10];
            cathode_4_param.shutter = (float)row[11];

            #region Send Message
            WeakReferenceMessenger.Default.Send<DurationParameters>(duration_param);
            WeakReferenceMessenger.Default.Send<HeatingParameters>(heating_param);
            WeakReferenceMessenger.Default.Send<BiasParameters>(bias_param);
            WeakReferenceMessenger.Default.Send<GasBlendControlParameters>(blend_control_param);
            WeakReferenceMessenger.Default.Send<GasPressureControlParameters>(pressure_control_param);

            
            WeakReferenceMessenger.Default.Send<List<GasChannelParameters>>(
                new List<GasChannelParameters>() { gas_channel_1_param , 
                    gas_channel_2_param ,gas_channel_3_param,gas_channel_4_param,gas_channel_5_param
                });
            WeakReferenceMessenger.Default.Send<Tuple<DC_Cathode_Object, DC_Cathode_Object>>(
                Tuple.Create<DC_Cathode_Object, DC_Cathode_Object>(cathode_1_param, cathode_4_param)
                );
            WeakReferenceMessenger.Default.Send<DC_Pls_Cathode_Object>(cathode_6_param);
            WeakReferenceMessenger.Default.Send<Plasma_Parameter>(plasma_param);
            #endregion

        }
        private void LoadGlowDischargeStepValue()
        {
            String commandstr = "SELECT id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                        "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                        "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                        "id9922,id9942,id9960 "
                        + "From glow" + " Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            OdbcConnection connection = new OdbcConnection("DSN=glow;");
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            connection.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];
            var duration_param = new DurationParameters();
            var heating_param = new HeatingParameters();
            var bias_param = new BiasParameters();
            
            var cathode_1_param = new DC_Cathode_Object() { module_name = "cathode_1" };
            var cathode_4_param = new DC_Cathode_Object() { module_name = "cathode_4" };
            var cathode_6_param = new DC_Pls_Cathode_Object() { module_name = "cathode_6" };
            var plasma_param = new Plasma_Parameter();
            #region time
            duration_param.total_step_time = (float)row[0]*3600+(float)row[1]*60+(float)row[2];

            #endregion
            #region heating
            duration_param.base_temperature = (float)row[3];
            heating_param.heating_power = (float)row[4];
            heating_param.heating_temperature = (float)row[5];
            heating_param.max_temperature = (float)row[6];
            heating_param.min_temperature = (float)row[7];
            heating_param.pump_speed = (float)row[8];
            #endregion
            #region bias
            bias_param.select_voltage_mode = (float)row[9];
            bias_param.Start_voltage = (float)row[10];
            bias_param.Start_current = (float)row[11];
            bias_param.bias_delay_time = (float)row[12]*3600+ (float)row[13]*60+ (float)row[14];
            
            bias_param.bias_ramp_time = (float)row[15] * 3600 + (float)row[16] * 60 + (float)row[17];
            
            bias_param.arc_detection_I = (float)row[18];


            bias_param.plasma_detection_I = (float)row[19];
            bias_param.plasma_detection_U = (float)row[20];
            bias_param.pulse_frequency = (float)row[21];
            bias_param.pulse_rrt = (float)row[22];
            bias_param.rotation_speed = (float)row[23];
            bias_param.bias_control_mode = (float)row[24];
            bias_param.end_voltage = (float)row[25];
            bias_param.end_current = (float)row[26];
            bias_param.U_arc_detection = (float)row[27];
            bias_param.I_arc_detection = (float)row[28];
            bias_param.arc_frequency_limit = (float)row[29];
            #endregion

            #region cathode
            cathode_1_param.shutter = (float)row[30];
            cathode_4_param.shutter = (float)row[31];
            plasma_param.plasma_current = (float)row[32];
            #endregion
            #region Send Message
            WeakReferenceMessenger.Default.Send<DurationParameters>(duration_param);
            WeakReferenceMessenger.Default.Send<HeatingParameters>(heating_param);
            WeakReferenceMessenger.Default.Send<BiasParameters>(bias_param);
            //WeakReferenceMessenger.Default.Send<Tuple<GasBlendControlParameters, GasPressureControlParameters>>(
            //    Tuple.Create<GasBlendControlParameters, GasPressureControlParameters>(blend_control_param, pressure_control_param));
            //WeakReferenceMessenger.Default.Send<List<GasChannelParameters>>(
            //    new List<GasChannelParameters>() { gas_channel_1_param ,
            //        gas_channel_2_param ,gas_channel_3_param,gas_channel_4_param,gas_channel_5_param
            //    });
            WeakReferenceMessenger.Default.Send<Tuple<DC_Cathode_Object, DC_Cathode_Object>>(
                Tuple.Create<DC_Cathode_Object, DC_Cathode_Object>(cathode_1_param, cathode_4_param)
                );
            WeakReferenceMessenger.Default.Send<DC_Pls_Cathode_Object>(cathode_6_param);
            WeakReferenceMessenger.Default.Send<Plasma_Parameter>(plasma_param);
            #endregion

        }
        private void LoadGasControlParamters(string datatable)
        {
            //icon Each 工艺
            String commandstr = "SELECT id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859,id9861 " +
                "From " + datatable +
                " Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            String dsn = "DSN=" + datatable + ";";
            OdbcConnection connection = new OdbcConnection(dsn);
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            connection.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];
            var blend_control_param = new GasBlendControlParameters();
            var pressure_control_param = new GasPressureControlParameters();
            #region Gas Control
            blend_control_param.gas_blend_control = (float)row[0];
            blend_control_param.slaver_gas = (float)row[1];
            blend_control_param.master_gas = (float)row[2];
            blend_control_param.ratio_slaver = (float)row[3];
            blend_control_param.ratio_master = (float)row[4];
            pressure_control_param.pressure_control = (float)row[5];
            pressure_control_param.pressure_control_gas = (float)row[6];
            pressure_control_param.max_deviation_pressure = (float)row[7];
            pressure_control_param.process_pressure = (float)row[8];
            pressure_control_param.corrrction_flow = (float)row[9];
            #endregion
            WeakReferenceMessenger.Default.Send<GasBlendControlParameters>(blend_control_param);
            WeakReferenceMessenger.Default.Send<GasPressureControlParameters>(pressure_control_param);
        }
        private void LoadGasChannelParameters(string datatable)
        {
            //Coating工艺
            String commandstr = "Select " +
                        "id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                        "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879," +
                        "id9882,id9883,id9884,id9885,id9886,id9887,id9888,id9889," +
                        "id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899," +
                        "id9902,id9903,id9904,id9905,id9906,id9907,id9908,id9909 " +
                        "From " + datatable +
                        " Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            String dsn = "DSN=" + datatable + ";";
            OdbcConnection connection = new OdbcConnection(dsn);
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            connection.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];
            #region gas channels 
            var gas_channel_1_param = new GasChannelParameters() { module_name = "gas_channel_1" };
            var gas_channel_2_param = new GasChannelParameters() { module_name = "gas_channel_2" };
            var gas_channel_3_param = new GasChannelParameters() { module_name = "gas_channel_3" };
            var gas_channel_4_param = new GasChannelParameters() { module_name = "gas_channel_4" };
            var gas_channel_5_param = new GasChannelParameters() { module_name = "gas_channel_5" };

            gas_channel_1_param.start_flow = (float)row[0];
            gas_channel_1_param.t_delay = (float)row[1]*3600+ (float)row[2]*60+ (float)row[3];
            
            gas_channel_1_param.end_flow = (float)row[4];
            gas_channel_1_param.t_ramp = (float)row[5] * 3600 + (float)row[6] * 60 + (float)row[7];
            

            gas_channel_2_param.start_flow = (float)row[8];
            
            gas_channel_2_param.t_delay = (float)row[9] * 3600 + (float)row[10] * 60 + (float)row[11]; ;
            
            gas_channel_2_param.end_flow = (float)row[12];
            gas_channel_2_param.t_ramp = (float)row[13] * 3600 + (float)row[14] * 60 + (float)row[15];
            

            gas_channel_3_param.start_flow = (Single)row[16];
            gas_channel_3_param.t_delay = (float)row[17] * 3600 + (float)row[18] * 60 + (float)row[19];
            
            gas_channel_3_param.end_flow = (Single)row[20];
            gas_channel_3_param.t_ramp = (float)row[21] * 3600 + (float)row[22] * 60 + (float)row[23];
            

            gas_channel_4_param.start_flow = (Single)row[24];
            gas_channel_4_param.t_delay = (float)row[25] * 3600 + (float)row[26] * 60 + (float)row[27];
            
            gas_channel_4_param.end_flow = (Single)row[28];
            gas_channel_4_param.t_ramp = (float)row[29] * 3600 + (float)row[30] * 60 + (float)row[31];
            

            gas_channel_5_param.start_flow = (Single)row[32];
            gas_channel_5_param.t_delay = (float)row[33] * 3600 + (float)row[34] * 60 + (float)row[35];
            
            gas_channel_5_param.end_flow = (Single)row[36];
            gas_channel_5_param.t_ramp = (float)row[37] * 3600 + (float)row[38] * 60 + (float)row[39];

            #endregion
            WeakReferenceMessenger.Default.Send<List<GasChannelParameters>>(
                new List<GasChannelParameters>() { gas_channel_1_param ,
                    gas_channel_2_param ,gas_channel_3_param,gas_channel_4_param,gas_channel_5_param
                });
        }
        private void LoadCathodeParameters()
        {
            String commandstr = "Select id9800,id9801,id9802,id9803,id9808,id9810,id9811,id9814," +
                        "id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                        "id9832,id9833,id9834,id9836 From coating "
                        + String.Format("Where Name='{0}' And Step_Name='{1}';",
                        currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            //String commandstr = String.Format("Select * From coating Where Name={0} and Step_Name={1};", 
            //    currentRecipe.Rev_name.Trim(), currentStep.Step_name.Trim());
            OdbcConnection connection = new OdbcConnection("DSN=coating;");
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];

            connection.Close();
            var duration_param = new DurationParameters();
            var heating_param = new HeatingParameters();
            var bias_param = new BiasParameters();
            var plasma_param = new Plasma_Parameter();
            #region time
            duration_param.total_step_time = (float)row[0] * 3600 + (float)row[1] * 60 + (float)row[2];
            duration_param.total_ah = (float)row[3];
            #endregion
            #region heating
            duration_param.base_temperature = (float)row[4];
            heating_param.heating_power = (float)row[5];
            heating_param.heating_temperature = (float)row[6];
            heating_param.pump_speed = (float)row[7];
            #endregion
            #region bias
            bias_param.select_voltage_mode = (float)row[8];
            bias_param.Start_voltage = (float)row[9];

            bias_param.bias_delay_time = (float)row[10] * 3600 + (float)row[11] * 60 + (float)row[12];
            
            bias_param.bias_ramp_time = (float)row[13] * 3600 + (float)row[14] * 60 + (float)row[15];
            
            bias_param.arc_detection_I = (float)row[16];


            bias_param.pulse_frequency = (float)row[17];
            bias_param.pulse_rrt = (float)row[18];
            bias_param.rotation_speed = (float)row[19];

            bias_param.end_voltage = (float)row[20];
            #endregion
            #region Send Message
            WeakReferenceMessenger.Default.Send<DurationParameters>(duration_param);
            WeakReferenceMessenger.Default.Send<HeatingParameters>(heating_param);
            WeakReferenceMessenger.Default.Send<BiasParameters>(bias_param);
            //WeakReferenceMessenger.Default.Send<Tuple<GasBlendControlParameters, GasPressureControlParameters>>(
            //    Tuple.Create<GasBlendControlParameters, GasPressureControlParameters>(blend_control_param, pressure_control_param));
            //WeakReferenceMessenger.Default.Send<List<GasChannelParameters>>(
            //    new List<GasChannelParameters>() { gas_channel_1_param ,
            //        gas_channel_2_param ,gas_channel_3_param,gas_channel_4_param,gas_channel_5_param
            //    });
            //WeakReferenceMessenger.Default.Send<Tuple<DC_Cathode_Object, DC_Cathode_Object, DC_Pls_Cathode_Object>>(
            //    Tuple.Create<DC_Cathode_Object, DC_Cathode_Object, DC_Pls_Cathode_Object>(cathode_1_param, cathode_4_param, cathode_6_param)
            //    );
            WeakReferenceMessenger.Default.Send<Plasma_Parameter>(plasma_param);
            #endregion


        }
        private void LoadCathode1_4_Parameter()
        {
            String commandstr = "Select " +
                        "id9920,id9922,id9923,id9924,id9925,id9926,id9927,id9928,id9929," +
                        "id9930,id9931,id9932,id9933,id9934,id9935," +
                        "id9940,id9942,id9943,id9944,id9945,id9946,id9947,id9948,id9949," +
                        "id9950,id9951,id9952,id9953,id9954,id9955 " +
                        "From coating " + "Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            OdbcConnection connection = new OdbcConnection("DSN=coating;");
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            connection.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];
            #region cathode1
            var cathode_1_param = new DC_Cathode_Object() { module_name = "cathode_1" };
            var cathode_4_param = new DC_Cathode_Object() { module_name = "cathode_4" };
            cathode_1_param.operation = (Single)row[0];

            cathode_1_param.shutter = (Single)row[1];
            cathode_1_param.t_delay = (float)row[2] * 3600 + (float)row[3] * 60 + (float)row[4];
            
            cathode_1_param.t_ramp = (float)row[5] * 3600 + (float)row[6] * 60 + (float)row[7];
            
            cathode_1_param.gas_valve = (Single)row[8];
            cathode_1_param.arc_1_start = (Single)row[9];
            cathode_1_param.arc_1_end = (Single)row[10];
            cathode_1_param.arc_2_start = (Single)row[11];
            cathode_1_param.arc_2_end = (Single)row[12];
            cathode_1_param.arc_3_start = (Single)row[13];
            cathode_1_param.arc_3_end = (Single)row[14];
            #endregion
            #region cathode4
            cathode_4_param.operation = (Single)row[15];

            cathode_4_param.shutter = (Single)row[16];
            cathode_4_param.t_delay = (float)row[17] * 3600 + (float)row[18] * 60 + (float)row[19];
            
            cathode_4_param.t_ramp = (float)row[20] * 3600 + (float)row[21] * 60 + (float)row[2];
            
            cathode_4_param.gas_valve = (Single)row[23];
            cathode_4_param.arc_1_start = (Single)row[24];
            cathode_4_param.arc_1_end = (Single)row[25];
            cathode_4_param.arc_2_start = (Single)row[26];
            cathode_4_param.arc_2_end = (Single)row[27];
            cathode_4_param.arc_3_start = (Single)row[28];
            cathode_4_param.arc_3_end = (Single)row[29];
            #endregion
            WeakReferenceMessenger.Default.Send<Tuple<DC_Cathode_Object, DC_Cathode_Object>>(
                Tuple.Create<DC_Cathode_Object, DC_Cathode_Object>(cathode_1_param, cathode_4_param)
                );
        }
        private void LoadCathode6Parameters()
        {
            String commandstr = "SELECT Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                        "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                        "Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End,Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End," +
                        "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_Lo_Current_Start,Cath6_3_Lo_Current_End,Cath6_Freq,Cath6_RRT"
                        + " From coating" + " Where [Name]='" + currentRecipe.Rev_name.Trim() + "' And [Step_Name]='" + currentStep.Step_name.Trim() + "';";
            OdbcConnection connection = new OdbcConnection("DSN=coating;");
            OdbcCommand command = new OdbcCommand(commandstr, connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
            DataTable param_table = new DataTable();
            connection.Open();
            dataAdapter.Fill(param_table);
            connection.Close();
            if (param_table.Rows.Count < 1)
            {
                MessageBox.Show("Warning: No parameter in database;", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRow row = param_table.Rows[0];

            if (string.IsNullOrEmpty(row[0].ToString()))
            {
                return;
            }
            var cathode_6_param = new DC_Pls_Cathode_Object() { module_name = "cathode_6" };
            cathode_6_param.operation = (int)row[0];
            cathode_6_param.control = (int)row[1];
            cathode_6_param.shutter = (int)row[2];
            cathode_6_param.gas_valve = (int)row[3];
            cathode_6_param.t_delay = (int)row[4];
            cathode_6_param.t_ramp = (int)row[5];
            cathode_6_param.arc_1_start_high = (int)row[6];
            cathode_6_param.arc_1_end_high = (int)row[7];
            cathode_6_param.arc_1_start_low = (int)row[8];
            cathode_6_param.arc_1_end_low = (int)row[9];
            cathode_6_param.arc_2_start_high = (int)row[10];
            cathode_6_param.arc_2_end_high = (int)row[11];
            cathode_6_param.arc_2_start_low = (int)row[12];
            cathode_6_param.arc_2_end_low = (int)row[13];
            cathode_6_param.arc_3_start_high = (int)row[14];
            cathode_6_param.arc_3_end_high = (int)row[15];
            cathode_6_param.arc_3_start_low = (int)row[16];
            cathode_6_param.arc_3_end_low = (int)row[17];
            cathode_6_param.frequency = (int)row[18];
            cathode_6_param.rrt = (int)row[19];
            WeakReferenceMessenger.Default.Send<DC_Pls_Cathode_Object>(cathode_6_param);
        }
        private void LoadStepValue()
        {
            Changed_parameters.Clear();
            
            switch (currentStep.Step_type)
            {
                case 1:
                    LoadHeatingStepValue();
                    break;
                case 2:
                    LoadGlowDischargeStepValue();
                    LoadGasControlParamters("glow");
                    LoadGasChannelParameters("glow");
                    break;
                case 3:
                    break;
                case 4:
                    LoadCathodeParameters();
                    LoadGasControlParamters("coating");
                    LoadGasChannelParameters("coating");
                    LoadCathode1_4_Parameter();
                    LoadCathode6Parameters();
                    break;
                default:
                    break;
            }
            
            Changed_parameters.Clear();
            WeakReferenceMessenger.Default.Send<string, string>("0","SaveEnableControl");
        }
        
        #endregion

        #region message
        public void Receive(RecipeObject recipe)
        {
            //配方的选择变更
            currentRecipe = recipe;
        }

        public void Receive(StepObject step)
        {
            //配方的选择变更
            currentStep = step;
            LoadStepValue();
        }

        public void Receive(Process_SequenceObject process)
        {
            //配方流程步骤选择变更
            currentProcess = process;

        }
        #endregion
    }
}
