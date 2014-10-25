using Nancy;

namespace HackManchester2014.Authentication
{
    public class AuthenticationModule : NancyModule
    {
        public AuthenticationModule()
            : base("/Authentication")
        {
            Get["/Callback"] = _ => "Thanks!";
        }
    }
}