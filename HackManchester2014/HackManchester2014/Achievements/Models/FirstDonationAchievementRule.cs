namespace HackManchester2014.Achievements.Models
{
    using System;
    using System.Linq;
    using HackManchester2014.Domain;
    using Raven.Client;

    public class FirstDonationAchievementRule : IAchievementRule
    {
        public bool Achieved(User user, IDocumentSession documentSession)
        {
            return user.Achievements.Any(x => x.Type == AchievementType.FirstDonation)
                && documentSession.Query<Entry>().Any(x => x.UserId == user.Id && x.Donation != null);
        }

        public Achievement AwardAchievement()
        {
            return new Achievement
            {
                Name = "First Donation",
                Description = "Congratulations, you have made your first donation.",
                Type = AchievementType.FirstDonation,
                Icon = "fa-bolt",
                Awarded = DateTime.Now
            };
        }
    }
}