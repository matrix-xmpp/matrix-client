namespace MatrixClient.Xmpp
{
    using MatrixClient.DbModel;
    using MatrixClient.LiteDb;
    using Polly;

    public class CapsStorage : StorageCache, ICapsStorage
    {
        LiteDbContext liteDbContext;

        public CapsStorage(LiteDbContext liteDbContext)
        {
            this.liteDbContext = liteDbContext;
        }

        public void AddCapability(string version, string xml)
        {
            liteDbContext.Capabilities.Insert(new Capability { Version = version, Xml = xml });
        }

        public bool HasCapability(string version)
        {
            return GetCapability(version) != null;
        }

        public Capability GetCapability(string version)
        {
            return Cache.Execute(
                () => liteDbContext.Capabilities.FindOne(c => c.Version == version),
                new Context(version)
            );
        }
    }
}
