using Nancy.Routing;

namespace HackManchester2014.Home
{
    using System.Collections.Generic;
    using GeoJSON.Net.Feature;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Raven.Client;
    using System.Linq;

    public class AuthModule : NancyModule
    {
        public AuthModule(IDocumentSession documentSession)
        {
            Get["/Login"] = _ =>
            {
                var returnUrl = Context.Request.Query.returnUrl != null ? Context.Request.Query.returnUrl : null;
                Session["returnUrl"] = returnUrl;
                return Negotiate.WithView("Login");
            };

            Get["/Logout"] =
                _ => Nancy.Authentication.Forms.FormsAuthentication.LogOutAndRedirectResponse(Context, "~/");
        }
    }
}