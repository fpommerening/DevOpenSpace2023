namespace FP.Monitoring.CustomerService.Business;

public interface IZipCodeRepository
{
    Task<bool> IsValidAsync(string zipCode);
}