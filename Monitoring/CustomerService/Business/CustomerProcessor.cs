namespace FP.Monitoring.CustomerService.Business;

public class CustomerProcessor : ICustomerProcessor
{
    private readonly ICustomerRepository _customerRepository;
    private static readonly Random Random = new();

    public CustomerProcessor(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> ValidateAsync(string lastName, string firstName)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(15, 25)));
        var customer = await _customerRepository.LoadCustomerAsync(lastName, firstName);
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(15, 25)));
        return customer;
    }
}