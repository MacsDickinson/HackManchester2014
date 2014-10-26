namespace HackManchester2014.Map.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using GeoJSON.Net.Feature;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MapDonation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MapLocation Location { get; set; }
        public List<MapDonation> Nominations { get; set; }

        public MapDonation(string title, string description, double latitude, double longitude)
        {
            Title = title;
            Description = description;
            Location = new MapLocation(latitude, longitude);
            Nominations = new List<MapDonation>();
        }

        public string DonationGeoJson
        {
            get
            {
                var point = new GeoJSON.Net.Geometry.Point(new GeoJSON.Net.Geometry.GeographicPosition(Location.Latitude, Location.Longitude));

                var featureProperties = new Dictionary<string, object>
                    {
                        {"title", Title},
                        {"description", Description},
                        {"marker-size", "small"},
                        {"marker-color", "#32A07E"},
                        {"marker-symbol", "star"}
                    };
                var feature = new Feature(point, featureProperties);

                return SerializedData(feature);
            }
        }

        public string NominationsGeoJson
        {
            get
            {
                var posts = new FeatureCollection(Nominations.Select(x =>
                {
                    var point =
                        new GeoJSON.Net.Geometry.Point(new GeoJSON.Net.Geometry.GeographicPosition(x.Location.Latitude,
                            x.Location.Longitude));

                    var featureProperties = new Dictionary<string, object>
                    {
                        {"title", x.Title},
                        {"description", x.Description},
                        {"marker-size", "small"},
                        {"marker-color", "#32A07E"},
                        {"marker-symbol", "star"}
                    };
                    return new Feature(point, featureProperties);
                }).ToList());

                return SerializedData(posts);
            }
        }

        private static string SerializedData(object toSerialize)
        {
            var serializedData = JsonConvert.SerializeObject(toSerialize, Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            return serializedData;
        }
    }
}