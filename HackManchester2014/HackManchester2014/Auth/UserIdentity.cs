using System;
using System.Collections.Generic;
using Nancy.Security;

namespace HackManchester2014.Auth
{
    public class UserIdentity : IUserIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(string username, Guid id)
        {
            UserName = username;
            Id = id;
        }

        public string UserName { get; private set; }
        public Guid Id { get; set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}