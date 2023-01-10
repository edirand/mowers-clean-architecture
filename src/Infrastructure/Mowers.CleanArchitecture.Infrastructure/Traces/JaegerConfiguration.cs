namespace Mowers.CleanArchitecture.Infrastructure.Traces;

/// <summary>
/// A configuration file to add jaeger exporter using OpenTelemetry.
/// </summary>
public class JaegerConfiguration
{
    /// <summary>
    /// Jaeger host.
    /// </summary>
    public string Host { get; set; }
    
    /// <summary>
    /// Jaeger port.
    /// </summary>
    public int Port { get; set; }
}