using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Donation
{
    public class DonationCore: NancyModule
    {
        public DonationCore()
        {
            Get["/BeginDonate"] = _ =>
            {
                var returnUrl = new UriBuilder
                {
                    Scheme = this.Request.Url.Scheme,
                    Host = this.Request.Url.HostName,
                    Path = "/ConfirmDonate",
                    Query = "DonationId=JUSTGIVING-DONATION-ID"
                }.ToString();

                var url = new JustGivingUrlBuilder
                {
                    Live = false,
                    CharityId = 300,
                    Amount = 3.14M,
                    Reference = "Matts First & Test",
                    ExitUrl = returnUrl
                }.ToString();
                
                return new RedirectResponse(url);
            };
        }
    }
}