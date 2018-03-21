namespace MatrixClient
{
    using MatrixClient.Controls;
    using MatrixClient.ViewModel;    

    using Microsoft.Extensions.DependencyInjection;
    
    using System;    
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public Account Account { get; }
        public MyViewModel MyViewModel { get; }
        public Commands Commands { get; }

        public LoginPage()
        {
            this.Account = ServiceLocator.Current.GetService<Account>();
            this.Commands =  ServiceLocator.Current.GetService<Commands>();
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();

            InitializeComponent();

            Observable
              .FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                      reh => this.txtPassword.PasswordChanged += reh,
                      reh => this.txtPassword.PasswordChanged -= reh)
               .Subscribe(reh => {
                   //Cast the 'sender' to a PasswordBox
                   PasswordBox pBox = reh.Sender as PasswordBox;
                   //Set this "EncryptedPassword" dependency property to the "SecurePassword"
                   //of the PasswordBox.
                   PasswordBoxAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
               });                   
        }     
    }
}
