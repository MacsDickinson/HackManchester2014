using Nancy.Security;

namespace HackManchester2014.Map
{
    using System;
    using HackManchester2014.Map.Models;
    using Nancy;

    public class MapModule : NancyModule
    {
        public MapModule()
            : base("Map")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var model = new MapHomeViewModel
                {
                    Donation = TestDonation()
                };
                return Negotiate.WithView("Map")
                                .WithModel(model);
            };
        }

        public MapDonation TestDonation()
        {
            var topDonation = new MapDonation("Macs Dickinson", "Macs donated £9 to Matts mum", 53.476362020773145,
                -2.2513389587402344);

            var matt = new MapDonation("Matt Smith", "Matt donated £15 to his mum", 53.477179333058984, -2.254300117492676);
            var ash = new MapDonation("Ashley Izat", "Ashley donated £10 to Matts mum", 53.476719596835295, -2.253549098968506);
            var lynden = new MapDonation("Lynden Oliver", "Lynden donated £27 to Matts mum", 53.47752413195797, -2.252669334411621);

            topDonation.Nominations.Add(matt);
            topDonation.Nominations.Add(ash);
            topDonation.Nominations.Add(lynden);
            
            var random = new Random();

            foreach (var nomination in topDonation.Nominations)
            {
                for (double i = 0; i < 3; i++)
                {
                    nomination.Nominations.Add(RandomChild(nomination, random));
                }
                foreach (var n2 in nomination.Nominations)
                {
                    for (double j = 0; j < 3; j++)
                    {
                        n2.Nominations.Add(RandomChild(n2, random));
                    }
                    foreach (var n3 in nomination.Nominations)
                    {
                        for (double k = 0; k < 3; k++)
                        {
                            n3.Nominations.Add(RandomChild(n3, random));
                        }
                    }
                }
            }

            return topDonation;
        }

        private MapDonation RandomChild(MapDonation parent, Random random)
        {
            return new MapDonation(parent.Title + " Child", parent.Title + " Child", (parent.Location.Latitude + RandomDouble(random)),
                parent.Location.Longitude + RandomDouble(random));
        }

        private double RandomDouble(Random random)
        {
            const double maximum = 0.05;
            const double minimum = -0.05;
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}