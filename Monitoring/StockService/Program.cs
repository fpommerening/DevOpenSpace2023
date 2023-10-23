using FP.Monitoring.StockService;
using Microsoft.AspNetCore.Mvc;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapGet("/", () => "Stockservice");

app.MapGet("/products", async ([FromServices] ProductRepository repository) =>
{
    await Task.Delay(TimeSpan.FromMilliseconds(50));
    var products = await repository.GetProducts();
    return products;
});

app.MapPut("/products/{productId:guid}", async ([FromRoute] Guid productId, [FromBody] int quantity, [FromServices] ProductRepository repository) =>
{
    await Task.Delay(TimeSpan.FromMilliseconds(50));
    try
    {
        await repository.UpdateProducts(productId, quantity);
    }
    catch (ArgumentOutOfRangeException)
    {
        return Results.NotFound();
    }
    catch (ArgumentException)
    {
        return Results.BadRequest();
    }
    return Results.Accepted();
});

app.Run();
