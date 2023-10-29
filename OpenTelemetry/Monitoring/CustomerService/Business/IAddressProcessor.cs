namespace FP.Monitoring.CustomerService.Business;

public interface IAddressProcessor
{
    Task<bool> IsValidAsync(Address address);
}