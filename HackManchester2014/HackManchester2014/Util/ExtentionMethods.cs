using System.Runtime.InteropServices.ComTypes;
using HackManchester2014.Auth;
using HackManchester2014.Domain;
using Nancy;
using Nancy.ViewEngines.Razor;
using RestSharp;

namespace HackManchester2014
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

        public static GeoIp GetGeoIp(this NancyContext context)
        {
            var client = new RestClient("http://http://freegeoip.net/json/");
            var request = new RestRequest(context.Request.UserHostAddress);
            var response = client.Execute<GeoIp>(request);
            return response.Data;
        }
    }
}