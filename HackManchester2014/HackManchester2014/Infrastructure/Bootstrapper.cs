using System.Reflection;
using HackManchester2014.Auth;
using Nancy;
using Nancy.Authentication.Forms;
using Raven.Client.Indexes;

namespace HackManchester2014.Infrastructure
{
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Cryptography;
    using Nancy.Session;
    using Nancy.TinyIoc;
    using Raven.Client;
    using System.Configuration;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var store = RavenSessionProvider.DocumentStore;
            container.Register<IDocumentStore>(store);
            container.Register<JustGivingConfiguration>(new JustGivingConfiguration
            {
                ApiHost = ConfigurationManager.AppSettings["JustGiving.ApiHost"],
                ApiKey = ConfigurationManager.AppSettings["JustGiving.ApiKey"],
                WebsiteHost = ConfigurationManager.AppSettings["JustGiving.WebsiteHost"],
            });
            // Custom view locations
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(context.ModuleName, "/Views/", viewName));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var store = container.Resolve<IDocumentStore>();
            container.Register(store.OpenSession());
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            StaticConfiguration.DisableErrorTraces = false;

            var cryptographyConfiguration =
                new CryptographyConfiguration(
                    new RijndaelEncryptionProvider(new PassphraseKeyGenerator("SuperSecretPass",
                        new byte[]
                        {
                            100, 111, 110, 116, 32, 109, 97, 107, 101, 32, 109, 101, 32, 108, 111, 103, 32, 105, 110, 32,
                            101, 97, 99, 104, 32, 116, 105, 109, 101, 32, 105, 32, 98, 117, 105, 108, 100, 33
                        })),
                    new DefaultHmacProvider(new PassphraseKeyGenerator("UberSecretPass",
                        new byte[]
                        {
                            90, 71, 57, 117, 100, 67, 66, 116, 89, 87, 116, 108, 73, 71, 49, 108, 73, 71, 120, 118, 90, 121,
                            66, 112, 98, 105, 66, 108, 89, 87, 78, 111, 73, 72, 82, 112, 98, 87, 85, 103, 97, 83, 66,
                            105, 100, 87, 108, 115, 90, 67, 69, 61
                        })));
             
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = new UserMapper(container.Resolve<IDocumentSession>()),
                    CryptographyConfiguration = cryptographyConfiguration
                };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                var documentSession = container.Resolve<IDocumentSession>();

                if (ctx.Response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    documentSession.SaveChanges();
                }

                documentSession.Dispose();
            });
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // Custom static file conventions
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("public"));
        }
    }
}