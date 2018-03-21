namespace MatrixClient.Xmpp
{
    using System.Collections.Generic;
    using Matrix.Xmpp;
    using Matrix.Xmpp.Capabilities;
    using Matrix.Xmpp.Disco;

    /// <summary>
    /// This class is holding the client capabilities
    /// </summary>
    public class ClientCaps : ICaps
    {
        public ClientCaps(ICapsStorage capsStorage)
        {
            var myDiscoInfo = DiscoInfo;
            capsHash = Caps.BuildHash(myDiscoInfo);

            // lets store our own caps as well
            if (!capsStorage.HasCapability(capsHash))
            {
                capsStorage.AddCapability(capsHash, myDiscoInfo.ToString());
            }
        }

        const string CLIENT_NAME    = "MatriX Client";
        const string CLIENT_VERSION = "0.1";
        const string HASH           = "sha-1";

             
        string capsHash;

        public IList<string> Features => new List<string>
                                                {
                                                    Namespaces.DiscoInfo,
                                                    Namespaces.Caps,
                                                    NotifyFeature(Namespaces.AvatarMetadata),
                                                    Namespaces.Nick,
                                                    NotifyFeature(Namespaces.Nick)
                                                };

        public List<Identity> Identities => new List<Identity>
                                                    {
                                                        new Identity()
                                                        {
                                                            Name = $"{CLIENT_NAME} {CLIENT_VERSION}",
                                                            Category = "client",
                                                            Type = "pc"
                                                        }
                                                    };

        public string Node => "http://ag-software.de";

        /// <inheritdoc/>
        public string CapsHash => capsHash;

        /// <inheritdoc/>
        public Caps ClientCapabilities
        {
            get
            {
                var clientCapabilities = new Caps
                {
                    Hash = HASH,
                    Version = capsHash
                };

                if (Node != null)
                    clientCapabilities.Node = Node;

                return clientCapabilities;
            }
        }

        /// <inheritdoc/>
        public Info DiscoInfo
        {
            get
            {
                var discoInfo = new Info();

                foreach (Identity id in Identities)
                    discoInfo.AddIdentity(id);

                foreach (var feat in Features)
                    discoInfo.AddFeature(new Feature(feat));

                return discoInfo;
            }
        }

        /// <summary>
        /// Appends the +notify to the namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        private string NotifyFeature(string ns) => $"{ns}+notify";

    }
}
