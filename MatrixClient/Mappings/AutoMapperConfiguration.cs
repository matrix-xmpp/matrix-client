namespace MatrixClient.Mappings
{
    using AutoMapper;
    using System;
    
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Builds the AutoMapper configuration
        /// </summary>
        /// <returns></returns>
        public static IMapper BuildMapperConfiguration()
        {
            var mc = new MapperConfiguration((cfg) =>
            {
                cfg
                    .CreateXmppShowTypeToViewOnlineStatusMap()
                    .CreateXmppMessageToViewMessageMap()
                    .CreateXmppRosterItemToViewContactMap()
                    .CreateXmppBookmarksConferenceToViewConferenceBookmarkMap()
                    .CreateXmppPresenceToViewPresenceMap()
                    .CreateXmppPresenceToSubscriptionRequestMap();
            });

            return new Mapper(mc);
        }

        private static IMapperConfigurationExpression CreateXmppShowTypeToViewOnlineStatusMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Show, ViewModel.OnlineStatus>().ConvertUsing(value =>
            {
                switch (value)
                {
                    case Matrix.Xmpp.Show.Away:
                        return ViewModel.OnlineStatus.Away;
                    case Matrix.Xmpp.Show.Chat:
                        return ViewModel.OnlineStatus.Chat;
                    case Matrix.Xmpp.Show.DoNotDisturb:
                        return ViewModel.OnlineStatus.DoNotDisturb;
                    case Matrix.Xmpp.Show.ExtendedAway:
                        return ViewModel.OnlineStatus.ExtendedAway;
                    case Matrix.Xmpp.Show.None:
                        return ViewModel.OnlineStatus.Online;
                    default:
                        return ViewModel.OnlineStatus.Offline;
                }
            });

            // reverse map
            cfg.CreateMap<ViewModel.OnlineStatus, Matrix.Xmpp.Show>().ConvertUsing(value =>
            {
                switch (value)
                {
                    case ViewModel.OnlineStatus.Away:
                        return Matrix.Xmpp.Show.Away;
                    case ViewModel.OnlineStatus.Chat:
                        return Matrix.Xmpp.Show.Chat;
                    case ViewModel.OnlineStatus.DoNotDisturb:
                        return Matrix.Xmpp.Show.DoNotDisturb;
                    case ViewModel.OnlineStatus.ExtendedAway:
                        return Matrix.Xmpp.Show.ExtendedAway;
                    case ViewModel.OnlineStatus.Online:
                        return Matrix.Xmpp.Show.None;
                    default:
                        return Matrix.Xmpp.Show.None;
                }
            });



            return cfg;
        }

        private static IMapperConfigurationExpression CreateXmppMessageToViewMessageMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Client.Message, ViewModel.Message>()                 
                   .ForMember(
                       dest => dest.Text,
                       opt => opt.MapFrom(src => src.Body)
                   )
                   .ForMember(
                       dest => dest.TimeStamp,
                       opt => opt.MapFrom(src => src.Delay != null ? src.Delay.Stamp : src.XDelay != null ? src.XDelay.Stamp : DateTime.Now)
                   );

            return cfg;
        }

        private static IMapperConfigurationExpression CreateXmppRosterItemToViewContactMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Roster.RosterItem, ViewModel.Contact>()
                  .ForMember(
                       dest => dest.Jid,
                       opt => opt.MapFrom(src => src.Jid.Bare)
                   )
                    .ForMember(
                       dest => dest.Name,
                       opt => opt.MapFrom(src => src.Name ?? src.Jid.Bare)
                   )
                   .ForMember(
                       dest => dest.IsConference,
                       opt => opt.UseValue<bool>(false)
                   )
                   .ForMember(
                       dest => dest.Subscription,
                       opt => opt.MapFrom(src => src.Subscription.ToString().ToLower())
                   );
            ;

            return cfg;
        }

        private static IMapperConfigurationExpression CreateXmppBookmarksConferenceToViewConferenceBookmarkMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Bookmarks.Conference, ViewModel.ConferenceBookmark>()
               .ForMember(
                    dest => dest.Jid,
                    opt => opt.MapFrom(src => src.Jid.Bare)
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name ?? "")
                )
                .ForMember(
                    dest => dest.AutoJoin,
                    opt => opt.MapFrom(src => src.AutoJoin)
                )
                .ForMember(
                    dest => dest.Nickname,
                    opt => opt.MapFrom(src => src.Nickname ?? "")
                );

            return cfg;
        }

        private static IMapperConfigurationExpression CreateXmppPresenceToViewPresenceMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Client.Presence, ViewModel.Presence>()
                .ForMember(
                     dest => dest.StatusText,
                     opt => opt.MapFrom(src => src.Status)
                 )
                 .ForMember(
                     dest => dest.Resource,
                     opt => opt.MapFrom(src => src.From.Resource)
                 )
                 .ForMember(
                     dest => dest.StatusText,
                     opt => opt.MapFrom(src => src.Status)
                 )
                 .ForMember(
                     dest => dest.OnlineStatus,
                     opt => opt.MapFrom(src => src.Show)
                 )
                 .ForMember(
                     dest => dest.Priority,
                     opt => opt.MapFrom(src => src.Priority)
                 );

            return cfg;
        }


        private static IMapperConfigurationExpression CreateXmppPresenceToSubscriptionRequestMap(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Matrix.Xmpp.Client.Presence, ViewModel.SubscriptionRequest>()
                .ForMember(
                     dest => dest.Jid,
                     opt => opt.MapFrom(src => src.From.Bare)
                 )
                 .ForMember(
                     dest => dest.Message,
                     opt => opt.MapFrom(src => src.Status)
                 )
                 .ForMember(
                     dest => dest.Name,
                     opt => opt.MapFrom(src => src.Nick != null ? src.Nick.Value : null )
                 );

            return cfg;
        }
    }
}
