using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace NewRecipeViewer
{
    public class Copy
    {
        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj == null || (obj is string) || (obj.GetType().IsValueType)) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
        
        
    }
    public class CustomerConverts
    {
        public static int ConvertIdRelease(String str)
        {
            switch (str)
            {
                case "Operator":
                    return 1;
                case "Supervisor":
                    return 2;
                case "Technician":
                    return 3;
                case "Developer":
                    return 4;
                case "Administrator":
                    return 5;
                case "Design Mode":
                    return 6;
                default:
                    return 0;
            }
        }

        public static Tuple<float,float,float> ConvertTimeValue(float secondtime)
        {
            float hour = (int)(secondtime / 3600);
            float minute = (int)((secondtime - hour*3600) / 60);
            float second = secondtime - hour*3600 - minute*60;
            return Tuple.Create<float, float, float>((int)hour, (int)minute, (int)second);
        }

        public static string ConvertDictionaryToUpdateString(string database, Dictionary<string, float> parameters, string recipe_name,string step_name)
        {
            string set_string;
            List<string> temp = new List<string>();
            foreach (var item in parameters)
            {
                temp.Add(item.Key + "=" + item.Value.ToString());
            }
            set_string =string.Join(",", temp);
            string command_str = string.Format("UPDATE {0}", database)+
                " SET "+ set_string+ 
                string.Format(" WHERE Name='{0}' AND Step_Name='{1}'", recipe_name, step_name);
            return command_str;
        }
        
    }
}