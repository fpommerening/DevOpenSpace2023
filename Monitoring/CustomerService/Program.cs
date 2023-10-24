using FP.Monitoring.CustomerService.Business;
using FP.Monitoring.CustomerService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IAddressProcessor, AddressProcessor>();
builder.Services.AddTransient<IZipCodeRepository, ZipCodeRepository>();
builder.Services.AddTransient<ICustomerProcessor, CustomerProcessor>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ICreditRateProvider, CreditRateProvider>();

builder.Services.AddGrpc();

builder.Services.AddHttpClient("credit-rate", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["CreditRateServiceUrl"]);
});

var app = builder.Build();

app.MapGrpcService<CustomerServices>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();