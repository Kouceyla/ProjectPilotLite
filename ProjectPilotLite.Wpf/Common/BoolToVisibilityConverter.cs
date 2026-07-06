using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectPilotLite.Wpf.Common
{
    /// <summary>
    /// Convertisseur bool -> Visibility (ex: indicateur de chargement). Si un convertisseur
    /// équivalent existe déjà dans le shell WPF partagé (Ceredine), réutiliser celui-ci et
    /// supprimer ce fichier pour éviter les doublons.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is Visibility.Visible;
    }
}
