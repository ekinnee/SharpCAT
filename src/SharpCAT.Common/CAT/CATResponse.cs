namespace SharpCAT.Common.CAT;

/// <summary>
/// Represents the response from a CAT command
/// </summary>
public class CATResponse
{
    /// <summary>
    /// The original command that generated this response
    /// </summary>
    public CATCommand Command { get; set; } = new();

    /// <summary>
    /// Whether the command was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Raw response bytes from the radio
    /// </summary>
    public byte[] ResponseBytes { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Parsed response data (if applicable)
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();

    /// <summary>
    /// Error message if the command failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Timestamp when the response was received
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Duration it took to receive the response
    /// </summary>
    public TimeSpan Duration { get; set; }
}