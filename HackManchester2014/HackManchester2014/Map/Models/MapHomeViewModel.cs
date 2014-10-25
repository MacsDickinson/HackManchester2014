namespace HackManchester2014.Map.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using GeoJSON.Net.Feature;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MapHomeViewModel
    {
        public MapLocation MyLocation { get; set; }
        public List<MapLocation> Donations { get; set; }

        public string DonationsJson
        {
            get
            {
                var posts = new FeatureCollection(Donations.Select(x =>
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

                var serializedData = JsonConvert.SerializeObject(posts, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });

                return serializedData;
            }
        }
    }
}