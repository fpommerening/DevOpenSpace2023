using FP.Monitoring.Common;
namespace FP.Monitoring.CustomerService.Business;

public class AddressProcessor : IAddressProcessor
{
    private readonly IZipCodeRepository _zipCodeRepository;
    private readonly IInstrumentation _instrumentation;
    private static readonly Random Random = new();

    public AddressProcessor(IZipCodeRepository zipCodeRepository, IInstrumentation instrumentation)
    {
        _zipCodeRepository = zipCodeRepository;
        _instrumentation = instrumentation;
    }

    public async Task<bool> IsValidAsync(Address address)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity($"{nameof(AddressProcessor)}.{nameof(IsValidAsync)}");
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(10, 30)));
        if (!await _zipCodeRepository.IsValidAsync(address.ZipCode))
        {
            return false;
        }
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(30, 60)));
        return ((address.Street.Length + address.Town.Length) % 7) == 0;
    }
}