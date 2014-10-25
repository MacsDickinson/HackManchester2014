using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Donation
{
    public class JustGivingUrlBuilder
    {
        public bool Live { get; set; }
        public int CharityId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string ExitUrl { get; set; }

        public override string ToString()
        {
            var queryString = string.Join("&", new Dictionary<string, string>
            {
                { "reference", Reference },
                { "amount", Amount.ToString() },
                { "exiturl", ExitUrl }
            }.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value))));

            var uri = new UriBuilder
            {
                Scheme = "https",
                Host = string.Format("{0}justgiving.com", Live ? string.Empty : "v3-sandbox."),
                Path = string.Format("/4w350m3/donation/direct/charity/{0}/", CharityId),
                Query = queryString
            };
            return uri.ToString();
        }
    }
}