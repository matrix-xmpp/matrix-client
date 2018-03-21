namespace MatrixClient.ViewModel
{  
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class SubscriptionRequests : ObservableCollection<SubscriptionRequest>
    {
        public SubscriptionRequests()
        {
            Observable
               .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                       cc => this.CollectionChanged += cc,
                       cc => this.CollectionChanged -= cc)
                 .Subscribe(cc => 
                 {
                     OnPropertyChanged(new PropertyChangedEventArgs(nameof(Current)));                     
                 });  
        }
      
        /// <summary>
        /// Gets the first request of the collection (FIFO)
        /// </summary>
        public SubscriptionRequest Current
        {
            get
            {
                return this.FirstOrDefault();
            }
        }
        
        /// <summary>
        /// Do we have a subscription request for the given Jid?
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public bool Contains(string jid)
        {
            return this.Any(c => c.Jid == jid);
        }

        /// <summary>
        /// Gets a subscription request by Jid
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public SubscriptionRequest Get(string jid)
        {
            return this.FirstOrDefault(c => c.Jid == jid);
        }
    }
}
