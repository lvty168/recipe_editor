using Microsoft.Xaml.Behaviors;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NewRecipeViewer
{
    public class FloatToTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {

                float hour = (float)value / 3600;
                float temp = (float)value % 3600;
                float minute = temp / 60;
                float second = temp % 60;
                //"{0,3:D2}:{1,3:D2}:{2,3:D2}" {第几个参数,长度:字符位数控制}
                return string.Format("{0,3:D2}:{1,3:D2}:{2,3:D2}", (int)hour, (int)minute, (int)second);
            }
            else
            {
                return string.Format("{0,3:D2}:{1,3:D2}:{2,3:D2}", 0, 0, 0);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = (string)value;
            var temp2 = temp.Split(":");
            if (temp2.Length == 3)
            {
                return int.Parse(temp2[0]) * 3600 + int.Parse(temp2[1])*60 + int.Parse(temp2[2]);
            }
            else
            {
                return 0;
            }
        }
    }

    public class FloatToScientific : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {

                return ((float)value).ToString("E02");

            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {

                return float.Parse((String)value);

            }
            else
            {
                throw new NotImplementedException();
            }

        }
    }

    public class FloatToValveStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((float)value == 1)
                {
                    return "Open";
                }
                else
                {
                    return "Closed";
                }
            }
            else
            {
                return "Error";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FloatToSwitchStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((float)value == 1)
                {
                    return "ON";
                }
                else
                {
                    return "OFF";
                }
            }
            else
            {
                return "Error";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StepTypeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (int.Parse(value.ToString()))
                {
                    case 1:
                        return "Heating";
                        
                    case 2:
                        return "Glow discharge";
                    case 3:
                        return "Ion etching";
                    case 4:
                        return "Coating";
                    default:
                        break;
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GreenLampConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return System.Windows.Media.Brushes.DarkRed;
            }
            if ((bool)value)
            {
                return System.Windows.Media.Brushes.Green;
            }
            return System.Windows.Media.Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class User_Convert
    {
        public static String ConvertIdReleaseToString(int Idrelease)
        {
            switch (Idrelease)
            {
                case 1:
                    return "Operator";
                case 2:
                    return "Supervisor";
                case 3:
                    return "Technician";
                case 4:
                    return "Developer";
                case 5:
                    return "Administrator";
                case 6:
                    return "Design Mode";
                default:
                    return "Operator";
            }
        }
    }

    public class StringTrim : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TextBoxEnterKeyUpdateBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject != null)
            {
                base.OnAttached();
                this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (e.Key == Key.Return)
                {
                    if (e.Key == Key.Enter)
                    {
                        textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        
                    }
                }
            }
        }
    }
}  