using System;

namespace HackManchester2014.Domain
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
        }

        public User(string providerName, SimpleAuthentication.Core.UserInformation userInformation)
        {
            UserInfo = userInformation;
            ProviderName = providerName;
        }
        public Guid Id { get; set; }
        public string ProviderName { get; set; }
        public SimpleAuthentication.Core.UserInformation UserInfo { get; set; }
        public List<Achievement> Achievements { get; set; }

    }
}