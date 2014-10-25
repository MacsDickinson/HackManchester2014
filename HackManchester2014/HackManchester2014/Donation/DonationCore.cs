using HackManchester2014.Infrastructure;
using JustGiving.Api.Sdk;
using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Donation
{
    public class DonationCore: NancyModule
    {
        public DonationCore(JustGivingConfiguration justGivingConfig)
        {
            Get["/BeginDonate"] = _ =>
            {
                var returnUrl = new UriBuilder
                {
                    Scheme = this.Request.Url.Scheme,
                    Host = this.Request.Url.HostName,
                    Port = this.Request.Url.Port ?? 80,
                    Path = "/ConfirmDonate",
                    Query = "DonationId=JUSTGIVING-DONATION-ID"
                }.ToString();

                var url = new JustGivingUrlBuilder
                {
                    Host = justGivingConfig.WebsiteHost,
                    CharityId = 300,
                    Amount = 3.14M,
                    Reference = "Matts First & Test",
                    ExitUrl = returnUrl
                }.ToString();
                
                return new RedirectResponse(url);
            };

            Get["/ConfirmDonate"] = _ =>
            {
                var c = new JustGivingClient(new ClientConfiguration(string.Format("https://{0}/", justGivingConfig.ApiHost), justGivingConfig.ApiKey, 3));

                int donationId = Context.Request.Query.DonationId;

                var donation = c.Donation.Retrieve(donationId);
                return JsonConvert.SerializeObject(donation, Formatting.Indented);
            };
        }
    }
}