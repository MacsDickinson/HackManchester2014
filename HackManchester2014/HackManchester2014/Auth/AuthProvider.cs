using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackManchester2014.Domain;
using HackManchester2014.Infrastructure;
using Nancy;
using Nancy.SimpleAuthentication;
using Raven.Client;

namespace HackManchester2014.Auth
{
    public class AuthProvider : IAuthenticationCallbackProvider
    {
        IDocumentSession Session { get; set; }

        public AuthProvider()
        {
            Session = RavenSessionProvider.DocumentStore.OpenSession();
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            var userId = model.AuthenticatedClient.UserInformation.Id;
            var providerName = model.AuthenticatedClient.ProviderName;
            var user = Session.Query<User>().SingleOrDefault(x => x.ProviderName == providerName && x.UserInfo.Id == userId);
            if (user == null)
            {
                user = new User(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation);
                Session.Store(user);
                Session.SaveChanges();
            }
            var returnUrl = nancyModule.Session["returnUrl"].ToString();
            nancyModule.Session["returnUrl"] = null;
            return Nancy.Authentication.Forms.FormsAuthentication.UserLoggedInRedirectResponse(nancyModule.Context,
                user.Id, null, returnUrl);
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }

    }
}