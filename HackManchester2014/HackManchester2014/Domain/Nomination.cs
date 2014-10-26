using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
    public class Nomination
    {
        public string Id { get; set; }
        public string NominatedByEntryId { get; set; }
    }
}