using FP.Monitoring.CustomerService.Business;
using FP.Monitoring.CustomerService.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddEnvironmentVariableDetector()
        .AddService("CustomerService"))
    .WithTracing(tb =>
    {
        tb.SetSampler(new AlwaysOnSampler());
        tb.AddAspNetCoreInstrumentation();
        tb.AddOtlpExporter(otp =>
        {
            otp.Endpoint = new Uri("https://t.container-training.de");
        });
    });

var app = builder.Build();

app.MapGrpcService<CustomerServices>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();