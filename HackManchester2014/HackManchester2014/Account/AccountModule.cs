using Nancy;

namespace HackManchester2014.Account
{
    using HackManchester2014.Account.Models;
    using HackManchester2014.Map;
    using HackManchester2014.Map.Models;
    using Nancy.Security;

    public class AccountModule : NancyModule
    {
        public AccountModule()
            : base ("my-account")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var model = new AccountIndexModel
                {
                    MapModel = new MapViewModel
                    {
                        Donation = MapModule.TestDonation()
                    }
                };

                return Negotiate.WithView("Account")
                    .WithModel(model);
            };
        }
    }
}