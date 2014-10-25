using Nancy;

namespace HackManchester2014.Home
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello!";
        }
    }
}