using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackManchester2014.Domain
{
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


    }
}