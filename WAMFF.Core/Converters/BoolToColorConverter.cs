using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace WAMFF.Core.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is bool isSelected) {
            return isSelected ? new SolidColorBrush(Colors.LightBlue) : new SolidColorBrush(Colors.LightGray);
        }
        return new SolidColorBrush(Colors.LightGray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}