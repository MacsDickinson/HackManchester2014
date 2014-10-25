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
                CharityName = "Mind",
                JustGivingCharityId = 300,
            });

            session.SaveChanges();
        }
    }
}