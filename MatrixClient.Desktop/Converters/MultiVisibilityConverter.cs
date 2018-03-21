namespace MatrixClient.Converters
{
    using System;
    using System.Windows.Data;

    public class MultiVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values,
                                Type targetType,
                                object parameter,
                                System.Globalization.CultureInfo culture)
        {
            bool visible = true;
            foreach (object value in values)
            {
                var val = ((System.Windows.Visibility)value) == System.Windows.Visibility.Visible ? true : false;
                visible = visible && val;
            }
            if (visible)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value,
                                    Type[] targetTypes,
                                    object parameter,
                                    System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}