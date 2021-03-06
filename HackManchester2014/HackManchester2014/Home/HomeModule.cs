using System.IO;
using HackManchester2014.Infrastructure.Indexes;
namespace HackManchester2014.Home
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using HackManchester2014.Domain;
    using HackManchester2014.Home.Models;
    using HackManchester2014.Map;
    using HackManchester2014.Map.Models;
    using Nancy;
    using Raven.Client;
    using System.Linq;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession)
        {
            Get["/"] = _ =>
            {
                var seed = new Random().Next(1, 2048);
                var entryId = documentSession.Query<EntryIndexItem>("EntryChildrenIndex")
                    .OrderByDescending(x => x.Decendants)
                    .FirstOrDefault();
                MapDonation Donation = null;
                if (entryId != null)
                {
                    var entry = documentSession.Load<Entry>(entryId.Id);
                    var children = documentSession.Query<Entry>().Where(x => x.ParentEntries.Any(y => y == entry.Id)).Where(x=>x.GeoIp != null).OrderBy(x=>x.ParentEntries).Take(256).ToList();
                    Donation = MapModule.BuildTree(entry, children);
                }
                var model = new HomeIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = Donation??MapModule.TestDonation(seed),
                        I = seed
                    },
                    TotalDonations = documentSession.Query<Entry>().ToList().Sum(x => x.Donation == null ? 0 : x.Donation.Amount ?? 0)
                };

                var user = Context.GetUser();
                if (user != null)
                {
                    var ach = documentSession.GetNewAchievements(Context.GetUser());
                    model.NewAchievements = ach;
                    if (ach.Any())
                    {
                        if (user.Achievements == null)
                        {
                            user.Achievements = new List<Achievement>();
                        }
                        user.Achievements.AddRange(model.NewAchievements);
                        documentSession.Store(user);
                    }

                }


                model.Entries = documentSession.Query<Entry>().Where(x => x.ProofImage != null).OrderByDescending(x => x.Donation.DonationDate).Take(6).ToList();

                model.TotalChallenges = documentSession.Query<Entry>().Count();

                return Negotiate.WithView("Index")
                    .WithModel(model);
            };
            Get["/register"] = _ => Response.AsRedirect("/register/1");
            Get["/register/1"] = _ =>
            {
                Session["returnUrl"] = "/register/2";
                return Negotiate.WithView("Register1");
            };

            Get["/register/2"] = _ =>
            {
                var challenges = documentSession.Query<Domain.Challenge>().Take(10).ToList();

                var viewModel = new HackManchester2014.Challenge.Models.ChallengesViewModel
                {
                    Challenges = challenges.Select(x => new HackManchester2014.Challenge.Models.ChallengeViewModel
                    {
                        Title = x.Title,
                        Brief = x.Brief,
                        Charity = x.CharityName,
                        Id = x.Id,
                    })
                };
                return Negotiate
                    .WithModel(viewModel)
                    .WithView("Register2");
            };
            Get["/image/{imageId}"] = _ =>
            {
                Guid imageId = _.imageId;
                var image = documentSession.Load<Image>(imageId);
                return Response.FromStream(new MemoryStream(Convert.FromBase64String(image.Base64Data)), image.ContentType);
            };
        }
    }
}