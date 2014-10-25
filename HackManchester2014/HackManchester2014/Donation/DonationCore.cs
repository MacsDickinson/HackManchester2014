using HackManchester2014.Infrastructure;
using JustGiving.Api.Sdk;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using HackManchester2014.Domain;
using HackManchester2014.Auth;
using Raven.Client;

namespace HackManchester2014.Donation
{
    public class DonationCore: NancyModule
    {
        public DonationCore(IDocumentSession session, JustGivingConfiguration justGivingConfig)
        {
            this.RequiresAuthentication();

            Get[@"/challenges/{challengeTag}/donate"] = _ =>
            {
                var user = (UserIdentity)Context.CurrentUser;

                string challengeTag = _.challengeTag;
                var challenge = session.Load<Challenge>(string.Format("challenges/{0}", challengeTag));

                var entry = new Entry(user.Id, user.UserName, challenge);

                session.Store(entry);
                session.SaveChanges();

                var returnUrl = new UriBuilder
                {
                    Scheme = this.Request.Url.Scheme,
                    Host = this.Request.Url.HostName,
                    Port = this.Request.Url.Port ?? 80,
                    Path = string.Format("/{0}/{1}/confirm-JUSTGIVING-DONATION-ID", challenge.Id, entry.Id)
                }.ToString();

                var url = new JustGivingUrlBuilder
                {
                    Host = justGivingConfig.WebsiteHost,
                    CharityId = 300,
                    Amount = 3.14M,
                    Reference = entry.Id,
                    ExitUrl = returnUrl
                }.ToString();
                
                return new RedirectResponse(url);
            };

            Get["/challenges/{challengeTag}/entries/{entryId}/confirm-{donationId}"] = _ =>
            {
                var c = new JustGivingClient(new ClientConfiguration(string.Format("https://{0}/", justGivingConfig.ApiHost), justGivingConfig.ApiKey, 3));

                string challengeTag = _.challengeTag;
                int entryId = _.entryId;
                int donationId = _.donationId;

                var donation = c.Donation.Retrieve(donationId);

                var entry = session.Load<Entry>(entryId);
                entry.Donation = donation;
                session.SaveChanges();

                return "Thanks for donating";
            };
        }
    }
}