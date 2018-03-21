namespace MatrixClient.Xmpp
{
    using Matrix.Xmpp;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.Nickname;
    using Matrix.Xmpp.PubSub;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NicknameManager
    {
        public static Iq CreatePublishNicknameStanza(string nickname)
        {
            var pIq = new PubSubIq { Type = IqType.Set };

            var publish = new Publish { Node = Namespaces.Nick };

            var item = new Item();
            item.Add(new Nick(nickname));

            publish.AddItem(item);
            pIq.PubSub.Publish = publish;

            return pIq;
        }
    }
}
