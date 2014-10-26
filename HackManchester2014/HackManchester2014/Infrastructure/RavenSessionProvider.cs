using Raven.Client.Indexes;

namespace HackManchester2014.Infrastructure
{
    using System.Configuration;
    using Raven.Client.Document;
    using Raven.Client;
    using HackManchester2014.Domain;
    using System.Transactions;
    using System.Linq;
    using System;

    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

        public bool SessionInitialized { get; set; }

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static DocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                Url = ConfigurationManager.AppSettings["RAVENHQ_URI"],
                DefaultDatabase = ConfigurationManager.AppSettings["RAVENHQ_Database"],
                ApiKey = ConfigurationManager.AppSettings["RAVENHQ_APIKEY"],
            };
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(RavenSessionProvider).Assembly, store);
            using (var session = store.OpenSession())
            {
                EnsureSeedData(session);
            }

            return store;
        }

        private static void EnsureSeedData(IDocumentSession session)
        {
            var challenges = new[] {
                new Challenge
                {
                    Id = "challenges/mind",
                    Title = "Mindful for Minds",
                    Brief = "Share a selfie with your thinking cap on",
                    CharityName = "Mind",
                    JustGivingCharityId = 300,
                },

                new Challenge
                {
                    Id = "challenges/autism",
                    Title = "Tapie Selfie",
                    Brief = "Tape your mouth shut to raise awareness for the issues faced by those with autism",
                    CharityName = "Autism Concern",
                    JustGivingCharityId = 114885,
                },

                new Challenge
                {
                    Id = "challenges/cancer",
                    Title = "Spread Faster Than Cancer",
                    Brief = "Spread your voice faster than cancer",
                    CharityName = "Cancer Research UK",
                    JustGivingCharityId = 2357,
                }
            }.ToList();

            challenges.ForEach(session.Store);

            Random rnd = new Random();

            Action<Challenge, Entry, int> makeFakePeople = null;
            makeFakePeople = (c, e, depth) =>
            {
                if (depth < 5)
                {
                    var geoIp = new GeoIp
                    {
                        latitude = (float)(e == null ? 53.476362020773145 : HackManchester2014.Map.MapModule.RandomDouble(rnd) + e.GeoIp.latitude),
                        longitude = (float)(e == null ? -2.2513389587402344 : HackManchester2014.Map.MapModule.RandomDouble(rnd) + e.GeoIp.longitude)
                    };

                    for (int i = 0; i < 3; ++i)
                    {
                        var user = new User
                        {
                            Id = Guid.NewGuid(),
                            UserInfo = new SimpleAuthentication.Core.UserInformation
                            {
                                Email = "bob.bobbington@bobbles.bob",
                                Name = "Bob Bobbington",
                            }
                        };
                        session.Store(user);
                        var entry = new Entry(user, c, geoIp, e);
                        session.Store(entry);
                        makeFakePeople(c, e, depth + 1);
                    }
                }
            };

            //challenges.ForEach(c => makeFakePeople(c, null, 0));

            session.SaveChanges();
        }
    }
}