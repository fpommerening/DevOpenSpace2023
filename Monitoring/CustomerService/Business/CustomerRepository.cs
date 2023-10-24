namespace FP.Monitoring.CustomerService.Business;

public class CustomerRepository : ICustomerRepository
{
    private static readonly Random Random = new();
    
    public async Task<Customer> LoadCustomerAsync(string lastName, string firstName)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(22, 66)));
        return new Customer(firstName, lastName)
        {
            IsValid = (lastName.Length + firstName.Length) % 4 == 0
        };
    }
}