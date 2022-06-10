namespace Microservice_A.Models
{
    public class WeatherModel
    {
        public DateTime? date { get; set; }
        public float temperatureC { get; set; }
        public float temperatureF { get; set; }
        public string Summary { get; set; }
    }
}
