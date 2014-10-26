using System.Linq;
using HackManchester2014.Domain;
using HackManchester2014.Infrastructure;

namespace HackManchester2014.Home
{
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
                var model = new HomeIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation()
                    }
                };
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

                var viewModel = new Challenge.Models.ChallengesViewModel
                {
                    Challenges = challenges.Select(x => new Challenge.Models.ChallengeViewModel
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
        }
    }
}