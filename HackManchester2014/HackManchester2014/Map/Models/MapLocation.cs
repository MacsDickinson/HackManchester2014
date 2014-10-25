namespace HackManchester2014.Map.Models
{
    public class MapLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MapLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}