namespace MatrixClient.Xmpp
{
    using System.Collections.Generic;
    using Matrix.Xmpp.Capabilities;
    using Matrix.Xmpp.Disco;
    
    /// <summary>
    /// interface for entity capabilities
    /// </summary>
    public interface ICaps
    {
        /// <summary>
        /// Gets the list of supported features
        /// </summary>
        IList<string> Features { get; }

        /// <summary>
        /// Gets the list of identities
        /// </summary>
        List<Identity> Identities { get; }

        /// <summary>
        /// Gets the node value for the entity capabilities
        /// </summary>
        string Node { get; }

        /// <summary>
        /// Gets the client Capabilities Xml
        /// </summary>
        Caps ClientCapabilities { get; }

        /// <summary>
        /// Gets the Disco information for this client
        /// </summary>
        Info DiscoInfo { get; }

        /// <summary>
        /// Gets the caps hash
        /// </summary>
        string CapsHash { get; }
    }
}
