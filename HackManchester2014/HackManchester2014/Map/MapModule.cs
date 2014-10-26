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
            Get["/"] = _ =>
            {
                var model = new MapViewModel
                {
                    Donation = TestDonation()
                };
                return Negotiate.WithView("Map")
                                .WithModel(model);
            };
        }

        public static MapDonation TestDonation()
        {
            var topDonation = new MapDonation("Macs Dickinson", "Macs donated £9 to Matts mum", 53.476362020773145,
                -2.2513389587402344);

            AddNominations(new Random(), topDonation, 0);

            return topDonation;
        }

        private static void AddNominations(Random random, MapDonation donation, int depth)
        {
            for (double j = 0; j < random.Next(0, 5); j++)
            {
                donation.Nominations.Add(RandomChild(donation, random));
            }
            if (depth < 5)
            {
                foreach (var nomination in donation.Nominations)
                {
                    AddNominations(random, nomination, depth + 1);
                }
            }
        }

        private static MapDonation RandomChild(MapDonation parent, Random random)
        {
            return new MapDonation(parent.Title + " Child", parent.Title + " Child", (parent.Location.Latitude + RandomDouble(random)),
                parent.Location.Longitude + RandomDouble(random));
        }

        public static double RandomDouble(Random random)
        {
            const double maximum = 0.1;
            const double minimum = -0.1;
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}