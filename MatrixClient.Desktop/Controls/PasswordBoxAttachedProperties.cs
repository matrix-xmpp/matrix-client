namespace MatrixClient.Controls
{
    using System.Security;
    using System.Windows;

    // from: https://arlvin.wordpress.com/2013/12/19/the-passwordbox-mvvm-dilemma-part-1/
    public static class PasswordBoxAttachedProperties
    {
        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for EncryptedPassword.
        public static readonly DependencyProperty EncryptedPasswordProperty 
            = DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxAttachedProperties));
    }
}
