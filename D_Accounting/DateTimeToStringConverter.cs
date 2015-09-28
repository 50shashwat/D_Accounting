using System;
using System.Windows.Data;

namespace D_Accounting
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((DateTime)value).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string[] s = value.ToString().Split('/');
                return new DateTime(Int32.Parse(s[2]), Int32.Parse(s[1]), Int32.Parse(s[0]));
            }
            catch (Exception e)
            {
                return new System.Windows.Controls.ValidationResult(false, e);
            }
        }
    }
}
