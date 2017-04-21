using System;
using System.Globalization;
using System.Windows.Data;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Converter
{
    public class DoubleToTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double degrees;
            if (value == null || !double.TryParse(value.ToString(), out degrees))
            {
                degrees = 0;
            }
            return $"{degrees:0.0} °C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
