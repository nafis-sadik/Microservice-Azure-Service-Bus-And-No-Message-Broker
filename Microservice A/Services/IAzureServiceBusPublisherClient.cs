namespace Microservice_A.Services
{
    public interface IAzureServiceBusPublisherClient
    {
        public Task Request<T>(T obj);
        public Task Request(string raw);
        public Task RequestTopic<T>(T obj);
        public Task RequestTopic(string raw);
    }
}
