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

        public List<Nomination> Nominations { get; set; }

        public List<string> ParentEntries { get; private set; }
        public string ParentEntry { get { return ParentEntries.LastOrDefault(); } }

        public GeoIp GeoIp { get; set; }

        private Entry()
        {
            Nominations = new List<Nomination>();
        }

        public Entry(User user, Challenge challenge, GeoIp geoIp, Entry parentEntry = null)
        {
            UserId = string.Format("users/{0}", user.Id);
            UserName = user.UserInfo.Name;
            ChallengeId = challenge.Id;
            ChallengeTitle = challenge.Title;
            Nominations = new List<Nomination>();
            GeoIp = geoIp;
            if (parentEntry != null)
            {
                ParentEntries = parentEntry.ParentEntries.Concat(new List<string> {parentEntry.Id}).ToList();
            }
            else
            {
                ParentEntries = new List<string>();
            }
        }

    }
}