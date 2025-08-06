using System.IO.Ports;
using System.Runtime.InteropServices;

namespace SharpCAT.Common.SerialPort;

/// <summary>
/// Cross-platform serial port helper utilities
/// </summary>
public static class SerialPortHelper
{
    /// <summary>
    /// Gets available serial port names for the current platform
    /// </summary>
    /// <returns>Array of available serial port names</returns>
    public static string[] GetAvailablePortNames()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetWindowsPortNames();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetLinuxPortNames();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GetMacOSPortNames();
            }
            else
            {
                // Fallback to .NET standard method
                return System.IO.Ports.SerialPort.GetPortNames();
            }
        }
        catch
        {
            // Fallback to .NET standard method on any error
            return System.IO.Ports.SerialPort.GetPortNames();
        }
    }

    /// <summary>
    /// Auto-detects the best serial port for CAT communication
    /// </summary>
    /// <returns>The detected port name, or null if none found</returns>
    public static async Task<string?> AutoDetectCATPortAsync()
    {
        var ports = GetAvailablePortNames();
        
        foreach (var portName in ports)
        {
            try
            {
                using var port = new System.IO.Ports.SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                port.ReadTimeout = 1000;
                port.WriteTimeout = 1000;
                
                port.Open();
                
                // Try a simple CAT command (may vary by radio)
                // This is a basic "ID" command that many radios support
                await Task.Delay(100); // Allow port to stabilize
                
                port.Close();
                
                // If we get here without exception, the port is likely valid
                return portName;
            }
            catch
            {
                // Port is not available or doesn't respond - continue to next
                continue;
            }
        }
        
        return null;
    }

    private static string[] GetWindowsPortNames()
    {
        // Windows COM ports
        var ports = new List<string>();
        for (int i = 1; i <= 256; i++)
        {
            string portName = $"COM{i}";
            try
            {
                using var port = new System.IO.Ports.SerialPort(portName);
                ports.Add(portName);
            }
            catch
            {
                // Port doesn't exist
            }
        }
        return ports.ToArray();
    }

    private static string[] GetLinuxPortNames()
    {
        // Linux serial devices
        var ports = new List<string>();
        
        // Standard serial ports
        for (int i = 0; i < 32; i++)
        {
            string portName = $"/dev/ttyS{i}";
            if (File.Exists(portName))
                ports.Add(portName);
        }
        
        // USB serial ports
        for (int i = 0; i < 32; i++)
        {
            string portName = $"/dev/ttyUSB{i}";
            if (File.Exists(portName))
                ports.Add(portName);
        }
        
        // USB ACM ports (often used by Arduino and similar devices)
        for (int i = 0; i < 32; i++)
        {
            string portName = $"/dev/ttyACM{i}";
            if (File.Exists(portName))
                ports.Add(portName);
        }
        
        return ports.ToArray();
    }

    private static string[] GetMacOSPortNames()
    {
        // macOS serial devices
        var ports = new List<string>();
        
        try
        {
            // Check for USB serial devices
            var devDirectory = "/dev";
            if (Directory.Exists(devDirectory))
            {
                var files = Directory.GetFiles(devDirectory, "tty.*");
                foreach (var file in files)
                {
                    if (file.Contains("usb", StringComparison.OrdinalIgnoreCase) ||
                        file.Contains("serial", StringComparison.OrdinalIgnoreCase))
                    {
                        ports.Add(file);
                    }
                }
                
                // Also check for cu.* devices which are common on macOS
                files = Directory.GetFiles(devDirectory, "cu.*");
                foreach (var file in files)
                {
                    if (file.Contains("usb", StringComparison.OrdinalIgnoreCase) ||
                        file.Contains("serial", StringComparison.OrdinalIgnoreCase))
                    {
                        ports.Add(file);
                    }
                }
            }
        }
        catch
        {
            // Fallback to standard method
        }
        
        return ports.ToArray();
    }
}