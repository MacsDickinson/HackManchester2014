using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }
}