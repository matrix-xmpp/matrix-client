namespace MatrixClient.Xmpp
{
    using Matrix;
    using Matrix.Crypt;
    using Matrix.Xmpp;
    using Matrix.Xmpp.Avatar;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.PubSub;
    using System;

    public class AvatarManager
    {
        public static Iq CreateRequestAvatarStanza(Jid to, string id)
        {
            var pIq = new PubSubIq
            {
                To = to,
                Type = IqType.Get
            };

            var items = new Items
            {
                Node = Namespaces.AvatarData
            };
            items.AddItem(new Item { Id = id });

            pIq.PubSub.Items = items;

            return pIq;           
        }

        public static Iq CreatePublishAvatarDataStanza(byte[] avatarBytes)
        {
            /*
            <iq type='set' from='juliet@capulet.lit/chamber' id='publish1'>
              <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                <publish node='urn:xmpp:avatar:data'>
                  <item id='111f4b3c50d7b0df729d299bc6f8e9ef9066971f'>
                    <data xmlns='urn:xmpp:avatar:data'>
                      qANQR1DBwU4DX7jmYZnncm...
                    </data>
                  </item>
                </publish>
              </pubsub>
            </iq>
           
            */
            var pIq = new PubSubIq { Type = IqType.Set };

            var publish = new Publish { Node = Namespaces.AvatarData };

            var item = new Item { Id = Hash.Sha1HashHex(avatarBytes)};           
            item.Add(
                new Data { Bytes = avatarBytes }
            );

            publish.AddItem(item);
            pIq.PubSub.Publish = publish;

            return pIq;
        }

        public static Iq CreatePublishAvatarMetadataStanza(byte[] avatarBytes, int height, int width, string contentType)
        {
            /*
            <iq type='set' from='juliet@capulet.lit/chamber' id='publish1'>
              <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                <publish node='urn:xmpp:avatar:data'>
                  <item id='111f4b3c50d7b0df729d299bc6f8e9ef9066971f'>
                    <data xmlns='urn:xmpp:avatar:data'>
                      qANQR1DBwU4DX7jmYZnncm...
                    </data>
                  </item>
                </publish>
              </pubsub>
            </iq>

             <iq type='set' from='juliet@capulet.lit/chamber' id='publish2'>
                  <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                    <publish node='urn:xmpp:avatar:metadata'>
                      <item id='111f4b3c50d7b0df729d299bc6f8e9ef9066971f'>
                        <metadata xmlns='urn:xmpp:avatar:metadata'>
                          <info bytes='12345'
                                id='111f4b3c50d7b0df729d299bc6f8e9ef9066971f'
                                height='64'
                                type='image/png'
                                width='64'/>
                        </metadata>
                      </item>
                    </publish>
                  </pubsub>
                </iq>
                */
            var pIq = new PubSubIq { Type = IqType.Set };

            // publish tag
            var publish = new Publish { Node = Namespaces.AvatarMetadata };

            // metadata Tag including the info child
            var metaData = new Metadata()
            {
                Info = new Info
                {
                    CountBytes = avatarBytes.Length,
                    Id = Hash.Sha1HashHex(avatarBytes),
                    Height = height,
                    Width = width,
                    Type = contentType
                }
            };

            var item = new Item { Id = metaData.Info.Id };
            item.Add(metaData);
            publish.AddItem(item);

            pIq.PubSub.Publish = publish;

            return pIq;
        }

    }
}
