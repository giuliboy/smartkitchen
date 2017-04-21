using System;
using System.Globalization;
using System.Windows.Data;
using HSR.CloudSolutions.SmartKitchen.Devices;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Converter
{
    public class DoorStateToSymbolConverter : IValueConverter
    {
        private const string OpenSymbol = "\ue1f7";
        private const string ClosedSymbol = "\ue1f6";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as DoorState? ?? DoorState.Closed;
            return state == DoorState.Open ? OpenSymbol : ClosedSymbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
