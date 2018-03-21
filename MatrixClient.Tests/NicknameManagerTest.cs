namespace MatrixClient.Tests
{
    using MatrixClient.Xmpp;    
    using Xunit;

    public class NicknameManagerTest
    {
        [Fact]
        public void CreatePublishNicknameStanzaTest()
        {
            var expectedXml = @"<iq type='set' id='foo' xmlns='jabber:client'>
                                  <pubsub xmlns='http://jabber.org/protocol/pubsub'>
                                    <publish node='http://jabber.org/protocol/nick'>
                                      <item>
                                        <nick xmlns='http://jabber.org/protocol/nick'>Alex</nick>
                                      </item>
                                    </publish>
                                  </pubsub>
                                </iq>";

            
            var pIq = NicknameManager.CreatePublishNicknameStanza("Alex");
            pIq.Id = "foo";
            pIq.ShouldBe(expectedXml);
        }      
    }
}
