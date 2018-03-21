namespace MatrixClient.Xmpp
{
    using Polly;
    using Polly.Caching;
    using Polly.Caching.MemoryCache;
    using System;

    public abstract class StorageCache
    {
        internal protected CachePolicy Cache;
        TimeSpan cacheDuration = TimeSpan.FromMinutes(60);

        public StorageCache()
        {
#if NET47            
            MemoryCacheProvider memoryCacheProvider 
                = new MemoryCacheProvider(System.Runtime.Caching.MemoryCache.Default);
#else
            Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache
                = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());

            MemoryCacheProvider memoryCacheProvider = new MemoryCacheProvider(memoryCache);
#endif

            Cache = Policy.Cache(memoryCacheProvider, cacheDuration);
        }
    }
}
