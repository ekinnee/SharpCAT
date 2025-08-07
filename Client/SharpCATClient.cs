using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SharpCAT.Client.Models;

namespace SharpCAT.Client
{
    /// <summary>
    /// Client for accessing SharpCAT Server API endpoints
    /// </summary>
    public class SharpCATClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _ownsHttpClient;

        /// <summary>
        /// Gets the base address of the SharpCAT server
        /// </summary>
        public Uri BaseAddress => _httpClient.BaseAddress ?? throw new InvalidOperationException("Base address not set");

        /// <summary>
        /// Initializes a new instance of the SharpCATClient with the specified base address
        /// </summary>
        /// <param name="baseAddress">Base URL of the SharpCAT server (e.g., "http://localhost:5188")</param>
        public SharpCATClient(string baseAddress) : this(new Uri(baseAddress))
        {
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClient with the specified base address
        /// </summary>
        /// <param name="baseAddress">Base URI of the SharpCAT server</param>
        public SharpCATClient(Uri baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = baseAddress };
            _ownsHttpClient = true;
        }

        /// <summary>
        /// Initializes a new instance of the SharpCATClient with an existing HttpClient
        /// </summary>
        /// <param name="httpClient">Configured HttpClient instance</param>
        public SharpCATClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _ownsHttpClient = false;
        }

        /// <summary>
        /// Gets all available serial ports on the system
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of available serial ports</returns>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<PortListResponse> GetPortsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PortListResponse>("api/cat/ports", cancellationToken);
                return response ?? throw new SharpCATClientException("Failed to deserialize response");
            }
            catch (HttpRequestException ex)
            {
                throw new SharpCATClientException("Failed to get ports", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SharpCATClientException("Request timed out", ex);
            }
        }

        /// <summary>
        /// Opens and configures a serial port for communication
        /// </summary>
        /// <param name="request">Port configuration parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result of the port open operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<PortOperationResponse> OpenPortAsync(OpenPortRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("api/cat/open", request, cancellationToken);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<PortOperationResponse>(cancellationToken: cancellationToken);
                    return response ?? throw new SharpCATClientException("Failed to deserialize response");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                    throw new SharpCATClientException($"API returned error: {errorResponse?.Error ?? "Unknown error"}", errorResponse?.Details);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new SharpCATClientException("Failed to open port", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SharpCATClientException("Request timed out", ex);
            }
        }

        /// <summary>
        /// Closes the currently opened serial port
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result of the port close operation</returns>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<PortOperationResponse> ClosePortAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var httpResponse = await _httpClient.PostAsync("api/cat/close", null, cancellationToken);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<PortOperationResponse>(cancellationToken: cancellationToken);
                    return response ?? throw new SharpCATClientException("Failed to deserialize response");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                    throw new SharpCATClientException($"API returned error: {errorResponse?.Error ?? "Unknown error"}", errorResponse?.Details);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new SharpCATClientException("Failed to close port", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SharpCATClientException("Request timed out", ex);
            }
        }

        /// <summary>
        /// Gets the current status of the serial port connection
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current port status</returns>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<PortOperationResponse> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PortOperationResponse>("api/cat/status", cancellationToken);
                return response ?? throw new SharpCATClientException("Failed to deserialize response");
            }
            catch (HttpRequestException ex)
            {
                throw new SharpCATClientException("Failed to get status", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SharpCATClientException("Request timed out", ex);
            }
        }

        /// <summary>
        /// Sends a CAT command to the connected radio
        /// </summary>
        /// <param name="request">CAT command to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result of the command operation including any response from the radio</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<CommandResponse> SendCommandAsync(SendCommandRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync("api/cat/command", request, cancellationToken);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<CommandResponse>(cancellationToken: cancellationToken);
                    return response ?? throw new SharpCATClientException("Failed to deserialize response");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                    throw new SharpCATClientException($"API returned error: {errorResponse?.Error ?? "Unknown error"}", errorResponse?.Details);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new SharpCATClientException("Failed to send command", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SharpCATClientException("Request timed out", ex);
            }
        }

        /// <summary>
        /// Sends a CAT command to the connected radio
        /// </summary>
        /// <param name="command">CAT command string to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Result of the command operation including any response from the radio</returns>
        /// <exception cref="ArgumentNullException">Thrown when command is null</exception>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<CommandResponse> SendCommandAsync(string command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            
            var request = new SendCommandRequest { Command = command };
            return await SendCommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Gets the current frequency from the radio (VFO A)
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current frequency in Hz, or null if the command failed or returned no data</returns>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<long?> GetFrequencyAsync(CancellationToken cancellationToken = default)
        {
            var response = await SendCommandAsync("FA;", cancellationToken);
            
            if (!response.Success || string.IsNullOrEmpty(response.Response))
                return null;

            // Parse frequency response: FA00014074000; -> 14074000 Hz
            var frequencyStr = response.Response;
            if (!string.IsNullOrEmpty(frequencyStr) && frequencyStr.StartsWith("FA") && frequencyStr.EndsWith(";"))
            {
                var freqPart = frequencyStr.Substring(2, frequencyStr.Length - 3);
                if (long.TryParse(freqPart, out var frequency))
                {
                    return frequency;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the frequency for the radio (VFO A)
        /// </summary>
        /// <param name="frequencyHz">Frequency in Hz</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the command was sent successfully</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when frequency is out of valid range</exception>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<bool> SetFrequencyAsync(long frequencyHz, CancellationToken cancellationToken = default)
        {
            if (frequencyHz < 0 || frequencyHz > 999999999999)
                throw new ArgumentOutOfRangeException(nameof(frequencyHz), "Frequency must be between 0 and 999,999,999,999 Hz");

            // Format frequency as 11-digit string with leading zeros
            var command = $"FA{frequencyHz:D11};";
            var response = await SendCommandAsync(command, cancellationToken);
            
            return response.Success;
        }

        /// <summary>
        /// Gets the current frequency from the radio (VFO B)
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current frequency in Hz, or null if the command failed or returned no data</returns>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<long?> GetFrequencyBAsync(CancellationToken cancellationToken = default)
        {
            var response = await SendCommandAsync("FB;", cancellationToken);
            
            if (!response.Success || string.IsNullOrEmpty(response.Response))
                return null;

            // Parse frequency response: FB00014074000; -> 14074000 Hz
            var frequencyStr = response.Response;
            if (!string.IsNullOrEmpty(frequencyStr) && frequencyStr.StartsWith("FB") && frequencyStr.EndsWith(";"))
            {
                var freqPart = frequencyStr.Substring(2, frequencyStr.Length - 3);
                if (long.TryParse(freqPart, out var frequency))
                {
                    return frequency;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the frequency for the radio (VFO B)
        /// </summary>
        /// <param name="frequencyHz">Frequency in Hz</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the command was sent successfully</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when frequency is out of valid range</exception>
        /// <exception cref="SharpCATClientException">Thrown when the API call fails</exception>
        public async Task<bool> SetFrequencyBAsync(long frequencyHz, CancellationToken cancellationToken = default)
        {
            if (frequencyHz < 0 || frequencyHz > 999999999999)
                throw new ArgumentOutOfRangeException(nameof(frequencyHz), "Frequency must be between 0 and 999,999,999,999 Hz");

            // Format frequency as 11-digit string with leading zeros
            var command = $"FB{frequencyHz:D11};";
            var response = await SendCommandAsync(command, cancellationToken);
            
            return response.Success;
        }

        /// <summary>
        /// Disposes the client and releases resources
        /// </summary>
        public void Dispose()
        {
            if (_ownsHttpClient)
            {
                _httpClient?.Dispose();
            }
        }
    }
}