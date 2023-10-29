namespace FP.Monitoring.PaymentService;

public class CardProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CardProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Validate(string type, string number)
    {
        switch (type.ToLowerInvariant())
        {
            case "visa":
                await ValidateVisa();
                break;
            case "master":
                await ValidateMaster();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type),$"Unknown Card-Type {type}");
        }
    }
    private async Task ValidateVisa()
    {
        var client = _httpClientFactory.CreateClient("visa");
        var result = await client.GetAsync("/");
        result.EnsureSuccessStatusCode();
    }

    private async Task ValidateMaster()
    {
        var client = _httpClientFactory.CreateClient("master");
        var result = await client.GetAsync("/");
        result.EnsureSuccessStatusCode();
    }
}