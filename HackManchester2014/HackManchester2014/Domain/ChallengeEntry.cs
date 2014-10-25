using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
    public class ChallengeEntry
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public string ChallengeId { get; set; }
        public string ChallengeTitle { get; set; }

        public DonationClass Donation { get; set; }


        public class DonationClass
        {
            public int DonationId { get; set; }
            public decimal ConfirmedDonationAmount { get; set; }
        }
    }
}