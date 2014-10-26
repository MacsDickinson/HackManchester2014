using System.Linq;
using HackManchester2014.Domain;
using HackManchester2014.Infrastructure;

namespace HackManchester2014.Home
{
    using HackManchester2014.Domain;
    using HackManchester2014.Home.Models;
    using HackManchester2014.Infrastructure.Indexes;
    using HackManchester2014.Map;
    using HackManchester2014.Map.Models;
    using Nancy;
    using Raven.Client;
    using System.Linq;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession, IImageStore imageStore)
        {
            Get["/"] = _ =>
            {
                var model = new HomeIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation()
                    }
                };

                model.TotalDonations = documentSession.Query<Entry>().Sum(x => x.Donation.Amount ?? 0); ;
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

            Get["/register/3"] = _ =>
            {
                return Negotiate.WithView("Register3");
            };
            Get["/register/4"] = _ =>
            {
                return Negotiate.WithView("Register4");
            };
            Post["/register/4"] = _ =>
            {
                var httpFile = Request.Files.FirstOrDefault();
                if (httpFile != null)
                {
                    var Id = imageStore.SaveImage(httpFile.Value);
                    var image = new Image()
                    {
                        Id = Id,
                        ContentType=httpFile.ContentType,
                        Name=httpFile.Name
                    };
                    documentSession.Store(image);
                }
                return Response.AsRedirect("/");
            };
        }
    }
}