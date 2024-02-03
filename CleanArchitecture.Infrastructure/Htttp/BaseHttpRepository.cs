using Azure;

using Newtonsoft.Json;
namespace CleanArchitecture.Infrastructure.Htttp
{
    public abstract class BaseHttpRepository<TInput>  where TInput : class
    {
        protected readonly HttpClient _httpClient;

        protected BaseHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
