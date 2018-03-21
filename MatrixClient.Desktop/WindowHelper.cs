using System.Linq;
using System.Windows;

namespace MatrixClient
{
    public class WindowHelper
    {
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        public static void CloseWindow<T>() where T : Window
        {
            if (Application.Current.Windows.OfType<T>().Any())
            {
                Application.Current.Windows.OfType<T>().First().Close();
            }               
        }
    }
}
