namespace SharpCAT.Common.Models;

/// <summary>
/// Serial port configuration for CAT communication
/// </summary>
public class SerialPortConfig
{
    /// <summary>
    /// Serial port name (e.g., COM1, /dev/ttyUSB0)
    /// </summary>
    public string? PortName { get; set; }

    /// <summary>
    /// Baud rate for serial communication
    /// </summary>
    public int BaudRate { get; set; } = 9600;

    /// <summary>
    /// Data bits
    /// </summary>
    public int DataBits { get; set; } = 8;

    /// <summary>
    /// Stop bits
    /// </summary>
    public System.IO.Ports.StopBits StopBits { get; set; } = System.IO.Ports.StopBits.One;

    /// <summary>
    /// Parity setting
    /// </summary>
    public System.IO.Ports.Parity Parity { get; set; } = System.IO.Ports.Parity.None;

    /// <summary>
    /// Flow control/handshake
    /// </summary>
    public System.IO.Ports.Handshake Handshake { get; set; } = System.IO.Ports.Handshake.None;

    /// <summary>
    /// Read timeout in milliseconds
    /// </summary>
    public int ReadTimeoutMs { get; set; } = 1000;

    /// <summary>
    /// Write timeout in milliseconds
    /// </summary>
    public int WriteTimeoutMs { get; set; } = 1000;

    /// <summary>
    /// Whether to auto-detect the serial port if not specified
    /// </summary>
    public bool AutoDetectPort { get; set; } = true;
}