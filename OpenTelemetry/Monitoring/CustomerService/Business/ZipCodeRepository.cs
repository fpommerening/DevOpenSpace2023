using FP.Monitoring.Common;
namespace FP.Monitoring.CustomerService.Business;

public class ZipCodeRepository : IZipCodeRepository
{
    private readonly IInstrumentation _instrumentation;
    private static readonly Random Random = new();

    public ZipCodeRepository(IInstrumentation instrumentation)
    {
        _instrumentation = instrumentation;
    }

    public async Task<bool> IsValidAsync(string zipCode)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity($"{nameof(ZipCodeRepository)}.{nameof(IsValidAsync)}");
        activity?.AddTag("ZipCode", zipCode);
        //Activity.Current?.AddTag("ZipCode", zipCode);
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(25, 45)));
        return zipCode.Length == 5;
    }
}