using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SharpCAT.Common.Models;

namespace SharpCAT.Common.Certificates;

/// <summary>
/// Certificate manager for TLS mutual authentication
/// </summary>
public class CertificateManager
{
    private readonly CertificateConfig _config;

    public CertificateManager(CertificateConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Ensures all required certificates exist, generating them if necessary
    /// </summary>
    public async Task EnsureCertificatesExistAsync()
    {
        // Create certificate directory if it doesn't exist
        if (!Directory.Exists(_config.CertificateStorePath))
        {
            Directory.CreateDirectory(_config.CertificateStorePath);
        }

        // Generate CA certificate if it doesn't exist
        var caCertPath = GetCACertificatePath();
        var caKeyPath = GetCAKeyPath();
        
        if (!File.Exists(caCertPath) || !File.Exists(caKeyPath))
        {
            await GenerateCACertificateAsync();
        }

        // Generate server certificate if it doesn't exist
        var serverCertPath = GetServerCertificatePath();
        var serverKeyPath = GetServerKeyPath();
        
        if (!File.Exists(serverCertPath) || !File.Exists(serverKeyPath))
        {
            await GenerateServerCertificateAsync();
        }

        // Generate client certificate if it doesn't exist
        var clientCertPath = GetClientCertificatePath();
        var clientKeyPath = GetClientKeyPath();
        
        if (!File.Exists(clientCertPath) || !File.Exists(clientKeyPath))
        {
            await GenerateClientCertificateAsync();
        }
    }

    /// <summary>
    /// Loads the CA certificate
    /// </summary>
    public X509Certificate2 LoadCACertificate()
    {
        var certPath = GetCACertificatePath();
        if (!File.Exists(certPath))
            throw new FileNotFoundException($"CA certificate not found at {certPath}");

        return new X509Certificate2(certPath);
    }

    /// <summary>
    /// Loads the server certificate with private key
    /// </summary>
    public X509Certificate2 LoadServerCertificate()
    {
        var certPath = GetServerCertificatePath();
        var keyPath = GetServerKeyPath();
        
        if (!File.Exists(certPath))
            throw new FileNotFoundException($"Server certificate not found at {certPath}");
        
        if (!File.Exists(keyPath))
            throw new FileNotFoundException($"Server private key not found at {keyPath}");

        return LoadCertificateWithKey(certPath, keyPath);
    }

    /// <summary>
    /// Loads the client certificate with private key
    /// </summary>
    public X509Certificate2 LoadClientCertificate()
    {
        var certPath = GetClientCertificatePath();
        var keyPath = GetClientKeyPath();
        
        if (!File.Exists(certPath))
            throw new FileNotFoundException($"Client certificate not found at {certPath}");
        
        if (!File.Exists(keyPath))
            throw new FileNotFoundException($"Client private key not found at {keyPath}");

        return LoadCertificateWithKey(certPath, keyPath);
    }

    /// <summary>
    /// Validates a certificate against the CA
    /// </summary>
    public bool ValidateCertificate(X509Certificate2 certificate)
    {
        try
        {
            var caCert = LoadCACertificate();
            var chain = new X509Chain();
            chain.ChainPolicy.ExtraStore.Add(caCert);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

            return chain.Build(certificate);
        }
        catch
        {
            return false;
        }
    }

    private async Task GenerateCACertificateAsync()
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=SharpCAT-CA", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        // Add extensions for CA certificate
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, true));
        
        var certificate = request.CreateSelfSigned(
            DateTimeOffset.Now.AddDays(-1), 
            DateTimeOffset.Now.AddDays(_config.ValidityDays));

        // Save certificate
        var certBytes = certificate.Export(X509ContentType.Cert);
        await File.WriteAllBytesAsync(GetCACertificatePath(), certBytes);

        // Save private key
        var keyBytes = rsa.ExportRSAPrivateKey();
        var keyPem = ConvertToPem(keyBytes, "RSA PRIVATE KEY");
        await File.WriteAllTextAsync(GetCAKeyPath(), keyPem);
    }

    private async Task GenerateServerCertificateAsync()
    {
        var caCert = LoadCACertificate();
        var caKey = LoadPrivateKey(GetCAKeyPath());

        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=SharpCAT-Server", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        // Add extensions for server certificate
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, true)); // Server Authentication
        
        var certificate = request.Create(caCert, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(_config.ValidityDays), new byte[4]);

        // Save certificate
        var certBytes = certificate.Export(X509ContentType.Cert);
        await File.WriteAllBytesAsync(GetServerCertificatePath(), certBytes);

        // Save private key
        var keyBytes = rsa.ExportRSAPrivateKey();
        var keyPem = ConvertToPem(keyBytes, "RSA PRIVATE KEY");
        await File.WriteAllTextAsync(GetServerKeyPath(), keyPem);
    }

    private async Task GenerateClientCertificateAsync()
    {
        var caCert = LoadCACertificate();
        var caKey = LoadPrivateKey(GetCAKeyPath());

        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=SharpCAT-Client", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        // Add extensions for client certificate
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, true));
        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.2") }, true)); // Client Authentication
        
        var certificate = request.Create(caCert, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(_config.ValidityDays), new byte[4]);

        // Save certificate
        var certBytes = certificate.Export(X509ContentType.Cert);
        await File.WriteAllBytesAsync(GetClientCertificatePath(), certBytes);

        // Save private key
        var keyBytes = rsa.ExportRSAPrivateKey();
        var keyPem = ConvertToPem(keyBytes, "RSA PRIVATE KEY");
        await File.WriteAllTextAsync(GetClientKeyPath(), keyPem);
    }

    private X509Certificate2 LoadCertificateWithKey(string certPath, string keyPath)
    {
        var cert = new X509Certificate2(certPath);
        var key = LoadPrivateKey(keyPath);
        return cert.CopyWithPrivateKey(key);
    }

    private RSA LoadPrivateKey(string keyPath)
    {
        var keyPem = File.ReadAllText(keyPath);
        var keyBytes = ConvertFromPem(keyPem, "RSA PRIVATE KEY");
        
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(keyBytes, out _);
        return rsa;
    }

    private static string ConvertToPem(byte[] data, string type)
    {
        var base64 = Convert.ToBase64String(data);
        var sb = new StringBuilder();
        sb.AppendLine($"-----BEGIN {type}-----");
        
        for (int i = 0; i < base64.Length; i += 64)
        {
            var length = Math.Min(64, base64.Length - i);
            sb.AppendLine(base64.Substring(i, length));
        }
        
        sb.AppendLine($"-----END {type}-----");
        return sb.ToString();
    }

    private static byte[] ConvertFromPem(string pem, string type)
    {
        var startMarker = $"-----BEGIN {type}-----";
        var endMarker = $"-----END {type}-----";
        
        var start = pem.IndexOf(startMarker) + startMarker.Length;
        var end = pem.IndexOf(endMarker);
        
        var base64 = pem.Substring(start, end - start).Trim();
        return Convert.FromBase64String(base64);
    }

    private string GetCACertificatePath() => 
        _config.CaCertificatePath ?? Path.Combine(_config.CertificateStorePath, "ca.crt");

    private string GetCAKeyPath() => 
        Path.Combine(_config.CertificateStorePath, "ca.key");

    private string GetServerCertificatePath() => 
        _config.ServerCertificatePath ?? Path.Combine(_config.CertificateStorePath, "server.crt");

    private string GetServerKeyPath() => 
        _config.ServerKeyPath ?? Path.Combine(_config.CertificateStorePath, "server.key");

    private string GetClientCertificatePath() => 
        _config.ClientCertificatePath ?? Path.Combine(_config.CertificateStorePath, "client.crt");

    private string GetClientKeyPath() => 
        _config.ClientKeyPath ?? Path.Combine(_config.CertificateStorePath, "client.key");
}