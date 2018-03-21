namespace MatrixClient
{
    using System;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;
    using MatrixClient.ViewModel;
    using System.Windows.Media;
    using System.Windows;
    using System.Reactive.Linq;
    using System.Collections.Specialized;

    /// <summary>
    /// Interaction logic for ChatPanel.xaml
    /// </summary>
    public partial class ChatPanel : UserControl
    {
        MyViewModel MyViewModel;
        ScrollViewer scrollViewerMessages;

        public ChatPanel()
        {
            this.MyViewModel = ServiceLocator.Current.GetService<MyViewModel>();

           
            Observable
              .FromEventPattern<DependencyPropertyChangedEventHandler, DependencyPropertyChangedEventArgs>(
                      dcc => this.DataContextChanged += dcc,
                      dcc => this.DataContextChanged -= dcc)
               
               .Subscribe(dcc => 
               {
                   var chatItem = dcc.EventArgs.NewValue as ChatItem;
                   
                   Observable
                       .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                               cc => chatItem.Messages.CollectionChanged += cc,
                               cc => chatItem.Messages.CollectionChanged -= cc)
                               
                         .Subscribe(cc =>
                         {
                             if (MyViewModel.SelectedChat == chatItem)
                             {
                                 ScrollViewerMessages.ScrollToEnd();
                             }
                         });
               });


            InitializeComponent();

           
        }

        ScrollViewer ScrollViewerMessages
        {
            get
            {
                if(scrollViewerMessages == null)
                {
                    if (VisualTreeHelper.GetChildrenCount(listMessages) > 0)
                    {
                        Border border = (Border)VisualTreeHelper.GetChild(listMessages, 0);
                        scrollViewerMessages = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    }
                }

                return scrollViewerMessages;
            }
        }

    }

}