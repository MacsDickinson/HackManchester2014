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
                                     from parentId in entry.ParentEntries
                                     let parentEntry = LoadDocument<Entry>(parentId)
                            select new
                            {
                                Id = parentId, 
                                UserId = parentEntry.UserId,
                                Decendants = 1, 
                                TotalDonations = entry.Donation.Amount,
                                OwnDonation = 0
                            });
            AddMap<Entry>(entries => from entry in entries
                                     select new
                                     {
                                         Id = entry.Id, 
                                         UserId = entry.UserId,
                                         Decendants = 0,
                                         TotalDonations = entry.Donation.Amount,
                                         OwnDonation = entry.Donation.Amount
                                     });
            Reduce = results => from result in results
                                group result by new{result.Id, result.UserId} into g
                                select new
                                {
                                    Id = g.Key.Id,
                                    UserId = g.Key.UserId, 
                                    Decendants = g.Sum(x => x.Decendants),
                                    TotalDonations = g.Sum(x => x.TotalDonations),
                                    OwnDonation = g.Sum(x => x.OwnDonation)
                                };
        }
    }

    public class EntryIndexItem
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public int Decendants { get; set; }
        public Decimal TotalDonations { get; set; }
        public Decimal OwnDonation { get; set; }
    }
}