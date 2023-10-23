using FP.Monitoring.Common.Models;

namespace FP.Monitoring.StockService;

public class ProductRepository
{
    private Random _random = new Random();
    private List<Product> items = new()
    {
        new()
        {
            Id = Guid.Parse("6679a09c-ab69-46ae-bd4c-9a4bb0a69d93"), Name = "Lenovo G27q-20", Price = 268.98m,
            Quantity = 41
        },
        new()
        {
            Id = Guid.Parse("fba18a9a-6f8c-4e8d-b264-dc165632d5da"), Name = "Geo GeoBook 140", Price = 259.05m,
            Quantity = 99
        },
        new Product
        {
            Id = Guid.Parse("c0110316-9540-4ded-aea0-42e7083ec174"), Name = "Microsoft Classic IntelliMouse",
            Price = 16.97m, Quantity = 196
        },
        new Product
        {
            Id = Guid.Parse("598f076d-0f50-4788-813f-f557e43d73f5"), Name = "Brother MFC-J5730DW", Price = 315.28m,
            Quantity = 214
        },
        new Product
        {
            Id = Guid.Parse("d05382c7-a560-45e5-9bbc-96c81a675adf"), Name = "AVM FRITZ!Box 7590", Price = 215.99m,
            Quantity = 63
        },
        new Product
        {
            Id = Guid.Parse("306a9983-4b1b-4691-96e0-156b80d10fb8"), Name = "Optoma HD146X Beamer", Price = 499.00m,
            Quantity = 37
        }
    };
    
    public async Task<IReadOnlyList<Product>> GetProducts()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(200, 500)));
        return items.AsReadOnly();
    }

    public async Task UpdateProducts(Guid id, int quantity)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(100, 200)));
        var product = items.FirstOrDefault(x => x.Id == id);

        if (product == null)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        var newQuantity = product.Quantity - quantity;

        if (newQuantity < 0)
        {
            throw new ArgumentException(nameof(quantity));
        }
        product.Quantity = newQuantity;
    }
}