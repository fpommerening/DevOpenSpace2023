using FP.Monitoring.Common;
namespace FP.Monitoring.CustomerService.Business;

public class CreditRateProvider : ICreditRateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IInstrumentation _instrumentation;
    private static readonly Random Random = new();

    public CreditRateProvider(IHttpClientFactory httpClientFactory, IInstrumentation instrumentation)
    {
        _httpClientFactory = httpClientFactory;
        _instrumentation = instrumentation;
    }
    
    public async Task<uint> GetRateValueAsync(Customer customer, Address address)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity($"{nameof(CreditRateProvider)}.{nameof(GetRateValueAsync)}");
        var client = _httpClientFactory.CreateClient("credit-rate");
        var result = await client.GetAsync("/");
        if (result.IsSuccessStatusCode)
        {
            return (uint)Random.Next(50, 98);
        }

        return (uint)Random.Next(0, 10);
    }
}