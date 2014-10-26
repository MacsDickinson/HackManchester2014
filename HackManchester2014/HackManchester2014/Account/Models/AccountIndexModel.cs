namespace HackManchester2014.Account.Models
{
    using System;
    using HackManchester2014.Map.Models;

    public class AccountIndexModel
    {
        public Decimal TotalDonated { get; set; }
        public int TotalDonations { get; set; }
        public Decimal TotalRaised { get; set; }
        public int TotalNominated { get; set; }
        public MapViewModel MapModel { get; set; }
    }
}