namespace MatrixClient.Tests
{
    using AutoMapper;
    using Matrix.Xmpp.Client;
    using Matrix.Xmpp.Roster;
    using MatrixClient.Mappings;
    using Microsoft.Extensions.DependencyInjection;
    using Shouldly;
    using Xunit;

    using ViewPresence = MatrixClient.ViewModel.Presence;

    public class AutoMapperConfigurationTests
    {
        [Fact]
        public void Status_Mapping_Tests()
        {
            var mapper = ServiceLocator.Current.GetService<IMapper>();
            mapper.Map<MatrixClient.ViewModel.OnlineStatus>(Matrix.Xmpp.Show.Chat).ShouldBe(MatrixClient.ViewModel.OnlineStatus.Chat);
            mapper.Map<MatrixClient.ViewModel.OnlineStatus>(Matrix.Xmpp.Show.DoNotDisturb).ShouldBe(MatrixClient.ViewModel.OnlineStatus.DoNotDisturb);
            mapper.Map<MatrixClient.ViewModel.OnlineStatus>(Matrix.Xmpp.Show.Away).ShouldBe(MatrixClient.ViewModel.OnlineStatus.Away);
            mapper.Map<MatrixClient.ViewModel.OnlineStatus>(Matrix.Xmpp.Show.ExtendedAway).ShouldBe(MatrixClient.ViewModel.OnlineStatus.ExtendedAway);
            mapper.Map<MatrixClient.ViewModel.OnlineStatus>(Matrix.Xmpp.Show.None).ShouldBe(MatrixClient.ViewModel.OnlineStatus.Online);
        }

        [Fact]
        public void Status_Reverse_Mapping_Tests()
        {
            var mapper = ServiceLocator.Current.GetService<IMapper>();
            mapper.Map<Matrix.Xmpp.Show>(MatrixClient.ViewModel.OnlineStatus.Chat).ShouldBe(Matrix.Xmpp.Show.Chat);
            mapper.Map<Matrix.Xmpp.Show>(MatrixClient.ViewModel.OnlineStatus.Away).ShouldBe(Matrix.Xmpp.Show.Away);
            mapper.Map<Matrix.Xmpp.Show>(MatrixClient.ViewModel.OnlineStatus.DoNotDisturb).ShouldBe(Matrix.Xmpp.Show.DoNotDisturb);
            mapper.Map<Matrix.Xmpp.Show>(MatrixClient.ViewModel.OnlineStatus.ExtendedAway).ShouldBe(Matrix.Xmpp.Show.ExtendedAway);
            mapper.Map<Matrix.Xmpp.Show>(MatrixClient.ViewModel.OnlineStatus.Online).ShouldBe(Matrix.Xmpp.Show.None);
        }

        [Fact]
        public void Presence_Mapping_Tests()
        {
            var mapper = ServiceLocator.Current.GetService<IMapper>();

            var pres = new Presence
            {
                From = "alex@ag-software.net/Foo",
                Status = "I am here!",
                Show = Matrix.Xmpp.Show.ExtendedAway,
                Priority = 99
            };
            var viewPres = mapper.Map<ViewPresence>(pres);
            viewPres.StatusText.ShouldBe("I am here!");
            viewPres.Resource.ShouldBe("Foo");
            viewPres.Priority.ShouldBe(99);
            viewPres.OnlineStatus.ShouldBe(MatrixClient.ViewModel.OnlineStatus.ExtendedAway);


            var pres2 = new Presence();
            var viewPres2 = mapper.Map<ViewPresence>(pres2);
            viewPres2.OnlineStatus.ShouldBe(MatrixClient.ViewModel.OnlineStatus.Online);

            var pres3 = new Presence { Show = Matrix.Xmpp.Show.Away };
            var viewPres3 = mapper.Map<ViewPresence>(pres3);
            viewPres3.OnlineStatus.ShouldBe(MatrixClient.ViewModel.OnlineStatus.Away);

            var pres4 = new Presence { Show = Matrix.Xmpp.Show.DoNotDisturb };
            var viewPres4 = mapper.Map<ViewPresence>(pres4);
            viewPres4.OnlineStatus.ShouldBe(MatrixClient.ViewModel.OnlineStatus.DoNotDisturb);

            var pres5 = new Presence { Show = Matrix.Xmpp.Show.Chat };
            var viewPres5 = mapper.Map<ViewPresence>(pres5);
            viewPres5.OnlineStatus.ShouldBe(MatrixClient.ViewModel.OnlineStatus.Chat);
        }

        [Fact]
        public void Contact_Mapping_Tests()
        {
            var mapper = AutoMapperConfiguration.BuildMapperConfiguration();

            var rosterItem = new RosterItem { Name = "Alex", Jid = "alex@server.com", Subscription = Subscription.None };

            var contact = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem);

            contact.Jid.ShouldBe("alex@server.com");
            contact.Name.ShouldBe("Alex");
            contact.Subscription.ShouldBe(MatrixClient.ViewModel.Subscription.None);
        }

        [Fact]
        public void Contact_Mapping_All_SubScriptionStates_Tests()
        {
            var mapper = AutoMapperConfiguration.BuildMapperConfiguration();

            var rosterItem = new RosterItem { Name = "Alex", Jid = "alex@server.com", Subscription = Subscription.Both };
            var contact = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem);           
            contact.Subscription.ShouldBe(MatrixClient.ViewModel.Subscription.Both);

            var rosterItem2 = new RosterItem { Name = "Alex", Jid = "alex@server.com", Subscription = Subscription.None };
            var contact2 = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem2);
            contact2.Subscription.ShouldBe(MatrixClient.ViewModel.Subscription.None);

            var rosterItem3 = new RosterItem { Name = "Alex", Jid = "alex@server.com", Subscription = Subscription.To };
            var contact3 = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem3);
            contact3.Subscription.ShouldBe(MatrixClient.ViewModel.Subscription.To);

            var rosterItem4 = new RosterItem { Name = "Alex", Jid = "alex@server.com", Subscription = Subscription.From };
            var contact4 = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem4);
            contact4.Subscription.ShouldBe(MatrixClient.ViewModel.Subscription.From);
        }

        [Fact]
        public void Contact_Mapping_Test_When_Name_Is_Missing_Should_Default_To_Jid()
        {
            var mapper = AutoMapperConfiguration.BuildMapperConfiguration();

            var rosterItem = new RosterItem { Jid = "alex@server.com" };

            var contact = mapper.Map<MatrixClient.ViewModel.Contact>(rosterItem);

            contact.Jid.ShouldBe("alex@server.com");
            contact.Name.ShouldBe("alex@server.com");
        }

        [Fact]
        public void Message_Mapping_Test()
        {
            var mapper = AutoMapperConfiguration.BuildMapperConfiguration();

            var message = new Message { From = "alex@server.com", Body = "Foo" };
            var viewMessage = mapper.Map<MatrixClient.ViewModel.Message>(message);

            viewMessage.Text.ShouldBe("Foo");
            viewMessage.Id.ShouldBeNull();

            message.Id = "12345";
            var viewMessage2 = mapper.Map<MatrixClient.ViewModel.Message>(message);            
            viewMessage2.Id.ShouldBe("12345");
        }

        [Fact]
        public void SubscriptionRequest_Mapping_Tests()
        {
            var mapper = ServiceLocator.Current.GetService<IMapper>();

            var pres = new Presence
            {
                From = "alex@server.com/Foo",
                Status = "hey alex, pleas allow me to add to your presence",                
                Nick = new Matrix.Xmpp.Nickname.Nick("Peter")
            };

            var subscriptionRequest = mapper.Map<MatrixClient.ViewModel.SubscriptionRequest>(pres);
            subscriptionRequest.Jid.ShouldBe("alex@server.com");
            subscriptionRequest.Name.ShouldBe("Peter");
            subscriptionRequest.Message.ShouldBe("hey alex, pleas allow me to add to your presence");


            var pres2 = new Presence
            {
                From = "alex@server.com",                
            };

            var subscriptionRequest2 = mapper.Map<MatrixClient.ViewModel.SubscriptionRequest>(pres2);
            subscriptionRequest2.Jid.ShouldBe("alex@server.com");
            subscriptionRequest2.Name.ShouldBeNull();
            subscriptionRequest2.Message.ShouldBeNull();
        }
    }
}
