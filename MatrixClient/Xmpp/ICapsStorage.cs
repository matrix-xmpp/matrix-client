namespace MatrixClient.Xmpp
{
    using MatrixClient.DbModel;

    public interface ICapsStorage
    {
        void AddCapability(string version, string xml);

        bool HasCapability(string version);

        Capability GetCapability(string version);
    }
}
