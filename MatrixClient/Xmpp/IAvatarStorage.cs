namespace MatrixClient.Xmpp
{
    using MatrixClient.DbModel;

    /// <summary>
    /// Interface for an avatar store (cache)
    /// </summary>
    public interface IAvatarStorage
    {
        /// <summary>
        /// Adds an avatar to the store
        /// </summary>
        /// <param name="version"></param>
        /// <param name="data"></param>
        void AddAvatar(string hash, byte[] data);

        /// <summary>
        /// Replaces an avatar to the store
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="data"></param>
        void ReplaceAvatar(string hash, byte[] data);

        /// <summary>
        /// Checks if an avatar exists in the store
        /// </summary>
        /// <param name="version">the version (hash) of the avatar</param>
        /// <returns></returns>
        bool HasAvatar(string hash);

        /// <summary>
        /// Gets the avatar data from the storage
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Avatar GetAvatar(string hash);
    }
}
