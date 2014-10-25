namespace HackManchester2014.Home
{
    using Nancy;
    using Raven.Client;

    public class HomeModule : NancyModule
    {
        public HomeModule(IDocumentSession documentSession)
        {
            Get["/"] = _ => Negotiate.WithView("Index");
            Get["/register"] = _ => Response.AsRedirect("/register/1");
            Get["/register/1"] = _ =>
            {
                Session["returnUrl"] = "/register/2";
                return Negotiate.WithView("Register1");
            };
            Get["/register/2"] = _ =>
            {
                return Negotiate.WithView("Register2");
            };
            Get["/register/3"] = _ =>
            {
                return Negotiate.WithView("Register3");
            };
            Get["/register/4"] = _ =>
            {
                return Negotiate.WithView("Register4");
            };
        }
    }
}