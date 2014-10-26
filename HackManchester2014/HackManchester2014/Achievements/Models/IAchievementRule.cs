namespace HackManchester2014.Achievements.Models
{
    using HackManchester2014.Domain;
    using Raven.Client;

    public interface IAchievementRule
    {
        bool Achieved(User user, IDocumentSession documentSession);
        Achievement AwardAchievement();
    }
}