using LightInject;
using TokenService.Services;

namespace TokenService.Composition
{
    internal class CompositionModule : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IConfiguration, Configuration>();
            serviceRegistry.Register<ITokenService, Services.TokenService>();
        }
    }
}