using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SharpCAT.Common.Logging;

/// <summary>
/// Custom logger provider that supports both console and native OS logging
/// </summary>
public class SharpCATLoggerProvider : ILoggerProvider
{
    private readonly bool _enableConsoleLogging;
    private readonly bool _enableOSLogging;
    private readonly string _applicationName;

    public SharpCATLoggerProvider(bool enableConsoleLogging = true, bool enableOSLogging = true, string applicationName = "SharpCAT")
    {
        _enableConsoleLogging = enableConsoleLogging;
        _enableOSLogging = enableOSLogging;
        _applicationName = applicationName;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SharpCATLogger(categoryName, _enableConsoleLogging, _enableOSLogging, _applicationName);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}

/// <summary>
/// Custom logger that supports both console and native OS logging
/// </summary>
public class SharpCATLogger : ILogger
{
    private readonly string _categoryName;
    private readonly bool _enableConsoleLogging;
    private readonly bool _enableOSLogging;
    private readonly string _applicationName;
    private readonly EventLog? _eventLog;

    public SharpCATLogger(string categoryName, bool enableConsoleLogging, bool enableOSLogging, string applicationName)
    {
        _categoryName = categoryName;
        _enableConsoleLogging = enableConsoleLogging;
        _enableOSLogging = enableOSLogging;
        _applicationName = applicationName;

        // Initialize Event Log for Windows
        if (_enableOSLogging && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                _eventLog = new EventLog();
                _eventLog.Source = _applicationName;
                
                // Create event source if it doesn't exist
                if (!EventLog.SourceExists(_applicationName))
                {
                    EventLog.CreateEventSource(_applicationName, "Application");
                }
            }
            catch
            {
                // If we can't create event log, just disable OS logging
                _eventLog = null;
            }
        }
    }

    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        
        if (exception != null)
            message += Environment.NewLine + exception.ToString();

        // Console logging
        if (_enableConsoleLogging)
        {
            LogToConsole(logLevel, message);
        }

        // OS logging
        if (_enableOSLogging)
        {
            LogToOS(logLevel, message);
        }
    }

    private void LogToConsole(LogLevel logLevel, string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var level = GetLogLevelString(logLevel);
        var formattedMessage = $"[{timestamp}] [{level}] [{_categoryName}] {message}";

        // Set console color based on log level
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = GetConsoleColor(logLevel);
        
        Console.WriteLine(formattedMessage);
        
        Console.ForegroundColor = originalColor;
    }

    private void LogToOS(LogLevel logLevel, string message)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            LogToWindowsEventLog(logLevel, message);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            LogToSyslog(logLevel, message);
        }
    }

    private void LogToWindowsEventLog(LogLevel logLevel, string message)
    {
        try
        {
            _eventLog?.WriteEntry($"[{_categoryName}] {message}", GetEventLogEntryType(logLevel));
        }
        catch
        {
            // Ignore errors writing to event log
        }
    }

    private void LogToSyslog(LogLevel logLevel, string message)
    {
        try
        {
            // Use logger command to write to syslog
            var priority = GetSyslogPriority(logLevel);
            var formattedMessage = $"[{_categoryName}] {message}";
            
            // Escape message for shell
            var escapedMessage = formattedMessage.Replace("\"", "\\\"");
            
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/logger",
                Arguments = $"-p {priority} -t \"{_applicationName}\" \"{escapedMessage}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            process.Start();
            process.WaitForExit(1000); // Wait max 1 second
        }
        catch
        {
            // Ignore errors writing to syslog
        }
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRIT",
            _ => "UNKNOWN"
        };
    }

    private static ConsoleColor GetConsoleColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.DarkRed,
            _ => ConsoleColor.White
        };
    }

    private static EventLogEntryType GetEventLogEntryType(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Warning => EventLogEntryType.Warning,
            LogLevel.Error => EventLogEntryType.Error,
            LogLevel.Critical => EventLogEntryType.Error,
            _ => EventLogEntryType.Information
        };
    }

    private static string GetSyslogPriority(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "user.debug",
            LogLevel.Debug => "user.debug",
            LogLevel.Information => "user.info",
            LogLevel.Warning => "user.warning",
            LogLevel.Error => "user.err",
            LogLevel.Critical => "user.crit",
            _ => "user.info"
        };
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}