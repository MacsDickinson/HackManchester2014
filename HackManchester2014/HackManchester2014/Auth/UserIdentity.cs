using System;
using System.Collections.Generic;
using HackManchester2014.Domain;
using Nancy.Security;

namespace HackManchester2014.Auth
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(User user)
        {
            UserName = user.UserInfo.UserName;
            User = user;
        }

        public string UserName { get; private set; }
        public User User { get; private set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}