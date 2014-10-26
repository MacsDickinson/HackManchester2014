using Nancy;

namespace HackManchester2014.Account
{
    using HackManchester2014.Account.Models;
    using HackManchester2014.Domain;
    using HackManchester2014.Infrastructure.Indexes;
    using HackManchester2014.Map;
    using HackManchester2014.Map.Models;
    using Nancy.Security;
    using Raven.Client;
    using Raven.Client.Linq;
    using System.Linq;

    public class AccountModule : NancyModule
    {
        public AccountModule(IDocumentSession documentSession)
            : base ("my-account")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var user = base.Context.GetUser();

                var entries = documentSession.Query<Entry>().FirstOrDefault(x => x.UserId == user.Id);

                var model = new AccountIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation()
                    }
                };

                if (entries != null)
                {
                    var indexItems = documentSession.Query<EntryIndexItem>("EntryChildrenIndex")
                        .Where(x => entries.Id == x.Id);

                    model.TotalDonated = indexItems.Sum(x => x.OwnDonation);
                    model.TotalDonations = indexItems.Count();
                    model.TotalRaised = indexItems.Sum(x => x.TotalDonations);
                    model.TotalDonations = indexItems.Sum(x => x.Decendants);
                }
                return Negotiate.WithView("Account")
                    .WithModel(model);
            };
        }
    }
}