using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectPilotLite.Wpf.Common
{
    /// <summary>Visible si la valeur n'est pas null (et, pour une string, non vide).</summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var estPresent = value switch
            {
                null => false,
                string s => !string.IsNullOrWhiteSpace(s),
                _ => true
            };
            return estPresent ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
