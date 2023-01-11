using System.Diagnostics.Metrics;

namespace Mowers.CleanArchitecture.Infrastructure.Metrics;

/// <summary>
/// A wrapper for OpenTelemetry metrics.
/// </summary>
public class Metrics
{
    private readonly Histogram<float> _processingDurationHistogram;
    private readonly Counter<int> _processingCounter;
    private readonly Counter<int> _processingErrorsCounter;
    
    /// <summary>
    /// The name of the meter.
    /// </summary>
    public string MeterName { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="Metrics"/>.
    /// </summary>
    /// <param name="meterName">The name of the meter.</param>
    /// <param name="context">The context prefix to add to metrics.</param>
    public Metrics(string meterName, string context)
    {
        var meter = new Meter(meterName);
        MeterName = meterName;
        _processingDurationHistogram = meter.CreateHistogram<float>($"{context}_file-processing-duration","ms", description:"File processing duration in ms");
        _processingCounter = meter.CreateCounter<int>($"{context}_file-processing-counter", description: "Files processed since last restart");
        _processingErrorsCounter = meter.CreateCounter<int>($"{context}_file-processing-error-counter", description:"File processing errors since last restart");
    }

    /// <summary>
    /// Increments the processing file counter.
    /// </summary>
    public void AddProcessing() => _processingCounter.Add(1);
    
    /// <summary>
    /// Increments the processing file error counter.
    /// </summary>
    public void AddProcessingError() => _processingErrorsCounter.Add(1);
    
    /// <summary>
    /// Records the processing duration as ms in the processing duration histogram.
    /// </summary>
    public void RecordProcessingDuration(float durationMs, bool success) =>
        _processingDurationHistogram.Record(durationMs, new KeyValuePair<string, object?>("success", success));
}