using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Microservice_A.Services
{
    public class AzureServiceBusPublisherClient : IAzureServiceBusPublisherClient
    {
        private readonly IQueueClient _queClient;
        private readonly ITopicClient _topicClient;
        public AzureServiceBusPublisherClient(IQueueClient queueClient, ITopicClient topicClient)
        {
            _queClient = queueClient;
            _topicClient = topicClient;
        }
        public Task Request<T>(T obj) => Request(JsonConvert.SerializeObject(obj));

        public Task Request(string raw) => _queClient.SendAsync(new Message(Encoding.UTF8.GetBytes(raw)));

        public Task RequestTopic<T>(T obj) => RequestTopic(JsonConvert.SerializeObject(obj));

        public Task RequestTopic(string raw)
        {
            var msg = new Message(Encoding.UTF8.GetBytes(raw)) { Label = "RadioCom Beta" };
            msg.UserProperties["radioMessage"] = "mystery";
            return _topicClient.SendAsync(msg);
        }
    }
}