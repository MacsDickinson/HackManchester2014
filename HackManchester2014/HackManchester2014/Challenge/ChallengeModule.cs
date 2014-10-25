using Nancy;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Challenge
{
    public class ChallengeModule : NancyModule
    {
        public ChallengeModule(IDocumentSession session)
        {
            Get["/challenges"] = _ =>
            {
                var challenges = session.Query<Domain.Challenge>().Take(10);

                var viewModel = new Models.ChallengesViewModel
                {
                    Challenges = challenges.Select(x => new Models.ChallengeViewModel
                    {
                        Title = x.Title,
                        Charity = x.CharityName,
                        Id = x.Id,
                    })
                };

                return Negotiate
                    .WithModel(viewModel)
                    .WithView("challenges");
            };

            Get["/challenges/{challengeTag}"] = _ =>
            {
                string challengeTag = _.challengeTag;
                var challenge = session.Load<Domain.Challenge>(string.Format("challenges/{0}", challengeTag));

                var viewModel = new Models.ChallengeViewModel
                {
                    Id = challenge.Id,
                    Title = challenge.Title,
                    Charity = challenge.CharityName,
                };

                return Negotiate
                    .WithModel(viewModel)
                    .WithView("challenge");
            };
        }
    }
}