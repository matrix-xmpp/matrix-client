namespace MatrixClient.Xmpp.Handlers
{
    using Matrix.Attributes;
    using Matrix.Network.Handlers;
    using Matrix.Xml;
    using Matrix.Xmpp;
    using MatrixClient.ViewModel;

    /// <summary>
    /// This handler automatically replies to incoming XMPP Pings from clients or servers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Name("ChatMessage-Handler")]
    public class ChatMessageHandler : XmppStanzaHandler
    {
        public ChatMessageHandler(MyViewModel viewModel)
        {
            Handle(
                el =>
                    el.OfType<Matrix.Xmpp.Client.Message>()
                    && el.Cast<Matrix.Xmpp.Client.Message>().Type == MessageType.Chat,                   

                async (context, xmppXElement) =>
                {
         
                });
        }
    }
}