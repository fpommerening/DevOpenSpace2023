using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using FP.Monitoring.UI.Business;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddTransient<OrderRepository>();

builder.Services.AddHttpClient("stockservice", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["StockServiceUrl"]);
});

builder.Services.AddHttpClient("paymentservice", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["PaymentServiceUrl"]);
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();