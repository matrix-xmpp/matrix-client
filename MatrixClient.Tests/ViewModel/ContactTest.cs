namespace MatrixClient.Tests.ViewModel
{
    using Xunit;
    using Shouldly;
    using MatrixClient.ViewModel;

    public class ContactTest
    {
        [Fact]
        public void OnlineStatus_Should_Be_Offline()
        {
            var contact = new Contact();
            contact.OnlineStatus.ShouldBe(OnlineStatus.Offline);
        }

        [Fact]
        public void OnlineStatus_Should_Be_Away()
        {
            var contact = new Contact();
            contact.Presences.Add(new Presence { OnlineStatus = OnlineStatus.DoNotDisturb, Priority = 10 });
            contact.Presences.Add(new Presence { OnlineStatus = OnlineStatus.Away, Priority = 20 });

            contact.OnlineStatus.ShouldBe(OnlineStatus.Away);
        }

        [Fact]
        public void StatusText_Should_Be_Bar()
        {
            var contact = new Contact();
            contact.Presences.Add(new Presence { Priority = 10, StatusText = "Foo" });
            contact.Presences.Add(new Presence { Priority = 20, StatusText = "Bar" });

            contact.StatusText.ShouldBe("Bar");
        }

        [Fact]
        public void StatusText_Should_Be_Empty()
        {
            var contact = new Contact();
            contact.StatusText.ShouldBe("");
        }

        [Fact]
        public void MessageResource_For_Offline_Contact_Should_Be_Null()
        {
            // create a contact without any Presences
            var contact = new Contact();
            contact.MessageResource.ShouldBe(null);            
        }

        [Fact]
        public void MessageResource_Should_Be_Null_Because_LastMessageResource_Not_Available()
        {            
            var contact = new Contact();            
            contact.LastMessageResource = "Home";
            contact.MessageResource.ShouldBe(null);            
        }

        [Fact]
        public void MessageResource_Should_Not_Be_Null_Because_LastMessageResource_Is_Available()
        {
            // create a contact without any Presences
            var contact = new Contact();            

            // should still be "Home", because "Home" is online
            contact.LastMessageResource = "Home";
            contact.Presences.Add(new Presence { Resource = "Home" });
            contact.MessageResource.ShouldBe("Home");
        }
    }
}
