namespace MatrixClient.Converters
{   
    using System;
    using System.Windows.Data;

    public class VisibilityInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return InvertVisibility((System.Windows.Visibility)value);            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return InvertVisibility((System.Windows.Visibility)value);
        }

        private System.Windows.Visibility InvertVisibility(System.Windows.Visibility visibility)
        {
            if (visibility == System.Windows.Visibility.Collapsed)
            {
                return System.Windows.Visibility.Visible;
            }
            else if (visibility == System.Windows.Visibility.Hidden)
            {
                return System.Windows.Visibility.Visible;
            }

            return System.Windows.Visibility.Collapsed;
        }
    }
}
