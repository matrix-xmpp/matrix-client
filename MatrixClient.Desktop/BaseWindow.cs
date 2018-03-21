namespace MatrixClient
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Base window class with the following features:
    /// 
    /// * Disable tablet context menu alignment to the left when right handed
    /// </summary>
    public class BaseWindow : Window
    {
        private static readonly FieldInfo menuDropAlignmentField;

        static BaseWindow()
        {
            menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);            

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EnsureStandardPopupAlignment();
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && menuDropAlignmentField != null)
            {
                menuDropAlignmentField.SetValue(null, false);
            }
        }
    }
}
