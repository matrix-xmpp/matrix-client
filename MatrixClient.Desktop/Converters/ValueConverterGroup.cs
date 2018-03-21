namespace MatrixClient.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    
    /*
    <c:ValueConverterGroup x:Key="InvertAndVisibilitate">
       <c:BooleanInverterConverter/>
       <c:BooleanToVisibilityConverter/>
    </c:ValueConverterGroup>
    */

    /// <summary>
    /// Converter which allow chaining multiple converters together and pipe through the values
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
