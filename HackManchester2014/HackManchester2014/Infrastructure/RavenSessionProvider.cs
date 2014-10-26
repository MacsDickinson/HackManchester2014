using Raven.Client.Indexes;

namespace HackManchester2014.Infrastructure
{
    using System.Configuration;
    using Raven.Client.Document;
    using Raven.Client;
    using HackManchester2014.Domain;
    using System.Transactions;

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
            session.Store(new Challenge
            {
                Id = "challenges/mind",
                Title = "Mindful for Minds",
                Brief = "Share a selfie with your thinking cap on",
                CharityName = "Mind",
                JustGivingCharityId = 300,
            });

            session.Store(new Challenge
            {
                Id = "challenges/autism",
                Title = "Tapie Selfie",
                Brief = "Tape your mouth shut to raise awareness for the issues faced by those with autism",
                CharityName = "Autism Concern",
                JustGivingCharityId = 114885,
            });

            session.Store(new Challenge
            {
                Id = "challenges/cancer",
                Title = "Spread Faster Than Cancer",
                Brief = "Spread your voice faster than cancer",
                CharityName = "Cancer Research UK",
                JustGivingCharityId = 2357,
            });

            session.SaveChanges();
        }
    }
}