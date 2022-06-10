using Microservice_A.Models;
using Microservice_A.ServiceClient;
using Microservice_A.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IAPIClientFactory<WeatherModel> _clientFactory;
        private readonly IAzureServiceBusPublisherClient _messagePublisher;
        public TestController(IAPIClientFactory<WeatherModel> clientFactory, IAzureServiceBusPublisherClient messagePublisher)
        {
            _clientFactory = clientFactory;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("API")]
        public async Task<IActionResult> TestAPI()
        {
            WeatherModel? response = await _clientFactory.Request(new WeatherModel
            {
                Summary = "Hakuan Matata",
                date = DateTime.Now,
                temperatureC = 26.7f,
                temperatureF = 20.06f
            });
            if (response == null)
                return BadRequest("Internal Exception, check log");
            else
            {
                if(response.date != null)
                    return Ok(response);
                else
                    return Conflict("Problem at Service B");
            }
        }

        [HttpGet]
        [Route("Que")]
        public IActionResult TestAzureServiceBusQue()
        {
            Task response = _messagePublisher.Request(obj: new WeatherModel
            {
                Summary = "Hakuan Matata",
                date = DateTime.Now,
                temperatureC = 26.7f,
                temperatureF = 20.06f
            });

            return Ok("Success");
        }

        [HttpGet]
        [Route("Topic")]
        public IActionResult TestAzureServiceBusTopic()
        {
            Task response = _messagePublisher.RequestTopic(obj: new WeatherModel
            {
                Summary = "Hakuan Matata",
                date = DateTime.Now,
                temperatureC = 26.7f,
                temperatureF = 20.06f
            });
            if (response.Exception != null)
                return Conflict(response.Exception.Message);
            return Ok(response.Status.ToString());
        }
    }
}
