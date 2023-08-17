using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace NewRecipeViewer.ViewModels
{
    /// <summary>
    /// 比较帮助类
    /// </summary>
    public class CompareHelper
    {
        /// <summary>
        /// 比较--两个类型一样的实体类对象的值
        /// </summary>
        /// <param name="oneT"></param>
        /// <param name="twoT"></param>
        /// <returns></returns>
        public static bool CompareType<T>(T oneT, T twoT)
        {
            bool result = true;//两个类型作比较时使用,如果有不一样的就false
            Type typeOne = oneT.GetType();
            Type typeTwo = twoT.GetType();
            //如果两个T类型不一样  就不作比较
            if (!typeOne.Equals(typeTwo)) { return false; }
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return false;
            }
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length))) { return false; }
            //遍历两个T类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //获取属性名
                string oneName = pisOne[i].Name;
                string twoName = pisTwo[i].Name;
                //获取属性的值
                object oneValue = pisOne[i].GetValue(oneT, null);
                object twoValue = pisTwo[i].GetValue(twoT, null);
                //比较,只比较值类型
                if ((pisOne[i].PropertyType.IsValueType || pisOne[i].PropertyType.Name.StartsWith("String")) && (pisTwo[i].PropertyType.IsValueType || pisTwo[i].PropertyType.Name.StartsWith("String")))
                {
                    if (oneName.Equals(twoName))
                    {
                        if (oneValue == null)
                        {
                            if (twoValue != null)
                            {
                                result = false;
                                break; //如果有不一样的就退出循环
                            }
                        }
                        else if (oneValue != null)
                        {
                            if (twoValue != null)
                            {
                                if (!oneValue.Equals(twoValue))
                                {
                                    result = false;
                                    break; //如果有不一样的就退出循环
                                }
                            }
                            else if (twoValue == null)
                            {
                                result = false;
                                break; //如果有不一样的就退出循环
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    //如果对象中的属性是实体类对象，递归遍历比较
                    bool b = CompareType(oneValue, twoValue);
                    if (!b) { result = b; break; }
                }
            }
            return result;
        }
        /// <summary>
        /// 比较两个对象返回比对元组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oneT"></param>
        /// <param name="twoT"></param>
        /// <returns></returns>
        public static Tuple<ObservableCollection<Tuple<String, String, Single, Single, bool>>,bool> CompareType2<T>(T oneT, T twoT)
        {
            //定义
            ObservableCollection<Tuple<String,String,Single, Single, bool>> compare_result;
            bool result = true;//两个类型作比较时使用,如果有不一样的就false
            Type typeOne = oneT.GetType();
            Type typeTwo = twoT.GetType();
            //如果两个T类型不一样  就不作比较
            if (!typeOne.Equals(typeTwo)) { return null; }
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return null;
            }
            compare_result = new ObservableCollection<Tuple<string,string, float, float, bool>>();
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length))) { return null; }
            //遍历两个T类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //获取属性名
                string oneName = pisOne[i].Name;
                string twoName = pisTwo[i].Name;
                //获取属性的值
                object oneValue = pisOne[i].GetValue(oneT, null);
                object twoValue = pisTwo[i].GetValue(twoT, null);
                //比较,只比较值类型
                if ((pisOne[i].PropertyType.IsValueType || pisOne[i].PropertyType.Name.StartsWith("String")) && (pisTwo[i].PropertyType.IsValueType || pisTwo[i].PropertyType.Name.StartsWith("String")))
                {

                    if (oneName.Equals(twoName))
                    {
                        if (oneValue == null)
                        {
                            if (twoValue != null)
                            {
                                //将对别结果写入队列
                                compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)oneValue, (Single)twoValue, false));
                                result = false;
                            }
                            
                        }
                        else if (oneValue != null)
                        {
                            if (twoValue != null)
                            {
                                if (!oneValue.Equals(twoValue))
                                {

                                    compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)oneValue, (Single)twoValue, false));
                                    result = false;
                                }
                                else
                                {
                                    compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)oneValue, (Single)twoValue, true));
                                }
                            }
                            else if (twoValue == null)
                            {
                                
                                compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)oneValue, (Single)twoValue, false));
                                result = false;

                            }
                        }
                    }
                    else
                    {
                        compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)oneValue, (Single)twoValue, false));
                        result = false;
                    }
                }
                else
                {
                    //如果对象中的属性是实体类对象，递归遍历比较
                    bool b = CompareType(oneValue, twoValue);
                    if (!b)
                    {
                        compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (oneName, twoName, (Single)0, (Single)0, b));
                        result = false;
                    }
                    
                    
                }
            }
            return Tuple.Create<ObservableCollection<Tuple<String, String, Single, Single, bool>>, bool>( compare_result,result);
        }
        public static Tuple<ObservableCollection<Tuple<String, String, Single, Single, bool>>, bool> CompareType3<T>(T oneT, T twoT,String [] list)
        {
            //定义
            ObservableCollection<Tuple<String, String, Single, Single, bool>> compare_result;
            bool result = true;//两个类型作比较时使用,如果有不一样的就false
            Type typeOne = oneT.GetType();
            Type typeTwo = twoT.GetType();
            //如果两个T类型不一样  就不作比较
            if (!typeOne.Equals(typeTwo)) { return null; }
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return null;
            }
            compare_result = new ObservableCollection<Tuple<string, string, float, float, bool>>();
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length))) { return null; }

            //遍历列表进行比对
            foreach (string item in list)
            {
                PropertyInfo property =  typeOne.GetProperty(item);
                object valueOne =  property.GetValue(oneT,null);
                object valueTwo =property.GetValue(twoT,null);
                try
                {
                    if (valueOne.Equals(valueTwo))
                    {
                        compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (item, item, (Single)valueOne, (Single)valueTwo, true));
                    }
                    else
                    {
                        compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (item, item, (Single)valueOne, (Single)valueTwo, false));
                        result = false;
                    }
                }
                catch (Exception)
                {
                    compare_result.Add(Tuple.Create<String, String, Single, Single, bool>
                                    (item, item, (Single)valueOne, (Single)valueTwo, false));
                    result = false;
                    throw;
                }
            }
            //遍历两个T类型，遍历属性，并作比较
            
            return Tuple.Create<ObservableCollection<Tuple<String, String, Single, Single, bool>>, bool>(compare_result, result);
        }

    }
    public class CompareViewModle:ObservableRecipient,IRecipient<RecipeObject>,
        IRecipient<ObservableCollection<StepObject>>, IRecipient<ObservableCollection<Process_SequenceObject>>
    {
        //当前配方
        private RecipeObject current_recipe;
        //当前配方步骤
        private ObservableCollection<StepObject> _steps;
        //当前配方工艺流程
        private ObservableCollection<Process_SequenceObject> _recipe_sequence;

        //数据库配方工艺数据存储字典
        private Dictionary<String, Parameter_T2> stepParameters = new Dictionary<string, Parameter_T2>();
        //plc配方流程数据
        private Dictionary<Tuple<int, int>, Parameter_T2> processParameters;

        //比对结果
        private ObservableCollection<Tuple<int, Process_SequenceObject,ObservableCollection<Tuple<String, String, Single, Single, bool>>, int>> compare_result;
        public ObservableCollection<Tuple<int, Process_SequenceObject, 
            ObservableCollection<Tuple<String, String, Single, Single, bool>>, int>> CompareResult
        {
            get { return compare_result; }
            set
            {
                compare_result = value;
                OnPropertyChanged();
            }
        }
        //对比结果查看表
        private Tuple<int, Process_SequenceObject, 
            ObservableCollection<Tuple<String, String, Single, Single, bool>>, int> select_process;

        public Tuple<int, Process_SequenceObject, 
            ObservableCollection<Tuple<String, String, Single, Single, bool>>, int> Select_process
        {
            get { return select_process; }
            set 
            { 
                select_process = value;
                CompareTable = select_process.Item3;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Tuple<String, String, Single, Single, bool>> compareTable;
        public ObservableCollection<Tuple<String, String, Single, Single, bool>> CompareTable
        {
            get { return compareTable; }
            set
            {
                compareTable = value;
                OnPropertyChanged();
            }
            
        }

        private bool compareStatus;
        public bool CompareStatus
        {
            get { return compareStatus; }
            set
            {
                compareStatus = value;
                OnPropertyChanged();
            }
        }
        //命令
        public ICommand LoadRecipeParameters { get; }
        public ICommand CompareParametersCommand { get; }
        public CompareViewModle()
        {
            this.IsActive = true;
            LoadRecipeParameters = new RelayCommand(()=> 
            {
                LoadStepParametersFromDB(); 
            });
            CompareParametersCommand = new RelayCommand(CompareProcessSequence);
            WeakReferenceMessenger.Default.
                Register<Dictionary<Tuple<int, int>, Parameter_T2>, string>(this, "PlcParameters", ReceivePlcParameters);
            WeakReferenceMessenger.Default.Register<ObservableCollection<Process_SequenceObject>, string>(this,
                "ProcessSequence",ReceiveProcessSequence);
        }

        private void ReceiveProcessSequence(object recipient, ObservableCollection<Process_SequenceObject> message)
        {
            _recipe_sequence = message;
        }

        public void Receive(RecipeObject recipe)
        {
            current_recipe = recipe;
        }

        public void Receive(ObservableCollection<StepObject> steps)
        {
            _steps = steps;
        }

        public void Receive(ObservableCollection<Process_SequenceObject> process_sequence)
        {
            _recipe_sequence = process_sequence;
        }

        private Parameter_T2 GetStepParameterFromDB(StepObject step)
        {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();
            Parameter_T2 parameter_temp = new Parameter_T2();
            switch (step.Step_type)
            {
                case 1://heating 
                    GetHeatingParameter(step, parameter_temp);
                    break;
                case 2://glow discharge
                    GetGlowDischargeParameter(step, parameter_temp);
                    GetGasControlParamters(step, "glow", parameter_temp);
                    GetGasChannelParameters(step, "glow", parameter_temp);

                    break;
                case 3://Ion etching
                    break;
                case 4://coating
                    GetCathodeParameters(step, parameter_temp);
                    GetGasControlParamters(step, "coating", parameter_temp);
                    GetGasChannelParameters(step, "coating", parameter_temp);
                    GetCathode1_4_Parameter(step, parameter_temp);
                    GetCathode6Parameters(step, parameter_temp);
                    break;
                default:
                    break;
            }
            String time1 = swatch.ElapsedMilliseconds.ToString();
            swatch.Stop();
            return parameter_temp;
        }

        private void GetHeatingParameter(StepObject step, Parameter_T2 parameter)
        {
            //加热工艺
            String commandstr = "SELECT id9800,id9801,id9802,id9806,id9808,id9810,id9811,id9814,id9815,id9834,id9922,id9942 "
                        + "From heating" + " Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";
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

            parameter.total_step_time_hour = (Single)row[0];
            parameter.total_step_time_minute = (Single)row[1];
            parameter.total_step_time_second = (Single)row[2];
            parameter.base_pressure = (Single)row[3];
            parameter.base_temperature = (Single)row[4];
            parameter.heating_power = (Single)row[5];
            parameter.heating_temperature = (Single)row[6];
            parameter.pump_speed = (Single)row[7];
            parameter.water_system_mode = (Single)row[8];
            parameter.rotation_speed = (Single)row[9];
            parameter.cath1_shutter = (Single)row[10];
            parameter.cath4_shutter = (Single)row[11];

        }
        private void GetGlowDischargeParameter(StepObject step, Parameter_T2 parameter)
        {
            //Discharge 工艺
            String commandstr = "SELECT id9800,id9801,id9802,id9808,id9810,id9811,id9812,id9813,id9814," +
                        "id9820,id9821,id9822,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                        "id9830,id9831,id9832,id9833,id9834,id9835,id9836,id9837,id9838,id9839,id9840," +
                        "id9922,id9942,id9960 "
                        + "From glow" + " Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";

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

            #region time
            parameter.total_step_time_hour = (Single)row[0];
            parameter.total_step_time_minute = (Single)row[1];
            parameter.total_step_time_second = (Single)row[2];
            #endregion
            #region heating
            parameter.base_temperature = (Single)row[3];
            parameter.heating_power = (Single)row[4];
            parameter.heating_temperature = (Single)row[5];
            parameter.max_temperature = (Single)row[6];
            parameter.min_temperature = (Single)row[7];
            parameter.pump_speed = (Single)row[8];
            #endregion
            #region bias
            parameter.select_voltage_mode = (Single)row[9];
            parameter.Start_voltage = (Single)row[10];
            parameter.Start_current = (Single)row[11];
            parameter.bias_delay_time_hour = (Single)row[12];
            parameter.bias_delay_time_minute = (Single)row[13];
            parameter.bias_delay_time_second = (Single)row[14];
            parameter.bias_ramp_time_hour = (Single)row[15];
            parameter.bias_ramp_time_minute = (Single)row[16];
            parameter.bias_ramp_time_second = (Single)row[17];
            parameter.arc_detection_I = (Single)row[18];


            parameter.plasma_detection_I = (Single)row[19];
            parameter.plasma_detection_U = (Single)row[20];
            parameter.pulse_frequency = (Single)row[21];
            parameter.pulse_rrt = (Single)row[22];
            parameter.rotation_speed = (Single)row[23];
            parameter.bias_control_mode = (Single)row[24];
            parameter.end_voltage = (Single)row[25];
            parameter.end_current = (Single)row[26];
            parameter.U_arc_detection = (Single)row[27];
            parameter.I_arc_detection = (Single)row[28];
            parameter.arc_frequency_limit = (Single)row[29];
            #endregion


            #region cathode
            parameter.cath1_shutter = (Single)row[30];
            parameter.cath4_shutter = (Single)row[31];
            parameter.plasma_current = (Single)row[32];
            #endregion

        }
        private void GetGasControlParamters(StepObject step, String datatable, Parameter_T2 parameter)
        {
            //icon Each 工艺
            String commandstr = "SELECT id9850,id9851,id9852,id9853,id9854,id9855,id9856,id9857,id9859,id9861 " +
                "From " + datatable +
                " Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";
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
            #region Gas Control
            parameter.gas_blend_control = (Single)row[0];
            parameter.slaver_gas = (Single)row[1];
            parameter.master_gas = (Single)row[2];
            parameter.ratio_slaver = (Single)row[3];
            parameter.ratio_master = (Single)row[4];
            parameter.pressure_control = (Single)row[5];
            parameter.pressure_control_gas = (Single)row[6];
            parameter.max_deviation_pressure = (Single)row[7];
            parameter.process_pressure = (Single)row[8];
            parameter.corrrction_flow = (Single)row[9];
            #endregion
        }
        private void GetGasChannelParameters(StepObject step, String datatable, Parameter_T2 parameter)
        {
            //Coating工艺
            String commandstr = "Select " +
                        "id9862,id9863,id9864,id9865,id9866,id9867,id9868,id9869," +
                        "id9872,id9873,id9874,id9875,id9876,id9877,id9878,id9879," +
                        "id9882,id9883,id9884,id9885,id9886,id9887,id9888,id9889," +
                        "id9892,id9893,id9894,id9895,id9896,id9897,id9898,id9899," +
                        "id9902,id9903,id9904,id9905,id9906,id9907,id9908,id9909 " +
                        "From " + datatable +
                        " Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";
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
            parameter.gas_channel_1_start_flow = (Single)row[0];
            parameter.gas_channel_1_delay_hour = (Single)row[1];
            parameter.gas_channel_1_delay_minute = (Single)row[2];
            parameter.gas_channel_1_delay_second = (Single)row[3];
            parameter.gas_channel_1_end_flow = (Single)row[4];
            parameter.gas_channel_1_ramp_hour = (Single)row[5];
            parameter.gas_channel_1_ramp_minute = (Single)row[6];
            parameter.gas_channel_1_ramp_second = (Single)row[7];

            parameter.gas_channel_2_start_flow = (Single)row[8];
            parameter.gas_channel_2_delay_hour = (Single)row[9];
            parameter.gas_channel_2_delay_minute = (Single)row[10];
            parameter.gas_channel_2_delay_second = (Single)row[11];
            parameter.gas_channel_2_end_flow = (Single)row[12];
            parameter.gas_channel_2_ramp_hour = (Single)row[13];
            parameter.gas_channel_2_ramp_minute = (Single)row[14];
            parameter.gas_channel_2_ramp_second = (Single)row[15];

            parameter.gas_channel_3_start_flow = (Single)row[16];
            parameter.gas_channel_3_delay_hour = (Single)row[17];
            parameter.gas_channel_3_delay_minute = (Single)row[18];
            parameter.gas_channel_3_delay_second = (Single)row[19];
            parameter.gas_channel_3_end_flow = (Single)row[20];
            parameter.gas_channel_3_ramp_hour = (Single)row[21];
            parameter.gas_channel_3_ramp_minute = (Single)row[22];
            parameter.gas_channel_3_ramp_second = (Single)row[23];

            parameter.gas_channel_4_start_flow = (Single)row[24];
            parameter.gas_channel_4_delay_hour = (Single)row[25];
            parameter.gas_channel_4_delay_minute = (Single)row[26];
            parameter.gas_channel_4_delay_second = (Single)row[27];
            parameter.gas_channel_4_end_flow = (Single)row[28];
            parameter.gas_channel_4_ramp_hour = (Single)row[29];
            parameter.gas_channel_4_ramp_minute = (Single)row[30];
            parameter.gas_channel_4_ramp_second = (Single)row[31];

            parameter.gas_channel_5_start_flow = (Single)row[32];
            parameter.gas_channel_5_delay_hour = (Single)row[33];
            parameter.gas_channel_5_delay_minute = (Single)row[34];
            parameter.gas_channel_5_delay_second = (Single)row[35];
            parameter.gas_channel_5_end_flow = (Single)row[36];
            parameter.gas_channel_5_ramp_hour = (Single)row[37];
            parameter.gas_channel_5_ramp_minute = (Single)row[38];
            parameter.gas_channel_5_ramp_second = (Single)row[39];
            #endregion
        }
        private void GetCathodeParameters(StepObject step, Parameter_T2 parameter)
        {
            String commandstr = "Select id9800,id9801,id9802,id9803,id9808,id9810,id9811,id9814," +
                        "id9820,id9821,id9823,id9824,id9825,id9826,id9827,id9828,id9829," +
                        "id9832,id9833,id9834,id9836 From coating "
                        + String.Format("Where Name='{0}' And Step_Name='{1}';",
                        current_recipe.Rev_name.Trim(), step.Step_name.Trim());
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
            #region time
            parameter.total_step_time_hour = (Single)row[0];
            parameter.total_step_time_minute = (Single)row[1];
            parameter.total_step_time_second = (Single)row[2];
            parameter.total_ah = (Single)row[3];
            #endregion
            #region heating
            parameter.base_temperature = (Single)row[4];
            parameter.heating_power = (Single)row[5];
            parameter.heating_temperature = (Single)row[6];
            parameter.pump_speed = (Single)row[7];
            #endregion
            #region bias
            parameter.select_voltage_mode = (Single)row[8];
            parameter.Start_voltage = (Single)row[9];

            parameter.bias_delay_time_hour = (Single)row[10];
            parameter.bias_delay_time_minute = (Single)row[11];
            parameter.bias_delay_time_second = (Single)row[12];
            parameter.bias_ramp_time_hour = (Single)row[13];
            parameter.bias_ramp_time_minute = (Single)row[14];
            parameter.bias_ramp_time_second = (Single)row[15];
            parameter.arc_detection_I = (Single)row[16];


            parameter.pulse_frequency = (Single)row[17];
            parameter.pulse_rrt = (Single)row[18];
            parameter.rotation_speed = (Single)row[19];

            parameter.end_voltage = (Single)row[20];
            #endregion


        }
        private void GetCathode1_4_Parameter(StepObject step, Parameter_T2 parameter)
        {
            String commandstr = "Select " +
                        "id9920,id9922,id9923,id9924,id9925,id9926,id9927,id9928,id9929," +
                        "id9930,id9931,id9932,id9933,id9934,id9935," +
                        "id9940,id9942,id9943,id9944,id9945,id9946,id9947,id9948,id9949," +
                        "id9950,id9951,id9952,id9953,id9954,id9955 " +
                        "From coating " + "Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";
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
            parameter.cath1_operation = (Single)row[0];

            parameter.cath1_shutter = (Single)row[1];
            parameter.cath1_delay_hour = (Single)row[2];
            parameter.cath1_delay_minute = (Single)row[3];
            parameter.cath1_delay_second = (Single)row[4];
            parameter.cath1_ramp_hour = (Single)row[5];
            parameter.cath1_ramp_minute = (Single)row[6];
            parameter.cath1_ramp_second = (Single)row[7];
            parameter.cath1_gas_valve = (Single)row[8];
            parameter.cath1_1_arc_start = (Single)row[9];
            parameter.cath1_1_arc_end = (Single)row[10];
            parameter.cath1_2_arc_start = (Single)row[11];
            parameter.cath1_2_arc_end = (Single)row[12];
            parameter.cath1_3_arc_start = (Single)row[13];
            parameter.cath1_3_arc_end = (Single)row[14];
            #endregion
            #region cathode4
            parameter.cath4_operation = (Single)row[15];

            parameter.cath4_shutter = (Single)row[16];
            parameter.cath4_delay_hour = (Single)row[17];
            parameter.cath4_delay_minute = (Single)row[18];
            parameter.cath4_delay_second = (Single)row[19];
            parameter.cath4_ramp_hour = (Single)row[20];
            parameter.cath4_ramp_minute = (Single)row[21];
            parameter.cath4_ramp_second = (Single)row[22];
            parameter.cath4_gas_valve = (Single)row[23];
            parameter.cath4_1_arc_start = (Single)row[24];
            parameter.cath4_1_arc_end = (Single)row[25];
            parameter.cath4_2_arc_start = (Single)row[26];
            parameter.cath4_2_arc_end = (Single)row[27];
            parameter.cath4_3_arc_start = (Single)row[28];
            parameter.cath4_3_arc_end = (Single)row[29];
            #endregion
        }
        private void GetCathode6Parameters(StepObject step, Parameter_T2 parameter)
        {
            String commandstr = "SELECT Cath6_Operation,Cath6_Mode,Cath6_Shuttle,Cath6_GasValve,Cath6_T_Delay,Cath6_T_Ramp," +
                        "Cath6_1_HI_Current_Start,Cath6_1_HI_Current_End,Cath6_1_Lo_Current_Start,Cath6_1_Lo_Current_End," +
                        "Cath6_2_HI_Current_Start,Cath6_2_HI_Current_End,Cath6_2_Lo_Current_Start,Cath6_2_Lo_Current_End," +
                        "Cath6_3_HI_Current_Start,Cath6_3_HI_Current_End,Cath6_3_Lo_Current_Start,Cath6_3_Lo_Current_End,Cath6_Freq,Cath6_RRT"
                        + " From coating" + " Where [Name]='" + current_recipe.Rev_name.Trim() + "' And [Step_Name]='" + step.Step_name.Trim() + "';";
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
            parameter.cath6_operation = (int)row[0];
            parameter.cath6_control = (int)row[1];
            parameter.cath6_shutter = (int)row[2];
            parameter.cath6_gas_valve = (int)row[3];
            parameter.cath6_delay = (int)row[4];
            parameter.cath6_ramp = (int)row[5];
            parameter.cath6_1_arc_high_start = (int)row[6];
            parameter.cath6_1_arc_high_end = (int)row[7];
            parameter.cath6_1_arc_low_start = (int)row[8];
            parameter.cath6_1_arc_low_end = (int)row[9];
            parameter.cath6_2_arc_high_start = (int)row[10];
            parameter.cath6_2_arc_high_end = (int)row[11];
            parameter.cath6_2_arc_low_start = (int)row[12];
            parameter.cath6_2_arc_low_end = (int)row[13];
            parameter.cath6_3_arc_high_start = (int)row[14];
            parameter.cath6_3_arc_high_end = (int)row[15];
            parameter.cath6_3_arc_low_start = (int)row[16];
            parameter.cath6_3_arc_low_end = (int)row[17];
            parameter.cath6_frequency = (int)row[18];
            parameter.cath6_rrt = (int)row[19];
        }

        private void LoadStepParametersFromDB()
        {
            stepParameters.Clear();
            if (_steps is null)
            {
                return;
            }
            foreach (StepObject step in _steps)
            {
                Parameter_T2 temp = new Parameter_T2();
                temp = GetStepParameterFromDB(step);
                try
                {
                    stepParameters.Add(step.Step_name.Trim(), temp);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("数据库中同一个配方,有多个名称为:{0}  的工艺步", step.Step_name.Trim()),
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                }
            }
        }

        

        private void ReceivePlcParameters(object recipient, Dictionary<Tuple<int, int>, Parameter_T2> parameters)
        {

            processParameters = parameters;
            //终端plc参数接收
        }

        private void CompareProcessSequence()
        {
            //创建对象
            CompareStatus = true;
            CompareResult = new ObservableCollection<
                Tuple<int, Process_SequenceObject,
                ObservableCollection<Tuple<String, String, Single, Single, bool>>,
                int>>();
            foreach (Process_SequenceObject item in _recipe_sequence)
            {

                var key = Tuple.Create(item.Seq_no, item.Step_type);
                var db_parameter = stepParameters[item.Step_name.Trim()];
                int compare_result_temp = 0;
                ObservableCollection<Tuple<String, String, Single, Single, bool>> temptable;
                try
                {
                    var plc_parameter = processParameters[key];
                    //队列判断通过,对比工艺参数
                    string[] list;
                    switch (key.Item2)
                    {
                        case 1:

                            string[] heatinglist = {"total_step_time_hour", "total_step_time_minute", "total_step_time_second",
                                "base_pressure", "base_temperature", "heating_power", "heating_temperature", "pump_speed",
                                "water_system_mode", "rotation_speed", "cath1_shutter", "cath4_shutter" };
                            list = heatinglist;
                            break;
                        case 2:
                            string[] glowlist = {"total_step_time_hour", "total_step_time_minute", "total_step_time_second",
                                "base_temperature", "heating_power", "heating_temperature", "max_temperature", "min_temperature",
                                "pump_speed", "select_voltage_mode", "Start_voltage", "Start_current",
                                "bias_delay_time_hour","bias_delay_time_minute","bias_delay_time_second","bias_ramp_time_hour",
                                "bias_ramp_time_minute","bias_ramp_time_second","arc_detection_I","plasma_detection_I",
                                "plasma_detection_U","pulse_frequency","pulse_rrt","rotation_speed","bias_control_mode",
                                "end_voltage","end_current","U_arc_detection","I_arc_detection","arc_frequency_limit",
                                "gas_blend_control","slaver_gas","master_gas","ratio_slaver","ratio_master","ratio_master","pressure_control",
                                "pressure_control_gas","max_deviation_pressure","process_pressure","corrrction_flow",
                                "gas_channel_1_start_flow","gas_channel_1_delay_hour","gas_channel_1_delay_minute","gas_channel_1_delay_second",
                                "gas_channel_1_end_flow","gas_channel_1_ramp_hour","gas_channel_1_ramp_minute","gas_channel_1_ramp_second",
                                "gas_channel_2_start_flow","gas_channel_2_delay_hour","gas_channel_2_delay_minute","gas_channel_2_delay_second",
                                "gas_channel_2_end_flow","gas_channel_2_ramp_hour","gas_channel_2_ramp_minute","gas_channel_2_ramp_second",
                                "gas_channel_3_start_flow","gas_channel_3_delay_hour","gas_channel_3_delay_minute","gas_channel_3_delay_second",
                                "gas_channel_3_end_flow","gas_channel_3_ramp_hour","gas_channel_3_ramp_minute","gas_channel_3_ramp_second",
                                "gas_channel_4_start_flow","gas_channel_4_delay_hour","gas_channel_4_delay_minute","gas_channel_4_delay_second",
                                "gas_channel_4_end_flow","gas_channel_4_ramp_hour","gas_channel_4_ramp_minute","gas_channel_4_ramp_second",
                                "gas_channel_5_start_flow","gas_channel_5_delay_hour","gas_channel_5_delay_minute","gas_channel_5_delay_second",
                                "gas_channel_5_end_flow","gas_channel_5_ramp_hour","gas_channel_5_ramp_minute","gas_channel_5_ramp_second",
                                "cath1_shutter","cath4_shutter","plasma_current"
                            };
                            list = glowlist;
                            break;
                        case 3:
                            string[] ionlist = {"total_step_time_hour", "total_step_time_minute", "total_step_time_second","total_ah",
                                "cathode_on_time","cathode_off_time",
                                "base_temperature","minimum_ampMinutes_CARC",
                                "heating_power", "heating_temperature", "max_temperature", "min_temperature",
                                "pump_speed", "select_voltage_mode", "Start_voltage",
                                "pulse_frequency","pulse_rrt","rotation_speed",
                                "gas_channel_2_start_flow","gas_channel_2_delay_hour","gas_channel_2_delay_minute","gas_channel_2_delay_second",
                                "gas_channel_2_end_flow","gas_channel_2_ramp_hour","gas_channel_2_ramp_minute","gas_channel_2_ramp_second",
                                "cath1_operation","cath1_control","cath1_shutter","cath1_delay_hour","cath1_delay_minute",
                                "cath1_delay_second","cath1_ramp_hour","cath1_ramp_minute","cath1_ramp_second",
                                "cath1_gas_valve","cath1_1_arc_start","cath1_1_arc_end","cath1_2_arc_start",
                                "cath1_2_arc_end","cath1_3_arc_start","cath1_3_arc_end",
                                "cath4_operation","cath4_control",
                                "cath4_shutter","cath4_delay_hour","cath4_delay_minute","cath4_delay_second",
                                "cath4_ramp_hour","cath4_ramp_minute","cath4_ramp_second","cath4_gas_valve",
                                "cath4_1_arc_start","cath4_1_arc_end","cath4_2_arc_start","cath4_2_arc_end",
                                "cath4_3_arc_start","cath4_3_arc_end","plasma_current"
                            };
                            list = ionlist;
                            break;
                        case 4:
                            string[] coatinglist = {
                            "total_step_time_hour","total_step_time_minute","total_step_time_second","total_ah",
                            "base_temperature","heating_power", "heating_temperature","pump_speed",
                            "select_voltage_mode", "Start_voltage",
                            "bias_delay_time_hour","bias_delay_time_minute","bias_delay_time_second","bias_ramp_time_hour",
                             "bias_ramp_time_minute","bias_ramp_time_second","arc_detection_I",
                             "pulse_frequency","pulse_rrt","rotation_speed","end_voltage",
                             "gas_blend_control","slaver_gas","master_gas","ratio_slaver","ratio_master","ratio_master","pressure_control",
                            "pressure_control_gas","max_deviation_pressure","process_pressure","corrrction_flow",
                            "gas_channel_1_start_flow","gas_channel_1_delay_hour","gas_channel_1_delay_minute","gas_channel_1_delay_second",
                            "gas_channel_1_end_flow","gas_channel_1_ramp_hour","gas_channel_1_ramp_minute","gas_channel_1_ramp_second",
                            "gas_channel_2_start_flow","gas_channel_2_delay_hour","gas_channel_2_delay_minute","gas_channel_2_delay_second",
                            "gas_channel_2_end_flow","gas_channel_2_ramp_hour","gas_channel_2_ramp_minute","gas_channel_2_ramp_second",
                            "gas_channel_3_start_flow","gas_channel_3_delay_hour","gas_channel_3_delay_minute","gas_channel_3_delay_second",
                            "gas_channel_3_end_flow","gas_channel_3_ramp_hour","gas_channel_3_ramp_minute","gas_channel_3_ramp_second",
                            "gas_channel_4_start_flow","gas_channel_4_delay_hour","gas_channel_4_delay_minute","gas_channel_4_delay_second",
                            "gas_channel_4_end_flow","gas_channel_4_ramp_hour","gas_channel_4_ramp_minute","gas_channel_4_ramp_second",
                            "gas_channel_5_start_flow","gas_channel_5_delay_hour","gas_channel_5_delay_minute","gas_channel_5_delay_second",
                            "gas_channel_5_end_flow","gas_channel_5_ramp_hour","gas_channel_5_ramp_minute","gas_channel_5_ramp_second",
                            "cath1_operation","cath1_control","cath1_shutter","cath1_delay_hour",
                            "cath1_delay_minute","cath1_delay_second","cath1_ramp_hour","cath1_ramp_minute",
                            "cath1_ramp_second","cath1_gas_valve","cath1_1_arc_start","cath1_1_arc_end","cath1_2_arc_start",
                            "cath1_2_arc_end","cath1_3_arc_start","cath1_3_arc_end",
                            "cath4_operation","cath4_control","cath4_shutter","cath4_delay_hour","cath4_delay_minute",
                            "cath4_delay_second","cath4_ramp_hour","cath4_ramp_minute","cath4_ramp_second",
                            "cath4_gas_valve","cath4_1_arc_start","cath4_1_arc_end","cath4_2_arc_start",
                            "cath4_2_arc_end","cath4_3_arc_start","cath4_3_arc_end",
                            "cath6_operation","cath6_control","cath6_shutter","cath6_delay","cath6_ramp",
                            "cath6_gas_valve","cath6_1_arc_high_start","cath6_1_arc_high_end","cath6_2_arc_high_start",
                            "cath6_2_arc_high_end","cath6_3_arc_high_start","cath6_3_arc_high_end","cath6_1_arc_low_start",
                            "cath6_1_arc_low_end","cath6_2_arc_low_start","cath6_2_arc_low_end","cath6_3_arc_low_start",
                            "cath6_3_arc_low_end","cath6_frequency","cath6_rrt"
                             };
                            list = coatinglist;
                            break;
                        default:
                            string[] errorlist = { };
                            list = errorlist;
                            break;
                    }
                    //对两个配方数据进行比较操作
                    //var ret = CompareHelper.CompareType2<Parameter_T2>(db_parameter,plc_parameter);
                    var ret = CompareHelper.CompareType3<Parameter_T2>(db_parameter, plc_parameter, list);
                    if (ret.Item2)
                    {
                        //当前流程步骤效验通过
                        compare_result_temp = 1;
                    }
                    else
                    {
                        //当前流程步骤效验异常
                        compare_result_temp = -1;
                    }
                    //比对表
                    temptable = ret.Item1;
                }

                catch (Exception)
                {
                    compare_result_temp = -1;
                    temptable = null;
                    break;
                    throw;
                }
                var result = Tuple.Create(item.Seq_no, item, temptable, compare_result_temp);
                CompareResult.Add(result);
                if (compare_result_temp == -1)
                {
                    CompareStatus = false;
                }
            }
        }
    }
}

