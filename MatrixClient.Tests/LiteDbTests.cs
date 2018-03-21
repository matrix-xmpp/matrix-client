namespace MatrixClient.Tests
{
    using Xunit;
    using Shouldly;
    using MatrixClient.DbModel;
    using Microsoft.Extensions.DependencyInjection;
    using MatrixClient.LiteDb;
    using System.IO;
    using AutoMapper;
    using Microsoft.Extensions.Caching.Memory;
    using Polly;
    using System;
    using Polly.Caching.MemoryCache;
    using System.Threading.Tasks;
    using MatrixClient.Xmpp;

    public class LiteDbTests
    {
        //[Fact]
        public async Task CacheTestAsync()
        {
           
            var ctx = ServiceLocator.Current.GetService<LiteDbContext>();
            var store = new UserAvatarStorage(ctx);

            store.ReplaceUserAvatar("alex@ag-softwrae.net", "22222");
            store.ReplaceUserAvatar("alex@ag-softwrae.net", "33333");
            store.ReplaceUserAvatar("bob@ag-softwrae.net", "444444");
            store.ReplaceUserAvatar("romeo@ag-softwrae.net", "1111111");
        }



        //[Fact]
        public void Dbtest()
        {
            var dbFile = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            //var dbFile2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Path.GetTempFileName());

            var old = ServiceLocator.Current.GetService<IMapper>();


            //.AddSingleton(new LiteDbConfiguration { DatabaseName = databaseName})
            ServiceLocator.ServiceCollection.ReplaceSingleton<IDbConfiguration>(new LiteDbConfiguration { DatabaseName = dbFile });


            var ctx2 = ServiceLocator.ServiceCollection.BuildServiceProvider().GetService<LiteDbContext>();

            var ctx = ServiceLocator.Current.GetService<LiteDbContext>();

            if (!ctx.Accounts.Exists(a => a.Jid == "alex@ag-software.net"))
            {
                ctx.Accounts.Insert(new Account { Jid = "alex@ag-software.net" });
            }

            if (!ctx.Accounts.Exists(a => a.Jid == "gnauck@jabber.org"))
            {
                ctx.Accounts.Insert(new Account { Jid = "gnauck@jabber.org" });
            }

            ctx.Accounts.Count().ShouldBe(2);
        }


    }
}

