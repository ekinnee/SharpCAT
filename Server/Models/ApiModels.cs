using System.ComponentModel.DataAnnotations;
using System.IO.Ports;

namespace Server.Models
{
    /// <summary>
    /// Response model for listing available serial ports
    /// </summary>
    public class PortListResponse
    {
        /// <summary>
        /// Array of available serial port names
        /// </summary>
        public string[] Ports { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Number of available ports
        /// </summary>
        public int Count => Ports.Length;
    }

    /// <summary>
    /// Request model for opening a serial port
    /// </summary>
    public class OpenPortRequest
    {
        /// <summary>
        /// Name of the serial port to open (e.g., "COM1", "/dev/ttyUSB0")
        /// </summary>
        [Required]
        public string PortName { get; set; } = string.Empty;

        /// <summary>
        /// Baud rate for communication (default: 9600)
        /// </summary>
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// Parity setting (default: None)
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// Stop bits setting (default: One)
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// Handshake setting (default: None)
        /// </summary>
        public Handshake Handshake { get; set; } = Handshake.None;
    }

    /// <summary>
    /// Response model for port operations
    /// </summary>
    public class PortOperationResponse
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Human-readable message about the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Name of the port that was operated on
        /// </summary>
        public string? PortName { get; set; }

        /// <summary>
        /// Current status of the port (open/closed)
        /// </summary>
        public bool IsOpen { get; set; }
    }

    /// <summary>
    /// Request model for sending CAT commands
    /// </summary>
    public class SendCommandRequest
    {
        /// <summary>
        /// CAT command string to send to the radio
        /// </summary>
        [Required]
        public string Command { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for CAT command operations
    /// </summary>
    public class CommandResponse
    {
        /// <summary>
        /// Indicates if the command was sent successfully
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The command that was sent
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Response received from the radio (if any)
        /// </summary>
        public string? Response { get; set; }

        /// <summary>
        /// Human-readable message about the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the command was executed
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Response model for errors
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Additional details about the error
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Timestamp when the error occurred
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}