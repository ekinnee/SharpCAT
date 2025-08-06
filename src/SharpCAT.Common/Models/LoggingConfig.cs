namespace SharpCAT.Common.Models;

/// <summary>
/// Logging configuration
/// </summary>
public class LoggingConfig
{
    /// <summary>
    /// Minimum log level
    /// </summary>
    public string LogLevel { get; set; } = "Information";

    /// <summary>
    /// Whether to enable console logging
    /// </summary>
    public bool EnableConsoleLogging { get; set; } = true;

    /// <summary>
    /// Whether to enable native OS logging (Event Log on Windows, syslog on Linux/macOS)
    /// </summary>
    public bool EnableOSLogging { get; set; } = true;

    /// <summary>
    /// Log file path (optional)
    /// </summary>
    public string? LogFilePath { get; set; }

    /// <summary>
    /// Whether to include timestamps in console output
    /// </summary>
    public bool IncludeTimestamps { get; set; } = true;

    /// <summary>
    /// Whether to include log levels in console output
    /// </summary>
    public bool IncludeLogLevel { get; set; } = true;
}