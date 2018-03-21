namespace MatrixClient.ViewModel
{
    public class Subscription
    {
        /// <summary>
        /// the user does not have a subscription to the contact's presence information,
        /// and the contact does not have a subscription to the user's presence information
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// the user has a subscription to the contact's presence information, 
        /// but the contact does not have a subscription to the user's presence information
        /// </summary>
        public const string To = "to";

        /// <summary>
        ///  the contact has a subscription to the user's presence information,
        ///  but the user does not have a subscription to the contact's presence information
        /// </summary>
        public const string From = "from";

        /// <summary>
        /// both the user and the contact have subscriptions to each other's presence information
        /// </summary>
        public const string Both = "both";
    }
}
