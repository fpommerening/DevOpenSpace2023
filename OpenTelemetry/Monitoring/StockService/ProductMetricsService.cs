using System.Diagnostics.Metrics;
using FP.Monitoring.Common;
using FP.Monitoring.Common.Models;

namespace FP.Monitoring.StockService;

public class ProductMetricsService :BackgroundService
{
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductMetricsService> _logger;
    private IReadOnlyList<Product>? _productsCache = null;
    

    public ProductMetricsService(ProductRepository productRepository, IInstrumentation instrumentation, ILogger<ProductMetricsService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
        instrumentation.CreateObservableGauge("products.current", GetCurrentQuantities);
    }
    
    private IEnumerable<Measurement<int>> GetCurrentQuantities()
    {
        if (_productsCache == null)
        {
            yield break;
        }
        var products = _productsCache.ToArray();
        foreach (var product in products)
        {
            yield return new Measurement<int>(product.Quantity,
                new KeyValuePair<string, object?>("id", product.Id.ToString("D")),
                new KeyValuePair<string, object?>("name", product.Name));
        }
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _productsCache = await _productRepository.GetProducts();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error on query products from repository");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);    
            }
        }
    }
}