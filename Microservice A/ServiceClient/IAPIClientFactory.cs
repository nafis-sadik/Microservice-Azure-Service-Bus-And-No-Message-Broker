using Microservice_A.Models;
namespace Microservice_A.ServiceClient
{
    public interface IAPIClientFactory<T> where T : class
    {
        public Task<T?> Request(T model);
    }
}
