namespace SharpCAT.Common.CAT;

/// <summary>
/// Interface for CAT communication
/// </summary>
public interface ICATInterface
{
    /// <summary>
    /// Sends a CAT command and waits for a response
    /// </summary>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The response from the radio</returns>
    Task<CATResponse> SendCommandAsync(CATCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a CAT command without waiting for a response
    /// </summary>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SendCommandOnlyAsync(CATCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the CAT interface is connected and ready
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Event fired when a response is received
    /// </summary>
    event EventHandler<CATResponse>? ResponseReceived;

    /// <summary>
    /// Event fired when an error occurs
    /// </summary>
    event EventHandler<string>? ErrorOccurred;
}