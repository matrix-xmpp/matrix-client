namespace MatrixClient.Converters
{
    using System;
    using System.Windows.Data;

    public class IntegerToStringConverter : IValueConverter
    {
        public int EmptyStringValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
                return value;
            else if (value is int && (int)value == EmptyStringValue)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                string s = (string)value;
                if (s.IsNumeric())
                    return System.Convert.ToInt32(s);
                else
                    return EmptyStringValue;
            }
            return value;
        }
    }
}
