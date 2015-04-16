using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace D_Accounting
{
    internal class FileInfoToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value as FileInfo).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return new FileInfo(value.ToString());
            }
            catch (ArgumentException e)
            {
                return new System.Windows.Controls.ValidationResult(false, e);
            }
        }
    }
}
