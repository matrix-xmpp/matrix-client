namespace MatrixClient
{
    using MatrixClient.ViewModel;
    using Microsoft.Extensions.DependencyInjection;
    using ReactiveUI;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ActionBar.xaml
    /// </summary>
    public partial class ActionBar : UserControl
    {
        public SubscriptionRequests SubscriptionRequests { get; }
        public SubscriptionCommands SubscriptionCommands { get; }
       
        public ActionBar()
        {
            this.SubscriptionRequests = ServiceLocator.Current.GetService<SubscriptionRequests>();
            this.SubscriptionCommands = ServiceLocator.Current.GetService<SubscriptionCommands>();

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionRequests.RemoveAt(0);
        }
    }
}
