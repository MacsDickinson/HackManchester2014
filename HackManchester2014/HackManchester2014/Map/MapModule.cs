using Nancy;

namespace HackManchester2014.Home
{
    using System.Collections.Generic;
    using System.Linq;
    using GeoJSON.Net.Feature;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MapModule : NancyModule
    {
        public MapModule()
            : base("Map")
        {

            Get["/"] = _ => Negotiate.WithView("Map");

            Get["/Data"] = _ =>
            {
                var posts = new FeatureCollection(TestLocations().Select(x =>
                {
                    var point =
                        new GeoJSON.Net.Geometry.Point(new GeoJSON.Net.Geometry.GeographicPosition(x.Latitude,
                            x.Longitude));

                    var featureProperties = new Dictionary<string, object>
                    {
                        {"title", x.Title},
                        {"description", x.Description},
                        {"marker-size", "large"},
                        {"marker-color", "#6FCAC5"},
                        {"marker-symbol", "star"}
                    };
                    return new Feature(point, featureProperties);
                }).ToList());

                var serializedData = JsonConvert.SerializeObject(posts, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });

                return serializedData;
            };
        }

        public List<Location> TestLocations()
        {
            return new List<Location>()
            {
                new Location("Matt Smith", "Matt donated £15 to his mum", 53.477179333058984, -2.254300117492676),
                new Location("Ashley Izat", "Ashley donated £10 to Matt's mum", 53.476719596835295, -2.253549098968506),
                new Location("Macs Dickinson", "Macs donated £9 to Matt's mum", 53.476362020773145, -2.2513389587402344),
                new Location("Lynden Oliver", "Lynden donated £27 to Matt's mum", 53.47752413195797, -2.252669334411621)
            };
        }
    }

    public class Location
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(string title, string description, double latitude, double longitude)
        {
            Title = title;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}