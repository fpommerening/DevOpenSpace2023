namespace FP.Monitoring.CustomerService.Business;

public class Address
{
    public Address(string street, string town, string zipCode)
    {
        Street = street;
        Town = town;
        ZipCode = zipCode;
    }

    public string Street { get; set; }
    
    public string Town { get; set; }
    
    public string ZipCode { get; set; }
}