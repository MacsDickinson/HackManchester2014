namespace HackManchester2014.Home.Models
{
    using System;
    using HackManchester2014.Map.Models;

    public class HomeIndexModel
    {
        public Decimal TotalDonations { get; set; }
        public int TotalChallenges { get; set; }
        public MapViewModel MapModel { get; set; }
    }
}