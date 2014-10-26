using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
    public class Nomination
    {
        public string Id { get; private set; }

        public string NominatedByEntryId { get; set; }
        public string NominatedById { get; set; }
        public string NominatedByName { get; set; }

        public string ChallengeId { get; set; }
        public string ChallengeTitle { get; set; }
        public string CharityName { get; set; }

        private Nomination() { }
        public Nomination(string id, Entry parentEntry, Challenge challenge, User nominatedBy)
        {
            Id = id;
            NominatedByEntryId = parentEntry.Id;

            NominatedById = string.Format("users/{0}", nominatedBy.Id);
            NominatedByName = nominatedBy.UserInfo.Name;

            ChallengeId = challenge.Id;
            ChallengeTitle = challenge.Title;
            CharityName = challenge.CharityName;
        }
    }
}