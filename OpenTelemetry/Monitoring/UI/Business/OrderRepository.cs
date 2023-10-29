using System.Text;
using System.Text.Json;
using FP.Monitoring.Common.Models;

namespace FP.Monitoring.UI.Business;

public class OrderRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public OrderRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IReadOnlyList<Product>> GetProducts()
    {
        var client = _httpClientFactory.CreateClient("stockservice");
        var response = await client.GetAsync("products");
        var products = JsonSerializer.Deserialize<Product[]>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
        return products ?? Array.Empty<Product>();
    }
    
    public async Task OrderProduct(Guid id, int quantity, string customer, string cardType, string cardNumber)
    {
        var stockClient = _httpClientFactory.CreateClient("stockservice");
        var productsResponse = await stockClient.GetAsync("products");
        var products = JsonSerializer.Deserialize<Product[]>(await productsResponse.Content.ReadAsStringAsync(), _jsonSerializerOptions);
        var product = products.Single(x => x.Id == id);

        var orderResponse = await stockClient.PutAsync($"products/{id}", new StringContent(quantity.ToString(), Encoding.UTF8, "application/json"));
        orderResponse.EnsureSuccessStatusCode();

        await Task.Delay(250);
        var payment = new Payment
        {
            Name = customer,
            Amount = product.Price * (decimal) quantity,
            Number = cardNumber,
            Type = cardType
        };

        var paymentClient = _httpClientFactory.CreateClient("paymentservice");
        var paymentAsJson = JsonSerializer.Serialize(payment, _jsonSerializerOptions);

        var paymentResponse = await paymentClient.PutAsync("payments", new StringContent(paymentAsJson, Encoding.UTF8, "application/json"));
        paymentResponse.EnsureSuccessStatusCode();

    }
}