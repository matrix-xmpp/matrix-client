namespace MatrixClient.LiteDb
{
    using LiteDB;
    using MatrixClient.DbModel;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// LiteDb database context class
    /// </summary>
    public class LiteDbContext
    {        
        public LiteDbContext(IDbConfiguration liteDbConfiguration)
        {
            var dbFolder = Path.Combine(UserHomeDir, "MatriX Client");
            var dbPath = Path.Combine(dbFolder, $"{liteDbConfiguration.DatabaseName}.db");

            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            InitLiteDb(dbPath);
        }

        private string UserHomeDir => Environment.GetEnvironmentVariable(
                                        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                                            ? "LocalAppData"    // Windows
                                            : "Home");          // Linux

        /// <summary>
        /// Gets the Database instance
        /// </summary>
        public LiteDatabase Database { get; private set; }

        /// <summary>
        /// Gets the avatar collection
        /// </summary>
        public LiteCollection<Avatar> Avatars { get; private set; }

        /// <summary>
        /// Gets the account collection
        /// </summary>
        public LiteCollection<Account> Accounts { get; private set; }

        public LiteCollection<Contact> Contacts { get; private set; }

        public LiteCollection<Capability> Capabilities { get; private set; }

        public LiteCollection<UserAvatar> UserAvatars { get; private set; }

        private void InitLiteDb(string dbFile)
        {
            Database = new LiteDatabase(dbFile);

            // Mappings
            var mapper = BsonMapper.Global;

            mapper.Entity<UserAvatar>()
                .Id(x => x.Jid);

            mapper.Entity<Avatar>()
                .Id(x => x.ImageHash);

            // Get a collections (or create, if doesn't exist)
            Avatars = Database.GetCollection<Avatar>(nameof(Avatars));
            Contacts = Database.GetCollection<Contact>(nameof(Contacts));
            Accounts = Database.GetCollection<Account>(nameof(Accounts));
            Capabilities = Database.GetCollection<Capability>(nameof(Capabilities));
            UserAvatars = Database.GetCollection<UserAvatar>(nameof(UserAvatars));

           
            //UserAvatars.EnsureIndex(u => u.Jid, true);

            Contacts.EnsureIndex(c => c.AccountId);
            Contacts.EnsureIndex(c => c.Jid);
            
            Capabilities.EnsureIndex(c => c.Version, true);            

            Accounts.EnsureIndex(a => a.Jid, true);           
        }
    }
}
