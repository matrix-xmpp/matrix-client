namespace MatrixClient.Xmpp
{
    using MatrixClient.DbModel;
    using MatrixClient.LiteDb;
    using Polly;

    public class AvatarStorage : StorageCache, IAvatarStorage
    {
        LiteDbContext liteDbContext;

        public AvatarStorage(LiteDbContext liteDbContext)
        {
            this.liteDbContext = liteDbContext;
        }
            /// <inheritdoc/>
        public void AddAvatar(string hash, byte[] data)
        {
            ReplaceAvatar(hash, data);            
        }

        public void ReplaceAvatar(string hash, byte[] data)
        {
            liteDbContext.Avatars.Upsert(new Avatar { ImageHash = hash, ImageBytes = data });
        }

        /// <inheritdoc/>
        public bool HasAvatar(string hash)
        {
            return GetAvatar(hash) != null;
        }

        /// <inheritdoc/>
        public Avatar GetAvatar(string hash)
        {
            return Cache.Execute(
                () => liteDbContext.Avatars.FindOne(c => c.ImageHash == hash),
                new Context(hash)
            );
        }
    }
}
