using Newtonsoft.Json;

namespace Microservice_A.ServiceClient
{
    public class APIClientFactory<T> : IAPIClientFactory<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public APIClientFactory(IConfiguration configuration, HttpClient client)
        {
            _httpClient = client;
            _configuration = configuration;

            string? createUrl = _configuration.GetSection("MicroService_B").GetValue<string>("TargetUrl");
            _httpClient.BaseAddress = new Uri(createUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T?> Request(T model)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("jsonmodel", model).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    string? responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                    return JsonConvert.DeserializeObject<T>(responseJson);
                }
                return default(T);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}