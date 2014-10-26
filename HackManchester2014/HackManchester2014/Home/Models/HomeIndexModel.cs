using System.Collections.Generic;
using HackManchester2014.Domain;

namespace HackManchester2014.Home.Models
{
    using System;
    using HackManchester2014.Map.Models;

    public class HomeIndexModel
    {
        public Decimal TotalDonations { get; set; }
        public int TotalChallenges { get; set; }
        public MapViewModel MapModel { get; set; }
        public List<Entry> Entries { get; set; }
        public List<Achievement> NewAchievements { get; set; }

        public HomeIndexModel()
        {
            NewAchievements = new List<Achievement>();
        }
    }
}