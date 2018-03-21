namespace MatrixClient
{
    using MatrixClient.ViewModel;
    using System.Windows.Controls;

    using Microsoft.Extensions.DependencyInjection;
    using ReactiveUI;
    using System.Windows;

    /// <summary>
    /// Interaction logic for AccountMenu.xaml
    /// </summary>
    public partial class AccountMenu : UserControl
    {
        public Account Account { get; }
        public Commands Commands { get; }
        public MyViewModel MyViewModel { get; }

        public ReactiveCommand ExitCommand { get; }
        public ReactiveCommand AboutCommand { get; }
        public ReactiveCommand DebugWindowCommand { get; }

        public AccountMenu()
        {
            this.Account = ServiceLocator.Current.GetService<Account>();
            this.Commands = ServiceLocator.Current.GetService<Commands>();
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();

            ExitCommand = ReactiveCommand.Create(
                () => Application.Current.MainWindow.Close()
            );

            AboutCommand = ReactiveCommand.Create(() =>
            {
                if (!WindowHelper.IsWindowOpen<About>())
                {
                    new About()
                    {
                        Owner = Application.Current.MainWindow
                    }
                    .Show();                
                }
            });

            DebugWindowCommand = ReactiveCommand.Create(() =>
            {
                if (!WindowHelper.IsWindowOpen<DebugWindow>())
                {
                    new DebugWindow().Show();
                }
            });

            InitializeComponent();
        }
    }
}
