namespace MatrixClient.Xmpp
{
     using MatrixClient.DbModel;

    /// <summary>
    /// Interface for an avatar store (cache)
    /// </summary>
    public interface IUserAvatarStorage
    {
        /// <summary>
        /// Adds a user avatar to the store
        /// </summary>
        /// <param name="version"></param>
        /// <param name="data"></param>
        void AddUserAvatar(string jid, string hash);

        /// <summary>
        /// replaces a user avatar to the store
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="hash"></param>
        void ReplaceUserAvatar(string jid, string hash);

        /// <summary>
        /// Checks if an user avatar exists in the store
        /// </summary>
        /// <param name="version">the version (hash) of the avatar</param>
        /// <returns></returns>
        bool HasUserAvatar(string jid);

        /// <summary>
        /// Gets the user avatar data from the storage
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        UserAvatar GetUserAvatar(string jid);
    }
}
