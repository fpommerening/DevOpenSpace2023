using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry().WithMetrics(mb =>
{
    //mb.AddRuntimeInstrumentation();
    mb.AddPrometheusExporter();
    mb.AddMeter(FP.Monitoring.MetricsPlayground.Pages.Index.MeterName);
    // mb.AddOtlpExporter(otp =>
    // {
    //     otp.Endpoint = new Uri("https://t.container-training.de");
    // });
    mb.AddView("playground.histogram", new ExplicitBucketHistogramConfiguration
    {
        Boundaries = new double[] { 1, 2, 3, 5 }
    });
});

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();