namespace MatrixClient.Xmpp
{    
    using System.Threading.Tasks;

    public interface IPresenceManager
    {
        Task SendPresenceAsync();
        Task SendPresenceAsync(ViewModel.OnlineStatus status);
    }
}
