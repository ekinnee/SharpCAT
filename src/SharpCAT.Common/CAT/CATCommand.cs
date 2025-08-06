namespace SharpCAT.Common.CAT;

/// <summary>
/// Represents a CAT (Computer Aided Transceiver) command
/// </summary>
public class CATCommand
{
    /// <summary>
    /// Unique identifier for the command
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable name of the command
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of what the command does
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The raw command bytes to send to the radio
    /// </summary>
    public byte[] CommandBytes { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Expected response pattern (if any)
    /// </summary>
    public byte[]? ExpectedResponse { get; set; }

    /// <summary>
    /// Parameters that can be included with the command
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Command timeout in milliseconds
    /// </summary>
    public int TimeoutMs { get; set; } = 1000;
}