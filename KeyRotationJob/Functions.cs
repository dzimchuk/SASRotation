using System;
using System.Configuration;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace KeyRotationJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void RegenerateKey(TextWriter log)
        {
            var manager = NamespaceManager.CreateFromConnectionString(ConfigurationManager.AppSettings["ServiceBusConnectionString"]);
            var description = manager.GetQueue(ConfigurationManager.AppSettings["QueueName"]);

            RegenerateKey(description, ConfigurationManager.AppSettings["ReadAuthorizationRuleName"], log);
            RegenerateKey(description, ConfigurationManager.AppSettings["WriteAuthorizationRuleName"], log);

            manager.UpdateQueue(description);
        }

        private static void RegenerateKey(QueueDescription description, string ruleName, TextWriter log)
        {
            SharedAccessAuthorizationRule rule;
            if (!description.Authorization.TryGetSharedAccessAuthorizationRule(ruleName, out rule))
                throw new Exception($"Authorization rule {ruleName} was not found");

            rule.SecondaryKey = rule.PrimaryKey;
            rule.PrimaryKey = SharedAccessAuthorizationRule.GenerateRandomKey();

            log.WriteLine($"Authorization rule: {ruleName}\nPrimary key: {rule.PrimaryKey}\nSecondary key: {rule.SecondaryKey}");
        }
    }
}
