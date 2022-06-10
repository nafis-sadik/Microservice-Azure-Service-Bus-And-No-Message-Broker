using Microservice_C.Models;
using Microservice_C.Services;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Microservice_C
{
    public class ServiceBusSubscriptionClient : BackgroundService
    {
        private readonly ISubscriptionClient _subscriptionClient;
        private readonly IAzureServiceBusClient _messagePublisher;
        public ServiceBusSubscriptionClient(ISubscriptionClient subscriptionClient, IAzureServiceBusClient messagePublisher)
        {
            _subscriptionClient = subscriptionClient;
            _messagePublisher = messagePublisher;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptionClient.RegisterMessageHandler((msg, token) =>
            {
                var recievedObject = JsonConvert.DeserializeObject<WeatherModel>(Encoding.UTF8.GetString(msg.Body));

                Console.WriteLine("Temperature recieved (C) : {0}", recievedObject.temperatureC);
                Console.WriteLine("Temperature recieved (F) : {0}", recievedObject.temperatureF);

                recievedObject.temperatureC = new Random().Next(0, 50);
                recievedObject.temperatureF = (recievedObject.temperatureC * 9 / 5) + 32;

                _messagePublisher.RequestTopic(obj: recievedObject);
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
