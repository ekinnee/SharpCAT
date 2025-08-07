using System.IO.Ports;

namespace Server.Services
{
    /// <summary>
    /// Interface for serial communication services
    /// </summary>
    public interface ISerialCommunicationService
    {
        /// <summary>
        /// Gets all available serial ports on the system
        /// </summary>
        /// <returns>Array of port names</returns>
        string[] GetAvailablePorts();

        /// <summary>
        /// Opens and configures a serial port
        /// </summary>
        /// <param name="portName">Name of the port to open</param>
        /// <param name="baudRate">Baud rate for communication</param>
        /// <param name="parity">Parity setting</param>
        /// <param name="stopBits">Stop bits setting</param>
        /// <param name="handshake">Handshake setting</param>
        /// <returns>True if port opened successfully</returns>
        Task<bool> OpenPortAsync(string portName, int baudRate, Parity parity, StopBits stopBits, Handshake handshake);

        /// <summary>
        /// Closes the currently opened port
        /// </summary>
        Task ClosePortAsync();

        /// <summary>
        /// Sends a CAT command to the radio
        /// </summary>
        /// <param name="command">CAT command string to send</param>
        /// <returns>Response from the radio, if any</returns>
        Task<string> SendCommandAsync(string command);

        /// <summary>
        /// Gets the status of the current port connection
        /// </summary>
        /// <returns>True if port is open and connected</returns>
        bool IsPortOpen();

        /// <summary>
        /// Gets the name of the currently opened port
        /// </summary>
        /// <returns>Port name or null if no port is open</returns>
        string? GetCurrentPortName();
    }
}