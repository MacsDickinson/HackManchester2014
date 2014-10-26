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

                var totals = documentSession.Query<UserStatsIndexItem, UserStatsIndex>()
                    .Where(x => x.UserId == user.Id)
                    .SingleOrDefault();

                var seed = new Random().Next(1, 2048);
                var model = new AccountIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation(seed),
                        I = seed
                    }
                };

                if (totals != null)
                {
                    model.TotalDonated = totals.OwnDonation;
                    model.TotalDonations = totals.DonatedCount;
                    model.TotalNominated = totals.NominatedCount;
                    model.TotalRaised = totals.DonationsTotal;
                }
                return Negotiate.WithView("Account")
                    .WithModel(model);
            };
        }
    }
}