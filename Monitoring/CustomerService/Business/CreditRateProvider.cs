namespace FP.Monitoring.CustomerService.Business;

public class CreditRateProvider : ICreditRateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly Random Random = new();

    public CreditRateProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<uint> GetRateValueAsync(Customer customer, Address address)
    {
        var client = _httpClientFactory.CreateClient("credit-rate");
        var result = await client.GetAsync("/");
        if (result.IsSuccessStatusCode)
        {
            return (uint)Random.Next(50, 98);
        }

        return (uint)Random.Next(0, 10);
    }
}