using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JG = JustGiving.Api.Sdk.Model.Donation;

namespace HackManchester2014.Domain
{
    public class Entry
    {
        public string Id { get; set; }

        public string UserId { get; private set; }
        public string UserName { get; set; }

        public string ChallengeId { get; set; }
        public string ChallengeTitle { get; set; }
        
        public JG.Donation Donation { get; set; }

        private Entry() { }
        public Entry(User user, Challenge challenge)
        {
            UserId = string.Format("users/{0}", user.Id);
            UserName = user.UserInfo.Name;
            ChallengeId = challenge.Id;
            ChallengeTitle = challenge.Title;
        }

    }
}