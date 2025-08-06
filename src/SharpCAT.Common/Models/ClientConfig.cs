namespace SharpCAT.Common.Models;

/// <summary>
/// Configuration model for the SharpCAT client
/// </summary>
public class ClientConfig
{
    /// <summary>
    /// Server hostname or IP address to connect to
    /// </summary>
    public string ServerHost { get; set; } = "localhost";

    /// <summary>
    /// Server port to connect to
    /// </summary>
    public int ServerPort { get; set; } = 8443;

    /// <summary>
    /// Certificate configuration for TLS
    /// </summary>
    public CertificateConfig Certificate { get; set; } = new();

    /// <summary>
    /// Logging configuration
    /// </summary>
    public LoggingConfig Logging { get; set; } = new();

    /// <summary>
    /// Connection timeout in milliseconds
    /// </summary>
    public int ConnectionTimeoutMs { get; set; } = 5000;
}