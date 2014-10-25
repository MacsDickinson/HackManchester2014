using Nancy.Security;

namespace HackManchester2014.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using GeoJSON.Net.Feature;
    using HackManchester2014.Map.Models;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MapModule : NancyModule
    {
        public MapModule()
            : base("Map")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var locations = TestLocations();
                var model = new MapHomeViewModel
                {
                    MyLocation = locations.First(),
                    Donations = locations
                };
                return Negotiate.WithView("Map")
                    .WithModel(model);
            };
        }

        public List<MapLocation> TestLocations()
        {
            return new List<MapLocation>
            {
                new MapLocation("Matt Smith", "Matt donated £15 to his mum", 53.477179333058984, -2.254300117492676),
                new MapLocation("Ashley Izat", "Ashley donated £10 to Matts mum", 53.476719596835295, -2.253549098968506),
                new MapLocation("Macs Dickinson", "Macs donated £9 to Matts mum", 53.476362020773145, -2.2513389587402344),
                new MapLocation("Lynden Oliver", "Lynden donated £27 to Matts mum", 53.47752413195797, -2.252669334411621)
            };
        }
    }
}