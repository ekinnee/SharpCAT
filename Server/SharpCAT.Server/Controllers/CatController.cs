using Microsoft.AspNetCore.Mvc;
using SharpCAT.Server.Models;
using SharpCAT.Server.Services;

namespace SharpCAT.Server.Controllers
{
    /// <summary>
    /// Controller for CAT (Computer Aided Transceiver) operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CatController : ControllerBase
    {
        private readonly ISerialCommunicationService _serialService;
        private readonly ILogger<CatController> _logger;

        public CatController(ISerialCommunicationService serialService, ILogger<CatController> logger)
        {
            _serialService = serialService ?? throw new ArgumentNullException(nameof(serialService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all available serial ports on the system
        /// </summary>
        /// <returns>List of available serial ports</returns>
        /// <response code="200">Returns the list of available serial ports</response>
        [HttpGet("ports")]
        [ProducesResponseType(typeof(PortListResponse), StatusCodes.Status200OK)]
        public ActionResult<PortListResponse> GetPorts()
        {
            try
            {
                _logger.LogInformation("Getting available serial ports");
                var ports = _serialService.GetAvailablePorts();
                
                var response = new PortListResponse
                {
                    Ports = ports
                };

                _logger.LogInformation("Found {PortCount} available ports", response.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available ports");
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal server error while getting ports",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Opens and configures a serial port for communication
        /// </summary>
        /// <param name="request">Port configuration parameters</param>
        /// <returns>Result of the port open operation</returns>
        /// <response code="200">Port opened successfully</response>
        /// <response code="400">Invalid request parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("open")]
        [ProducesResponseType(typeof(PortOperationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PortOperationResponse>> OpenPort([FromBody] OpenPortRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid request parameters",
                        Details = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                _logger.LogInformation("Attempting to open port {PortName} with baud rate {BaudRate}", 
                    request.PortName, request.BaudRate);

                var success = await _serialService.OpenPortAsync(
                    request.PortName, 
                    request.BaudRate, 
                    request.Parity, 
                    request.StopBits, 
                    request.Handshake);

                var response = new PortOperationResponse
                {
                    Success = success,
                    PortName = request.PortName,
                    IsOpen = _serialService.IsPortOpen(),
                    Message = success 
                        ? $"Port {request.PortName} opened successfully" 
                        : $"Failed to open port {request.PortName}"
                };

                if (success)
                {
                    _logger.LogInformation("Successfully opened port {PortName}", request.PortName);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Failed to open port {PortName}", request.PortName);
                    return StatusCode(500, new ErrorResponse 
                    { 
                        Error = response.Message 
                    });
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for opening port: {PortName}", request.PortName);
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Invalid port parameters",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening port: {PortName}", request.PortName);
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal server error while opening port",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Closes the currently opened serial port
        /// </summary>
        /// <returns>Result of the port close operation</returns>
        /// <response code="200">Port closed successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("close")]
        [ProducesResponseType(typeof(PortOperationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PortOperationResponse>> ClosePort()
        {
            try
            {
                var currentPortName = _serialService.GetCurrentPortName();
                _logger.LogInformation("Attempting to close port {PortName}", currentPortName ?? "unknown");

                await _serialService.ClosePortAsync();

                var response = new PortOperationResponse
                {
                    Success = true,
                    PortName = currentPortName,
                    IsOpen = _serialService.IsPortOpen(),
                    Message = "Port closed successfully"
                };

                _logger.LogInformation("Successfully closed port");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing port");
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal server error while closing port",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Sends a CAT command to the connected radio
        /// </summary>
        /// <param name="request">CAT command to send</param>
        /// <returns>Result of the command operation including any response from the radio</returns>
        /// <response code="200">Command sent successfully</response>
        /// <response code="400">Invalid request parameters or no port open</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("command")]
        [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommandResponse>> SendCommand([FromBody] SendCommandRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "Invalid request parameters",
                        Details = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                if (!_serialService.IsPortOpen())
                {
                    return BadRequest(new ErrorResponse 
                    { 
                        Error = "No serial port is currently open",
                        Details = "You must open a serial port before sending commands"
                    });
                }

                _logger.LogInformation("Sending CAT command: {Command}", request.Command);

                var radioResponse = await _serialService.SendCommandAsync(request.Command);

                var response = new CommandResponse
                {
                    Success = true,
                    Command = request.Command,
                    Response = radioResponse,
                    Message = "Command sent successfully",
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation("CAT command sent successfully. Response: {Response}", radioResponse);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid command: {Command}", request.Command);
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Invalid command",
                    Details = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot send command - port not open");
                return BadRequest(new ErrorResponse 
                { 
                    Error = "Cannot send command",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending CAT command: {Command}", request.Command);
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal server error while sending command",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets the current status of the serial port connection
        /// </summary>
        /// <returns>Current port status</returns>
        /// <response code="200">Returns current port status</response>
        [HttpGet("status")]
        [ProducesResponseType(typeof(PortOperationResponse), StatusCodes.Status200OK)]
        public ActionResult<PortOperationResponse> GetStatus()
        {
            try
            {
                var isOpen = _serialService.IsPortOpen();
                var currentPort = _serialService.GetCurrentPortName();

                var response = new PortOperationResponse
                {
                    Success = true,
                    PortName = currentPort,
                    IsOpen = isOpen,
                    Message = isOpen 
                        ? $"Port {currentPort} is open" 
                        : "No port is currently open"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting port status");
                return StatusCode(500, new ErrorResponse 
                { 
                    Error = "Internal server error while getting status",
                    Details = ex.Message
                });
            }
        }
    }
}