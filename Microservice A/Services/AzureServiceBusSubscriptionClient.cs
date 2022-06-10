using Microservice_A.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Microservice_A.Services
{
    public class AzureServiceBusSubscriptionClient: BackgroundService
    {
        private readonly ISubscriptionClient _subscriptionClient;
        public AzureServiceBusSubscriptionClient(ISubscriptionClient subscriptionClient)
        {
            _subscriptionClient = subscriptionClient;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptionClient.RegisterMessageHandler((msg, token) =>
            {
                WeatherModel? recievedObject = JsonConvert.DeserializeObject<WeatherModel>(Encoding.UTF8.GetString(msg.Body));
                if (recievedObject != null)
                {
                    Console.WriteLine("Temperature recieved (C) : {0}", recievedObject.temperatureC);
                    Console.WriteLine("Temperature recieved (F) : {0}", recievedObject.temperatureF);
                }

                return _subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);
            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 5,
            });

            return Task.CompletedTask;
        }
    }
}
