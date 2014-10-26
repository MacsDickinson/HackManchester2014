namespace HackManchester2014.Domain
{
    public class GeoIp
    {
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string metro_code { get; set; }
        public string area_code { get; set; }
    }
}