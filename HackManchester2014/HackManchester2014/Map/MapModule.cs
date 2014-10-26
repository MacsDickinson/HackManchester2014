using System.Collections.Generic;
using System.Linq;
using HackManchester2014.Domain;
using Nancy.Security;
using Raven.Client;

namespace HackManchester2014.Map
{
    using System;
    using HackManchester2014.Map.Models;
    using Nancy;

    public class MapModule : NancyModule
    {
        public MapModule(IDocumentSession documentSession)
            : base("Map")
        {
            Get["/"] = _ =>
            {
                var seed = new Random().Next(1,2048);
                var model = new MapViewModel
                {
                    Donation = TestDonation(seed),
                    I = seed
                };
                return Negotiate.WithView("Map")
                                .WithModel(model);
            };
        }

        public static MapDonation BuildTree(Entry root, List<Entry> decendants)
        {
            var children = decendants.Where(x => x.ParentEntry == root.Id);
            var childrenModels = children.Select(x => BuildTree(x, decendants));
            if (root.GeoIp != null)
            {
                var donation = new MapDonation(root.UserName, root.ChallengeTitle, root.GeoIp.latitude, root.GeoIp.longitude);
                donation.Nominations.AddRange(childrenModels);
                return donation;
            }
            return null;
        }


        public static MapDonation TestDonation(int seed)
        {
            var topDonation = new MapDonation("Macs Dickinson", "Macs donated £9 to Matts mum", 53.476362020773145,
                -2.2513389587402344);

            AddNominations(new Random(seed), topDonation, 0);

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
            const double maximum = 0.2;
            const double minimum = -0.2;
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}