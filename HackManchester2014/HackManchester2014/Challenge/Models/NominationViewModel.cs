using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Challenge.Models
{
    public class NominationViewModel
    {
        public string Id { get; set; }
        public string ChallengeTitle { get; set; }
        public string CharityName { get; set; }

        public string NominatedById { get; set; }
        public string NominatedByName { get; set; }
    }
}