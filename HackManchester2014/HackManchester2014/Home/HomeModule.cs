namespace HackManchester2014.Home
{
    using System.Collections.Generic;
    using GeoJSON.Net.Feature;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Raven.Client;
    using System.Linq;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession)
        {
            Get["/"] = _ => "Hello!";

            Get["/Map"] = _ => Negotiate.WithView("Map");

            Get["/MapData"] = _ =>
            {
                var posts = new FeatureCollection(TestLocations().Select(x =>
                {
                    var point = new GeoJSON.Net.Geometry.Point(new GeoJSON.Net.Geometry.GeographicPosition(x.Latitude, x.Longitude));

                    var featureProperties = new Dictionary<string, object>
                    {
                        { "title", x.Title },
                        { "description", x.Description },
                        { "marker-size", "large" },
                        { "marker-color", "#6FCAC5" },
                        { "marker-symbol", "star" }
                    };
                    return new Feature(point, featureProperties);
                }).ToList());

                var serializedData = JsonConvert.SerializeObject(posts, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore });

                return serializedData;
            };
        }

        public List<Location> TestLocations()
        {
            return new List<Location>();
        }
    }

    public class Location
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}