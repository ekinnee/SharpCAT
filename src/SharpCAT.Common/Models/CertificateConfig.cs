namespace SharpCAT.Common.Models;

/// <summary>
/// Certificate configuration for TLS mutual authentication
/// </summary>
public class CertificateConfig
{
    /// <summary>
    /// Path to the Certificate Authority (CA) certificate file
    /// </summary>
    public string? CaCertificatePath { get; set; }

    /// <summary>
    /// Path to the server certificate file
    /// </summary>
    public string? ServerCertificatePath { get; set; }

    /// <summary>
    /// Path to the server private key file
    /// </summary>
    public string? ServerKeyPath { get; set; }

    /// <summary>
    /// Path to the client certificate file
    /// </summary>
    public string? ClientCertificatePath { get; set; }

    /// <summary>
    /// Path to the client private key file
    /// </summary>
    public string? ClientKeyPath { get; set; }

    /// <summary>
    /// Certificate store location for storing generated certificates
    /// </summary>
    public string CertificateStorePath { get; set; } = "./certificates";

    /// <summary>
    /// Whether to auto-generate certificates if they don't exist
    /// </summary>
    public bool AutoGenerateCertificates { get; set; } = true;

    /// <summary>
    /// Certificate validity period in days
    /// </summary>
    public int ValidityDays { get; set; } = 365;

    /// <summary>
    /// Subject name for generated certificates
    /// </summary>
    public string Subject { get; set; } = "CN=SharpCAT";
}