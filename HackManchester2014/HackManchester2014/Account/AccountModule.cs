using Nancy;

namespace HackManchester2014.Account
{
    using System;
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

                var entries = documentSession.Query<Entry>().Where(x => x.UserId == user.Id).ToList();

                var seed = new Random().Next(1, 2048);
                var model = new AccountIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation(seed),
                        I = seed
                    }
                };

                if (entries != null)
                {
                    var indexItems = documentSession.Query<EntryIndexItem>("EntryChildrenIndex").ToList()
                        .Where(x => entries.Any(y=>y.Id==x.Id)).ToList();

                    model.TotalDonated = indexItems.Sum(x => x.OwnDonation);
                    model.TotalDonations = indexItems.Count();
                    model.TotalRaised = indexItems.Sum(x => x.TotalDonations);
                    model.TotalNominated = indexItems.Sum(x => x.Decendants);
                }
                return Negotiate.WithView("Account")
                    .WithModel(model);
            };
        }
    }
}