namespace HackManchester2014.Map.Models
{
    public class MapLocation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MapLocation(string title, string description, double latitude, double longitude)
        {
            Title = title;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}