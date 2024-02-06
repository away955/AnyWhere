using System.Globalization;

namespace Away.Wind.Converters;

public class XrayNodeStatusConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            XrayNodeStatus.Error => "AirplaneAlert",
            XrayNodeStatus.Default => "AirplaneSearch",
            XrayNodeStatus.Success => "AirplaneCheck",
            _ => string.Empty
        }; ;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
