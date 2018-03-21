namespace MatrixClient.Xmpp.Handlers
{
    using Matrix;
    using Matrix.Attributes;
    using Matrix.Extensions.Client.Disco;
    using Matrix.Xml;
    using Matrix.Xmpp;
    using Matrix.Xmpp.Capabilities;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.Disco;

    using System;

    /// <summary>
    /// This handler collects caps information
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Name("CapsCollection-Handler")]
    public class CapsCollectionHandler : XmppStanzaHandlerEx
    {
        public CapsCollectionHandler(ICapsStorage capsStorage)
        {
            Handle(
                el =>
                    el.OfType<Presence>(),

                async (context, xmppXElement) =>
                {
                    var pres = xmppXElement.Cast<Presence>();
                    var caps = pres.Caps;
                    if (caps != null
                        && caps.Hash != null    // old style caps has no hash attribute, we ignore old style caps
                        && !capsStorage.HasCapability(caps.Version))
                    {
                        try
                        {
                            var discoResult = await this.DiscoverInformationAsync(pres.From, caps.Node + "#" + caps.Version);

                            if (discoResult.Type == IqType.Result)
                            {

                                if (discoResult.Query.OfType<Info>())
                                {
                                    var info = discoResult.Query as Info;
                                    // check if Hash is correct
                                    if (caps.HashAlgorithm != Matrix.Crypt.HashAlgorithms.Unknown)
                                    {
                                        try
                                        {
                                            var validateHash = Caps.BuildHash(info, caps.HashAlgorithm);
                                            if (validateHash == caps.Version && !capsStorage.HasCapability(caps.Version))
                                            {
                                                capsStorage.AddCapability(caps.Version, info.ToString(true));
                                            }
                                            else
                                            {
                                                // wrong hash, malicious or broken client
                                                // ignore or log
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            // could crash because of not supported Hash algorithm by the platform
                                            // ignore
                                        }
                                    }
                                    else
                                    {
                                        if (caps.Version != null && !capsStorage.HasCapability(caps.Version))
                                            capsStorage.AddCapability(caps.Version, info.ToString(true));
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignore, eg timeouts                            
                        }
                    }
                });
        }
    }
}