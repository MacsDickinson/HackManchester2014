using System.Runtime.InteropServices.ComTypes;
using HackManchester2014.Auth;
using HackManchester2014.Domain;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace HackManchester2014.Util
{
    public static class ExtentionMethods
    {
        public static User GetUser(this NancyContext context)
        {
            return context.CurrentUser != null ? ((UserIdentity) context.CurrentUser).User : null;
        }

        public static User GetUser<T>(this HtmlHelpers<T> helper)
        {
            return helper.CurrentUser != null ? ((UserIdentity)helper.CurrentUser).User : null;
        }
    }
}