using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace FP.Monitoring.Common;

public interface IInstrumentation
{
    ActivitySource ActivitySource { get; }
    ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string? unit = null, string? description = null) where T : struct;
    ObservableGauge<T> CreateObservableGauge<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null) where T : struct;
    Counter<T> CreateCounter<T>(string name, string? unit = null, string? description = null) where T : struct;
}