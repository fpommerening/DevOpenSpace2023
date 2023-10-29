using FP.Monitoring.Common;
namespace FP.Monitoring.CustomerService.Business;

public class CustomerRepository : ICustomerRepository
{
    private readonly IInstrumentation _instrumentation;
    private static readonly Random Random = new();
    public CustomerRepository(IInstrumentation instrumentation)
    {
        _instrumentation = instrumentation;
    }
    public async Task<Customer> LoadCustomerAsync(string lastName, string firstName)
    {
        using var activity = _instrumentation.ActivitySource.StartActivity($"{nameof(CustomerRepository)}.{nameof(LoadCustomerAsync)}");
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(22, 66)));
        return new Customer(firstName, lastName)
        {
            IsValid = (lastName.Length + firstName.Length) % 4 == 0
        };
    }
}