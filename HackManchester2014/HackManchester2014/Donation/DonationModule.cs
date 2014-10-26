using HackManchester2014.Infrastructure;
using HackManchester2014;
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
    public class DonationModule: NancyModule
    {
        public DonationModule(IDocumentSession session, JustGivingConfiguration justGivingConfig, IImageStore imageStore)
        {
            this.RequiresAuthentication();

            Get[@"/challenges/{challengeTag}/donate"] = _ =>
            {
                var user = (UserIdentity)Context.CurrentUser;

                string challengeTag = _.challengeTag;
                var challenge = session.Load<Domain.Challenge>(string.Format("challenges/{0}", challengeTag));

                var entry = new Entry(user.User, challenge, Context.GetGeoIp());

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

                var entry = session
                    .Include<Entry>(x => x.ChallengeId)
                    .Include<Entry>(x => x.UserId)
                    .Load(entryId);
                var challenge = session.Load<Domain.Challenge>(entry.ChallengeId);
                var user = session.Load<Domain.User>(entry.UserId);

                var nomination = new Domain.Nomination(string.Format("nominations/kLw{0:00000}IaQ", entryId), entry, challenge, user);
                session.Store(nomination);
                entry.Donation = donation;
                entry.Nominations.Add(nomination);
                session.SaveChanges();

                var nominationUrl = new UriBuilder(Request.Url.ToString());
                nominationUrl.Path = nomination.Id;

                return new RedirectResponse(string.Format("/challenges/{0}/entries/{1}/upload", challengeTag, entryId));
            };

            Get["/challenges/{challengeTag}/entries/{entryId}/upload"] = _ =>
            {
                var viewModel = new Models.ConfirmationViewModel
                {

                };

                return Negotiate
                    .WithModel(viewModel)
                    .WithView("register4");
            };

            Post["/challenges/{challengeTag}/entries/{entryId}/upload"] = _ =>
            {
                string challengeTag = _.challengeTag;
                int entryId = _.entryId;
                var entry = session.Load<Entry>(string.Format("entries/{0}", entryId));
                var httpFile = Request.Files.FirstOrDefault();
                if (httpFile != null)
                {
                    var id = imageStore.SaveImage(httpFile.Value);
                    var image = new Image()
                    {
                        Id = id,
                        ContentType = httpFile.ContentType,
                        Name = httpFile.Name
                    };
                    entry.ProofImage = image;
                    session.Store(entry);
                }
                return Response.AsRedirect(string.Format("/challenges/{0}/entries/{1}/nominate", challengeTag, entryId));
            };
        }
    }
}