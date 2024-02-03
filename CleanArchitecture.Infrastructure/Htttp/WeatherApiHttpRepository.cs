using Newtonsoft.Json;

using System.Net.Http.Headers;

namespace CleanArchitecture.Infrastructure.Htttp
{
    public interface IWeatherApiHttpRepository
    {
        Task<List<WeatherApiDto>> GetData(string url, CancellationToken cancellationToken);
    }

    public class WeatherApiHttpRepository : BaseHttpRepository<WeatherApiDto>, IWeatherApiHttpRepository
    {

        protected string apiKey = "d3d9fd2b66ba1fbb58696115bd6a424f";

        protected string city = "London"; // Replace with the desired city name


        public WeatherApiHttpRepository(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public virtual async Task<List<WeatherApiDto>> GetData(string url, CancellationToken cancellationToken)
        {
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            var client_ = _httpClient;

            try
            {
                using var request_ = new HttpRequestMessage();

                request_.Method = new HttpMethod("GET");
                request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));


                request_.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

                var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                var status_ = (int)response_.StatusCode;

                if (status_ == 200)
                {

                    var responseText = await response_.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var data = JsonConvert.DeserializeObject<List<WeatherApiDto>>(responseText);

                    return data ?? new List<WeatherApiDto>(0);
                }
                else if (status_ == 400)
                {
                    string responseText = response_.Content == null ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception($"Problem with the request, such as a missing, invalid or type mismatched parameter. : {responseText}");
                }
                else
                {
                    var responseData = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception($"Problem with the request, such as a missing, invalid or type mismatched parameter. :{responseData}");
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
