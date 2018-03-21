namespace MatrixClient.Xmpp
{
    using MatrixClient.DbModel;
    using MatrixClient.LiteDb;
    using Polly;

    public class UserAvatarStorage : StorageCache, IUserAvatarStorage
    {
        LiteDbContext liteDbContext;

        public UserAvatarStorage(LiteDbContext liteDbContext)
        {
            this.liteDbContext = liteDbContext;
        }

        /// <inheritdoc/>
        public void AddUserAvatar(string jid, string hash)
        {
            ReplaceUserAvatar(jid, hash);
        }

        public void ReplaceUserAvatar(string jid, string hash)
        {
            liteDbContext.UserAvatars.Upsert(new UserAvatar { Jid = jid, Hash = hash });
        }

        /// <inheritdoc/>
        public bool HasUserAvatar(string jid)
        {
            return GetUserAvatar(jid) != null;
        }

        /// <inheritdoc/>
        public UserAvatar GetUserAvatar(string jid)
        {
            return Cache.Execute(
                () => liteDbContext.UserAvatars.FindOne(ua => ua.Jid == jid),
                new Context(jid)
            );
        }
    }
}
