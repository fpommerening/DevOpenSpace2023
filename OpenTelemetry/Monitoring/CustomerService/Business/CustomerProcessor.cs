using FP.Monitoring.Common;
namespace FP.Monitoring.CustomerService.Business;

public class CustomerProcessor : ICustomerProcessor
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IInstrumentation _instrumentation;
    private static readonly Random Random = new();

    public CustomerProcessor(ICustomerRepository customerRepository, IInstrumentation instrumentation)
    {
        _customerRepository = customerRepository;
        _instrumentation = instrumentation;
    }

    public async Task<Customer> ValidateAsync(string lastName, string firstName)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity($"{nameof(CustomerProcessor)}.{nameof(ValidateAsync)}");
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(15, 25)));
        var customer = await _customerRepository.LoadCustomerAsync(lastName, firstName);
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(15, 25)));
        return customer;
    }
}