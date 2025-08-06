namespace SharpCAT.Common.Models;

/// <summary>
/// Configuration model for the SharpCAT server
/// </summary>
public class ServerConfig
{
    /// <summary>
    /// TCP port to listen on
    /// </summary>
    public int Port { get; set; } = 8443;

    /// <summary>
    /// Certificate configuration for TLS
    /// </summary>
    public CertificateConfig Certificate { get; set; } = new();

    /// <summary>
    /// Serial port configuration for CAT communication
    /// </summary>
    public SerialPortConfig SerialPort { get; set; } = new();

    /// <summary>
    /// Logging configuration
    /// </summary>
    public LoggingConfig Logging { get; set; } = new();

    /// <summary>
    /// Maximum number of concurrent clients
    /// </summary>
    public int MaxClients { get; set; } = 10;
}