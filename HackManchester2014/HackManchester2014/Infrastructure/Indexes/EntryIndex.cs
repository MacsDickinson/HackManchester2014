using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackManchester2014.Domain;
using Raven.Client.Indexes;

namespace HackManchester2014.Infrastructure.Indexes
{
    public class EntryChildrenIndex : AbstractMultiMapIndexCreationTask<EntryIndexItem>
    {
        public EntryChildrenIndex()
        {
            AddMap<Entry>(entries => from entry in entries
                            from Id in entry.ParentEntries
                            select new
                            {
                                Id, 
                                Decendants = 1, 
                                TotalDonations = entry.Donation.Amount,
                                OwnDonation = 0
                            });
            AddMap<Entry>(entries => from entry in entries
                                     select new
                                     {
                                         Id = entry.Id, 
                                         Decendants = 0,
                                         TotalDonations = entry.Donation.Amount,
                                         OwnDonation = entry.Donation.Amount
                                     });
            Reduce = results => from result in results
                                group result by result.Id into g
                                select new
                                {
                                    Id = g.Key, 
                                    Decendants = g.Sum(x => x.Decendants),
                                    TotalDonations = g.Sum(x => x.TotalDonations),
                                    OwnDonation = g.Sum(x => x.OwnDonation)
                                };
        }
    }

    public class EntryIndexItem
    {
        public string Id { get; set; }
        public int Decendants { get; set; }
        public Decimal TotalDonations { get; set; }
        public Decimal OwnDonation { get; set; }
    }
}