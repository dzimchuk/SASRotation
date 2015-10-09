using System.Web.Http;
using LightInject;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TokenService.Startup))]

namespace TokenService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            InitializeLightInject(config);
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }

        private static void InitializeLightInject(HttpConfiguration config)
        {
            var container = new ServiceContainer();
            container.RegisterFrom<Composition.CompositionModule>();
            container.RegisterApiControllers();
            container.EnableWebApi(config);
        }
    }
}
