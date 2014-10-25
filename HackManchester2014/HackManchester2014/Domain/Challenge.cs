using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
    public class Challenge
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string CharityName { get; set; }
        public int JustGivingCharityId { get; set; }
        
    }
}