namespace FP.Monitoring.CustomerService.Business;

public class ZipCodeRepository : IZipCodeRepository
{
    private static readonly Random Random = new();
    
    public async Task<bool> IsValidAsync(string zipCode)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Next(25, 45)));
        return zipCode.Length == 5;
    }
}