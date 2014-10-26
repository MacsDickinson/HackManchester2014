using Nancy;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using HackManchester2014.Donation;
using HackManchester2014.Infrastructure;
using Nancy.Responses;

namespace HackManchester2014.Nomination
{
    public class NominationModule : NancyModule
    {
        public NominationModule(IDocumentSession session, JustGivingConfiguration justGivingConfig)
        {
            Get["/nominations/{nominationId}"] = _ =>
            {
                string nominationId = _.nominationId;
                var nomination = session.Load<Domain.Nomination>(string.Format("nominations/{0}", nominationId));
                var viewModel = new Models.NominationViewModel
                {
                    Id = nomination.Id,
                    ChallengeTitle = nomination.ChallengeTitle,
                    CharityName = nomination.CharityName,
                    NominatedById = nomination.NominatedById,
                    NominatedByName = nomination.NominatedByName
                };

                return Negotiate
                    .WithModel(viewModel)
                    .WithView("nomination");
            };

            Post["/nominations/{nominationId}/accept"] = _ =>
            {
                this.RequiresAuthentication();

                var user = Context.GetUser();
                string nominationId = _.nominationId;

                var nomination = session
                    .Include<Domain.Nomination>(x => x.ChallengeId)
                    .Include(x => x.NominatedByEntryId)
                    .Load(string.Format("nominations/{0}", nominationId));
                var challenge = session.Load<Domain.Challenge>(nomination.ChallengeId);
                var parentEntry = session.Load<Domain.Entry>(nomination.NominatedByEntryId);

                var entry = new Domain.Entry(user, challenge, Context.GetGeoIp(), parentEntry);

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
        }
    }
}