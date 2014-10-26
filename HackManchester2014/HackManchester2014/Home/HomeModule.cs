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
                return Negotiate.WithView("Register2");
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