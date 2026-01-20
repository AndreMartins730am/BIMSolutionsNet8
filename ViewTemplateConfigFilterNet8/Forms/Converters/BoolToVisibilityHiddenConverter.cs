using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ViewTemplateConfigFilterNet8.Forms.Converters
{
    /// <summary>
    /// Converts a <see cref="bool"/> to <see cref="Visibility"/> mapping <c>true</c> → <see cref="Visibility.Visible"/>
    /// and <c>false</c> → <see cref="Visibility.Hidden"/>. The reverse conversion maps
    /// <see cref="Visibility.Visible"/> → <c>true</c> and any other value → <c>false</c>.
    /// </summary>
    public class BoolToVisibilityHiddenConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The source value expected to be a <see cref="bool"/>.</param>
        /// <param name="targetType">The target type (ignored; expected to be <see cref="Visibility"/>).</param>
        /// <param name="parameter">Optional converter parameter (unused).</param>
        /// <param name="culture">The culture to use in the converter (unused).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> when <paramref name="value"/> is <c>true</c>;
        /// otherwise <see cref="Visibility.Hidden"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isTrue = value is bool b && b;
            return isTrue ? Visibility.Visible : Visibility.Hidden;//false => Hidden            
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The source value expected to be a <see cref="Visibility"/>.</param>
        /// <param name="targetType">The target type (ignored; expected to be <see cref="bool"/>).</param>
        /// <param name="parameter">Optional converter parameter (unused).</param>
        /// <param name="culture">The culture to use in the converter (unused).</param>
        /// <returns>
        /// <c>true</c> when <paramref name="value"/> is <see cref="Visibility.Visible"/>;
        /// otherwise <c>false</c>.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}
