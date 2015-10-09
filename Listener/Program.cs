using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;

namespace Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Ctrl+C to exit.");
            ReceiveMessages().Wait();
        }

        private static async Task ReceiveMessages()
        {
            var client = await GetQueueClientAsync();
            while (true)
            {
                try
                {
                    var message = await client.ReceiveAsync(TimeSpan.MaxValue);
                    await message.CompleteAsync();
                    Console.WriteLine("{0} Received {1}", DateTime.Now, message.GetBody<Guid>());
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    client = await GetQueueClientAsync();
                }
            }
        }

        private static async Task<QueueClient> GetQueueClientAsync()
        {
            var sharedAccessSignature = await GetTokenAsync();

            var address = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["Namespace"], string.Empty);
            var messagingFactory = MessagingFactory.Create(address, TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessSignature));
            return messagingFactory.CreateQueueClient(ConfigurationManager.AppSettings["QueueName"]);
        }

        private static async Task<string> GetTokenAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetStringAsync(ConfigurationManager.AppSettings["ReadTokenUrl"]);
            var jObject = JObject.Parse(response);
            return jObject.GetValue("SharedAccessSignature").ToString();
        }
    }
}
