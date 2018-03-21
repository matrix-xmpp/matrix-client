namespace MatrixClient
{
    using System.Reactive.Linq;
    using System.Windows.Input;
    using MatrixClient.ViewModel;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Windows;
    using MatrixClient.Xmpp;
    using System.Windows.Data;
    using System.Windows.Controls;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {       
        public Account Account { get; }
        public MyViewModel MyViewModel { get; }
        

        public MainWindow()
        {
            this.Account = ServiceLocator.Current.GetService<Account>();
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();
            

            var xmppSession = ServiceLocator.Current.GetService<XmppSession>();
                        
            InitializeComponent();           

            Observable
               .FromEventPattern<KeyEventHandler, KeyEventArgs>(
                       ke => this.KeyDown += ke,
                       ke => this.KeyDown -= ke)
                .Where(kea => !WindowHelper.IsWindowOpen<DebugWindow>() && kea.EventArgs.Key == Key.F12)
                .Subscribe(kea => new DebugWindow().Show());
         

            Observable
             .FromEventPattern<SelectionChangedEventHandler, SelectionChangedEventArgs>(
                     sc => listNav.SelectionChanged+= sc,
                     sc => listNav.SelectionChanged-= sc)
              .Where(sc => listNav.SelectedItem is ChatItemBase)   
              .Subscribe(sc =>
              {
                  MyViewModel.SelectedChat = listNav.SelectedItem as ChatItemBase;
              });

            Observable
                .FromEventPattern<CancelEventHandler, CancelEventArgs>(
                        cea => this.Closing += cea,
                        cea => this.Closing -= cea)
                .Subscribe(async cea =>
                {
                    await xmppSession.Disconnect();    
                    if (WindowHelper.IsWindowOpen<DebugWindow>())
                    {
                        WindowHelper.CloseWindow<DebugWindow>();
                    }
                });       

          
        }      
    }
}
