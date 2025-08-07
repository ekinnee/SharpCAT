using System.IO.Ports;
using System.Text;

namespace Server.Services
{
    /// <summary>
    /// Service for managing serial communication with radios using SharpCAT library
    /// </summary>
    public class SerialCommunicationService : ISerialCommunicationService, IDisposable
    {
        private readonly ILogger<SerialCommunicationService> _logger;
        private SharpCAT.Serial? _serialConnection;
        private SerialPort? _directSerialPort;
        private string? _currentPortName;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _disposed = false;

        public SerialCommunicationService(ILogger<SerialCommunicationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public string[] GetAvailablePorts()
        {
            try
            {
                var sharpCat = new SharpCAT.SharpCAT();
                var ports = sharpCat.PortNames;
                _logger.LogInformation("Found {PortCount} available serial ports", ports?.Length ?? 0);
                return ports ?? Array.Empty<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available serial ports");
                return Array.Empty<string>();
            }
        }

        /// <inheritdoc />
        public async Task<bool> OpenPortAsync(string portName, int baudRate, Parity parity, StopBits stopBits, Handshake handshake)
        {
            if (string.IsNullOrWhiteSpace(portName))
                throw new ArgumentException("Port name cannot be null or empty", nameof(portName));

            await _semaphore.WaitAsync();
            try
            {
                // Close existing connection if any
                await ClosePortInternalAsync();

                _logger.LogInformation("Attempting to open port {PortName} with baud rate {BaudRate}", portName, baudRate);

                // Create direct SerialPort for more control over communication
                _directSerialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate,
                    Parity = parity,
                    StopBits = stopBits,
                    Handshake = handshake,
                    DataBits = 8,
                    ReadTimeout = 5000,
                    WriteTimeout = 5000,
                    Encoding = Encoding.ASCII
                };

                _directSerialPort.Open();
                _currentPortName = portName;

                _logger.LogInformation("Successfully opened port {PortName}", portName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open port {PortName}", portName);
                _directSerialPort?.Dispose();
                _directSerialPort = null;
                _currentPortName = null;
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task ClosePortAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                await ClosePortInternalAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task<string> SendCommandAsync(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Command cannot be null or empty", nameof(command));

            await _semaphore.WaitAsync();
            try
            {
                if (_directSerialPort == null || !_directSerialPort.IsOpen)
                {
                    throw new InvalidOperationException("No serial port is currently open");
                }

                _logger.LogDebug("Sending command: {Command}", command);

                // Send the command
                await Task.Run(() => _directSerialPort.WriteLine(command));

                // Wait a bit for response
                await Task.Delay(100);

                // Try to read response
                string response = "";
                if (_directSerialPort.BytesToRead > 0)
                {
                    response = await Task.Run(() => 
                    {
                        try
                        {
                            return _directSerialPort.ReadExisting();
                        }
                        catch (TimeoutException)
                        {
                            return "";
                        }
                    });
                }

                _logger.LogDebug("Command response: {Response}", response);
                return response.Trim();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending command: {Command}", command);
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public bool IsPortOpen()
        {
            return _directSerialPort?.IsOpen == true;
        }

        /// <inheritdoc />
        public string? GetCurrentPortName()
        {
            return _currentPortName;
        }

        private async Task ClosePortInternalAsync()
        {
            if (_directSerialPort != null)
            {
                try
                {
                    if (_directSerialPort.IsOpen)
                    {
                        await Task.Run(() => _directSerialPort.Close());
                    }
                    _directSerialPort.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error closing serial port");
                }
                finally
                {
                    _directSerialPort = null;
                    _currentPortName = null;
                }
            }

            if (_serialConnection != null)
            {
                _serialConnection = null;
            }

            _logger.LogInformation("Serial port closed");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ClosePortInternalAsync().Wait();
                _semaphore?.Dispose();
                _disposed = true;
            }
        }
    }
}