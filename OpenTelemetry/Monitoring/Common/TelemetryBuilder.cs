using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FP.Monitoring.Common;

public class TelemetryBuilder
{
      private TelemetryBuilder(string serviceName, string url)
    {
        ServiceName = serviceName;
        Url = url;
    }
    
    private string ServiceName { get; }
    private string Url { get; }

    public static TelemetryBuilder ForConfiguration(string serviceName, string url)
    {
        return new TelemetryBuilder(serviceName, url);
    }
    
    private bool AddCustomTracing { get; set; }
    private bool AddHttpClientTracing { get; set; }
    
    private bool AddGrpcClientTracing { get; set; }
    private bool AddAspNetCoreTracing { get; set; }

    public TelemetryBuilder AddTracing(IServiceCollection services, 
        bool addHttpClientTracing = true,
        bool addAspNetCoreTracing = true,
        bool addGrpcTracing = true)
    {
        Services = services;
        AddCustomTracing = true;
        AddHttpClientTracing = addHttpClientTracing || addGrpcTracing;
        AddAspNetCoreTracing = addAspNetCoreTracing;
        AddGrpcClientTracing = addGrpcTracing;
        return this;
    }

    private IServiceCollection? Services { get; set; }
    
    private bool AddCustomMetrics { get; set; }
    private bool AddRuntimeMetrics { get; set; }
    private bool AddHttpClientMetrics { get; set; }
    private bool AddAspNetCoreMetrics { get; set; }

    public TelemetryBuilder AddMetrics(IServiceCollection services,
        bool addCustomMetrics = true,
        bool addRuntimeMetrics = true,
        bool addHttpClientMetrics = true, 
        bool addAspNetCoreMetrics = true)
    {
        Services = services;
        AddCustomMetrics = addCustomMetrics;
        AddRuntimeMetrics = addRuntimeMetrics;
        AddHttpClientMetrics = addHttpClientMetrics;
        AddAspNetCoreMetrics = addAspNetCoreMetrics;
        return this;
    }
    
    private ILoggingBuilder? LoggingBuilder { get; set; }
    
    public TelemetryBuilder AddLogging(ILoggingBuilder loggingBuilder)
    {
        LoggingBuilder = loggingBuilder;
        return this;
    }

    public void Build()
    {
        if (Services != null)
        {
            Services.AddSingleton<IInstrumentation, Instrumentation>();
            
            var builder = Services.AddOpenTelemetry()
                .ConfigureResource(rb => rb.AddEnvironmentVariableDetector().AddService(ServiceName));

            if (AddCustomMetrics || AddRuntimeMetrics || AddHttpClientMetrics || AddAspNetCoreMetrics)
            {
                builder.WithMetrics(metricsBuilder =>
                {
                    if (AddAspNetCoreMetrics)
                    {
                        metricsBuilder.AddAspNetCoreInstrumentation();    
                    }
                    if (AddHttpClientMetrics)
                    {
                        metricsBuilder.AddHttpClientInstrumentation();    
                    }

                    if (AddRuntimeMetrics)
                    {
                        metricsBuilder.AddRuntimeInstrumentation();    
                    }

                    if (AddCustomMetrics)
                    {
                        metricsBuilder.AddMeter(Instrumentation.MeterName);
                    }
                    
                    metricsBuilder.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(Url);
                    });
                });
            }
            
            if (AddCustomTracing || AddHttpClientTracing || AddAspNetCoreTracing)
            {
                builder.WithTracing(traceBuilder =>
                {
                    if (AddCustomTracing)
                    {
                        traceBuilder.AddSource(Instrumentation.ActivitySourceName);
                        traceBuilder.SetSampler(new AlwaysOnSampler());
                    }
                    
                    if (AddAspNetCoreTracing)
                    {
                        traceBuilder.AddAspNetCoreInstrumentation();    
                    }
                    if (AddHttpClientTracing)
                    {
                        traceBuilder.AddHttpClientInstrumentation();
                    }

                    if (AddGrpcClientTracing)
                    { 
                        traceBuilder.AddGrpcClientInstrumentation(opt =>
                        {
                            opt.SuppressDownstreamInstrumentation = true;
                        });
                    }
                    
                    traceBuilder.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(Url);
                    });
                });
            }
        }

        if (LoggingBuilder != null)
        {
            //LoggingBuilder.ClearProviders();
            LoggingBuilder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName));
                options.AddOtlpExporter(te =>
                {
                    te.Endpoint = new Uri(Url);
                });
            });
        }
        
    }
}