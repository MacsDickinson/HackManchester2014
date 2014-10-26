using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackManchester2014.Domain;
using Raven.Client.Indexes;

namespace HackManchester2014.Infrastructure.Indexes
{
    public class UserStatsIndex : AbstractMultiMapIndexCreationTask<UserStatsIndexItem>
    {
        public UserStatsIndex()
        {
            AddMap<Entry>(entries => from entry in entries
                            from parentId in entry.ParentEntries
                            let parentEntry = LoadDocument<Entry>(parentId)
                            select new UserStatsIndexItem
                            {
                                UserId = parentEntry.UserId,
                                NominatedCount = 1,
                                DonatedCount = entry.Donation == null ? 0 : 1,
                                DonationsTotal = entry.Donation.Amount.Value,
                                OwnDonation = 0
                            });
            AddMap<Entry>(entries => from entry in entries
                                     select new UserStatsIndexItem
                                     {
                                         UserId = entry.UserId,
                                         NominatedCount = 0,
                                         DonatedCount = 0,
                                         DonationsTotal = entry.Donation.Amount.Value,
                                         OwnDonation = entry.Donation.Amount.Value
                                     });
            Reduce = results => from result in results
                                group result by result.UserId into g
                                select new UserStatsIndexItem
                                {
                                    UserId = g.Key,
                                    NominatedCount = g.Sum(x=>x.NominatedCount),
                                    DonatedCount = g.Sum(x=>x.DonatedCount),
                                    DonationsTotal = g.Sum(x=>x.DonationsTotal),
                                    OwnDonation = g.Sum(x => x.OwnDonation)
                                };
        }
    }

    public class UserStatsIndexItem
    {
        public Guid UserId { get; set; }
        public int NominatedCount { get; set; }
        public int DonatedCount { get; set; }
        public Decimal DonationsTotal { get; set; }
        public Decimal OwnDonation { get; set; }
    }
}