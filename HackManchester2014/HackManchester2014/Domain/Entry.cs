﻿using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HackManchester2014.Domain
{
    using System;

    public class Entry
    {
        public string Id { get; set; }

        public Guid UserId { get; private set; }
        public string UserName { get; set; }

        public string ChallengeId { get; set; }
        public string ChallengeTitle { get; set; }
        
        public Donation Donation { get; set; }

        public List<Nomination> Nominations { get; set; }

        public List<string> ParentEntries { get; private set; }
        public string ParentEntry { get { return ParentEntries.LastOrDefault(); } }

        public GeoIp GeoIp { get; set; }

        public Guid? ProofImage { get; set; }

        private Entry()
        {
            Nominations = new List<Nomination>();
        }

        public Entry(User user, Challenge challenge, GeoIp geoIp, Entry parentEntry = null)
        {
            UserId = user.Id;
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