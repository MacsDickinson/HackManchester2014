namespace HackManchester2014.Achievements.Models
{
    using System;
    using System.Linq;
    using HackManchester2014.Domain;
    using HackManchester2014.Infrastructure.Indexes;
    using Raven.Client;

    public class TotalDonationsInTreeOver100Rule : IAchievementRule
    {
        public bool Achieved(User user, IDocumentSession documentSession)
        {
            return false;

            if (user.Achievements != null && user.Achievements.Any(x => x.Type == AchievementType.TotalDonationsInTreeOver100))
            {
                return false;
            }

            var entries = documentSession.Query<Entry>().Where(x => x.UserId == user.Id);

            if (entries.Any())
            {
                // TODO: Replace with Matts snazzy new index
                var indexItems = documentSession.Query<EntryIndexItem>("EntryChildrenIndex").ToList()
                    .Where(x => entries.Any(y => y.Id == x.Id)).ToList();

                if (indexItems.Sum(x => x.OwnDonation) > 100)
                {
                    return true;
                }
            }
            return false;
        }

        public Achievement AwardAchievement()
        {
            return new Achievement
            {
                Name = "Total Donations In Tree Over £100",
                Description = "Congratulations, you're tree of influence has donated over £100",
                Type = AchievementType.TotalDonationsInTreeOver100,
                Awarded = DateTime.Now,
                Icon = "fa-btc"
            };
        }
    }
}