namespace MatrixClient
{
    using System.Windows.Controls;
    using MatrixClient.ViewModel;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Interaction logic for AddContactPage.xaml
    /// </summary>
    public partial class AddContactPage : UserControl
    {
        public MyViewModel MyViewModel { get; }
        public AddUser AddUser { get; }
        public RosterCommands RosterCommands { get; }

        public AddContactPage()
        {
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();
            this.AddUser = ServiceLocator.Current.GetService<AddUser>();
            this.RosterCommands = ServiceLocator.Current.GetService<RosterCommands>();

            InitializeComponent();
        }
    }
}
