namespace FP.Monitoring.CustomerService.Business;

public class Customer
{
    public Customer(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public bool IsValid { get; set; }
}