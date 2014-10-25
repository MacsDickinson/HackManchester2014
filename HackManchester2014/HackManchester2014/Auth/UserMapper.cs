using System;
using System.Linq;
using System.Web;
using HackManchester2014.Domain;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Raven.Client;

namespace HackManchester2014.Auth
{
    public class UserMapper : IUserMapper
    {
        public IDocumentSession Session { get; set; }

        public UserMapper(IDocumentSession session)
        {
            Session = session;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = Session.Load<User>(identifier);
            return user == null ? null : new UserIdentity(user);
        }
    }
}