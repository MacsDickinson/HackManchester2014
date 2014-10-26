namespace HackManchester2014.Domain
{
    using System;

    public class Achievement
    {
        public string Id { get; set; }
        public AchievementType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime Awarded { get; set; }
    }
}