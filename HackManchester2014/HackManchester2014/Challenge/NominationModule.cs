using Nancy;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Challenge
{
    public class NominationModule : NancyModule
    {
        public NominationModule(IDocumentSession session)
        {
            Get["/nominations/{nominationId}"] = _ =>
            {
                string nominationId = _.nominationId;
                var nomination = session.Load<Domain.Nomination>(string.Format("nominations/{0}", nominationId));
                var viewModel = new Models.NominationViewModel
                {
                    Id = nomination.Id,
                    ChallengeTitle = nomination.Id,
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
                var user = Context.GetUser();

                //string challengeTag = _.challengeTag;
                //var challenge = session.Load<Domain.Challenge>(string.Format("challenges/{0}", challengeTag));

                //var entry = new Entry(user.User, challenge);

                //session.Store(entry);
                //session.SaveChanges();

                //var returnUrl = new UriBuilder
                //{
                //    Scheme = this.Request.Url.Scheme,
                //    Host = this.Request.Url.HostName,
                //    Port = this.Request.Url.Port ?? 80,
                //    Path = string.Format("/{0}/{1}/confirm-JUSTGIVING-DONATION-ID", challenge.Id, entry.Id)
                //}.ToString();

                //var url = new JustGivingUrlBuilder
                //{
                //    Host = justGivingConfig.WebsiteHost,
                //    CharityId = 300,
                //    Amount = 3.14M,
                //    Reference = entry.Id,
                //    ExitUrl = returnUrl
                //}.ToString();

                //return new RedirectResponse(url);
                return null;
            };
        }
    }
}