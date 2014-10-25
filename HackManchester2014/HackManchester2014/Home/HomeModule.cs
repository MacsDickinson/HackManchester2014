namespace HackManchester2014.Home
{
    using Nancy;
    using Raven.Client;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession)
        {
            Get["/"] = _ => "Hello!, go to /challenges/mind/donate to donate";
        }
    }
}