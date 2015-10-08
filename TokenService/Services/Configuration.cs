using System.Configuration;

namespace TokenService.Services
{
    internal class Configuration : IConfiguration
    {
        public string Find(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}