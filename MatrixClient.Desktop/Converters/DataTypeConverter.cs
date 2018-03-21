namespace MatrixClient.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DataTypeConverter : IValueConverter
    {
        /*
            Change your DataTrigger to use the Converter, and set the value to the Type:

            <DataTrigger Binding="{Binding SelectedItem,  
                  Converter={StaticResource DataTypeConverter}}" 
                  Value="{x:Type local:MyType}">
            ...
            </DataTrigger>
            Declare DataTypeConverter in the resources:

            <UserControl.Resources>
                <v:DataTypeConverter x:Key="DataTypeConverter"></v:DataTypeConverter>
            </UserControl.Resources>
        */
        public object Convert(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            return value.GetType();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
