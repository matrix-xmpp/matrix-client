namespace MatrixClient.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Contacts : ObservableCollection<Contact>
    {        
        /// <summary>
        /// Is the given Jid on our contact list / aka roster
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public bool Contains(string jid)
        {
            return this.Any(c => c.Jid == jid);
        }     

        /// <summary>
        /// Gets a contact by Jid
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public Contact this[string jid]  
        {
            get { return this.FirstOrDefault(c => c.Jid == jid); }
        }
    }
}
