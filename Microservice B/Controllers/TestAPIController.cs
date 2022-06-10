using Microservice_B.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservice_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        [HttpPost]
        [Route("hit/{jsonmodel}")]
        public IActionResult TestAPI(WeatherModel jsonmodel)
        {
            try
            {
                jsonmodel.temperatureC = new Random().Next(0, 50);
                jsonmodel.temperatureF = (jsonmodel.temperatureC * 9 / 5) + 32;
                return Ok(jsonmodel);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
