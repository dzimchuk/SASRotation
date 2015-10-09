using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TokenService.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public Task<string> GetReadSharedAccessSignature()
        {
            var ruleName = configuration.Find("ReadAuthorizationRuleName");
            return GetSharedAccessSignature(ruleName);
        }

        public Task<string> GetWriteSharedAccessSignature()
        {
            var ruleName = configuration.Find("WriteAuthorizationRuleName");
            return GetSharedAccessSignature(ruleName);
        }

        private async Task<string> GetSharedAccessSignature(string ruleName)
        {
            var queueName = configuration.Find("QueueName");

            var manager = NamespaceManager.CreateFromConnectionString(configuration.Find("ServiceBusConnectionString"));
            var description = await manager.GetQueueAsync(queueName);
            
            SharedAccessAuthorizationRule rule;
            if (!description.Authorization.TryGetSharedAccessAuthorizationRule(ruleName, out rule))
                throw new Exception($"Authorization rule {ruleName} was not found");

            var address = ServiceBusEnvironment.CreateServiceUri("sb", configuration.Find("Namespace"), string.Empty);
            var queueAddress = address + queueName;

            return SharedAccessSignatureTokenProvider.GetSharedAccessSignature(ruleName, rule.PrimaryKey, queueAddress,
                TimeSpan.FromSeconds(int.Parse(configuration.Find("SignatureExpiration"))));
        }
    }
}