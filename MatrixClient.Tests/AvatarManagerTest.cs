namespace MatrixClient.Tests
{
    using MatrixClient.Xmpp;    
    using System.Text;
    using Xunit;

    public class AvatarManagerTest
    {
        [Fact]
        public void CreateRequestAvatarStanzaTest()
        {
            var expectedXml = @"<iq id='foo' to='user@server.com' type='get' xmlns='jabber:client'>
                <pubsub xmlns='http://jabber.org/protocol/pubsub'>                    
                     <items node='urn:xmpp:avatar:data'>
                      <item id='2aae6c35c94fcfb415dbe95f408b9ce91ee846ed'/>
                    </items>                    
                  </pubsub>
                </iq>";
            
            var pIq = AvatarManager.CreateRequestAvatarStanza("user@server.com", "2aae6c35c94fcfb415dbe95f408b9ce91ee846ed");
            pIq.Id = "foo";
            pIq.ShouldBe(expectedXml);
        }

        [Fact]
        public void CreatePublishAvatarDataStanzaTest()
        {
            var expectedXml = @"<iq id='foo' type='set' xmlns='jabber:client'>
                <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                    <publish node='urn:xmpp:avatar:data'>
                      <item id='2aae6c35c94fcfb415dbe95f408b9ce91ee846ed'>
                        <data xmlns='urn:xmpp:avatar:data'>aGVsbG8gd29ybGQ=</data>
                      </item>
                    </publish>
                  </pubsub>
                </iq>";

            var bytes = Encoding.ASCII.GetBytes("hello world");
            var pIq = AvatarManager.CreatePublishAvatarDataStanza(bytes);
            pIq.Id = "foo";
            pIq.ShouldBe(expectedXml);
        }

        [Fact]
        public void CreatePublishAvatarMetadataStanzaTest()
        {
            var expectedXml = @"<iq type='set' id='foo' xmlns='jabber:client'>
                  <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                    <publish node='urn:xmpp:avatar:metadata'>
                      <item id='2aae6c35c94fcfb415dbe95f408b9ce91ee846ed'>
                        <metadata xmlns='urn:xmpp:avatar:metadata'>
                          <info bytes='11'
                                id='2aae6c35c94fcfb415dbe95f408b9ce91ee846ed'
                                height='100'
                                type='image/jpg'
                                width='100'/>
                        </metadata>
                      </item>
                    </publish>
                  </pubsub>
                </iq>";

            var bytes = Encoding.ASCII.GetBytes("hello world");
            var pIq = AvatarManager.CreatePublishAvatarMetadataStanza(bytes, 100, 100, "image/jpg");
            pIq.Id = "foo";
            pIq.ShouldBe(expectedXml);
        }
    }
}
