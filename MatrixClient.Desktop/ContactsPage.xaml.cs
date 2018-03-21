namespace MatrixClient
{
    using System;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using MatrixClient.ViewModel;
    using MatrixClient.Xmpp;
    using Microsoft.Extensions.DependencyInjection;
    using ReactiveUI;

    /// <summary>
    /// Interaction logic for Contacts.xaml
    /// </summary>
    public partial class ContactsPage : UserControl
    {
        public RosterCommands RosterCommands { get; }
        public Contacts Contacts { get; }
        public MyViewModel MyViewModel { get; }

        public ContactsPage()
        {
            this.RosterCommands = ServiceLocator.Current.GetService<RosterCommands>();
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();
            this.Contacts = ServiceLocator.Current.GetService<Contacts>();

           // DataContext = this;

            InitializeComponent();            

            Observable
                .FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                        tc => txtSearch.TextChanged += tc,
                        tc => txtSearch.TextChanged -= tc)
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.MainThreadScheduler)                
                .Subscribe(tc =>
                 {
                     listContacts.ItemsSource =
                         Contacts
                             .Where(c => c.Name.ToUpper().Contains(txtSearch.Text.ToUpper()) || c.Jid.ToUpper().Contains(txtSearch.Text.ToUpper()));
                 });        


            Observable
             .FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(
                     mbh => listContacts.MouseDoubleClick += mbh,
                     mbh => listContacts.MouseDoubleClick -= mbh)

              .Subscribe((mbh) =>
              {
                  var item = ItemsControl.ContainerFromElement(listContacts, mbh.EventArgs.OriginalSource as DependencyObject) as ListBoxItem;
                  if (item != null)
                  {
                      var contact = item.DataContext as Contact;
                      var chats = MyViewModel.Chats;
                      if (!chats.Any(c => c.Jid == contact.Jid))
                      {
                          // conversation does not exist => add
                          var newConversation = ChatItem.CreateFromContact(contact);
                          MyViewModel.MenuItems.Add(newConversation);
                          MyViewModel.SelectedChat = newConversation;
                      }
                      else
                      {
                          // conversation exist => select it                          
                          MyViewModel.SelectedChat = MyViewModel.Chats.FirstOrDefault(c => c.Jid == contact.Jid);
                      }
                      MyViewModel.ChatsVisible = true;
                  }
              });
        }

      
    }
}
