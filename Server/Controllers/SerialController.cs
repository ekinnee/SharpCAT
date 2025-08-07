using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    /// <summary>
    /// Controller for serial port management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SerialController : ControllerBase
    {
        private readonly ISerialCommunicationService _serialService;
        private readonly ILogger<SerialController> _logger;

        public SerialController(ISerialCommunicationService serialService, ILogger<SerialController> logger)
        {
            _serialService = serialService ?? throw new ArgumentNullException(nameof(serialService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all available serial ports on the server
        /// </summary>
        /// <returns>JSON array of available serial port names</returns>
        /// <response code="200">Returns the list of available serial ports</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("ports")]
        [ProducesResponseType(typeof(PortListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
    }
}