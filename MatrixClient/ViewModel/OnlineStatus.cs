namespace MatrixClient.ViewModel
{
    /// <summary>
    /// Represents the XMPP online status
    /// </summary>
    public enum OnlineStatus
    {
        /// <summary>
        /// User is online
        /// </summary>
        Online,

        /// <summary>
        /// User is offline
        /// </summary>
        Offline,

        /// <summary>
        /// User is online and free for chat
        /// </summary>
        Chat,

        /// <summary>
        /// User is away
        /// </summary>
        Away,

        /// <summary>
        /// User is away for a longer period of time
        /// </summary>
        ExtendedAway,

        /// <summary>
        /// User does not want to be disturbed
        /// </summary>
        DoNotDisturb,
    }
}
