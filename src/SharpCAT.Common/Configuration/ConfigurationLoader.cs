using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharpCAT.Common.Models;

namespace SharpCAT.Common.Configuration;

/// <summary>
/// Configuration loader for SharpCAT applications
/// </summary>
public static class ConfigurationLoader
{
    /// <summary>
    /// Loads server configuration from JSON file
    /// </summary>
    /// <param name="configPath">Path to the configuration file</param>
    /// <returns>Server configuration</returns>
    public static ServerConfig LoadServerConfig(string configPath = "server-config.json")
    {
        return LoadConfig<ServerConfig>(configPath, GetDefaultServerConfig());
    }

    /// <summary>
    /// Loads client configuration from JSON file
    /// </summary>
    /// <param name="configPath">Path to the configuration file</param>
    /// <returns>Client configuration</returns>
    public static ClientConfig LoadClientConfig(string configPath = "client-config.json")
    {
        return LoadConfig<ClientConfig>(configPath, GetDefaultClientConfig());
    }

    /// <summary>
    /// Saves server configuration to JSON file
    /// </summary>
    /// <param name="config">Configuration to save</param>
    /// <param name="configPath">Path to save the configuration file</param>
    public static async Task SaveServerConfigAsync(ServerConfig config, string configPath = "server-config.json")
    {
        await SaveConfigAsync(config, configPath);
    }

    /// <summary>
    /// Saves client configuration to JSON file
    /// </summary>
    /// <param name="config">Configuration to save</param>
    /// <param name="configPath">Path to save the configuration file</param>
    public static async Task SaveClientConfigAsync(ClientConfig config, string configPath = "client-config.json")
    {
        await SaveConfigAsync(config, configPath);
    }

    /// <summary>
    /// Creates default server configuration file if it doesn't exist
    /// </summary>
    /// <param name="configPath">Path to create the configuration file</param>
    public static async Task EnsureServerConfigExistsAsync(string configPath = "server-config.json")
    {
        if (!File.Exists(configPath))
        {
            await SaveServerConfigAsync(GetDefaultServerConfig(), configPath);
        }
    }

    /// <summary>
    /// Creates default client configuration file if it doesn't exist
    /// </summary>
    /// <param name="configPath">Path to create the configuration file</param>
    public static async Task EnsureClientConfigExistsAsync(string configPath = "client-config.json")
    {
        if (!File.Exists(configPath))
        {
            await SaveClientConfigAsync(GetDefaultClientConfig(), configPath);
        }
    }

    private static T LoadConfig<T>(string configPath, T defaultConfig) where T : class
    {
        try
        {
            if (!File.Exists(configPath))
            {
                // Create default configuration file
                var defaultJson = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                File.WriteAllText(configPath, defaultJson);
                return defaultConfig;
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: false, reloadOnChange: false)
                .Build();

            var config = configuration.Get<T>();
            return config ?? defaultConfig;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load configuration from {configPath}: {ex.Message}", ex);
        }
    }

    private static async Task SaveConfigAsync<T>(T config, string configPath) where T : class
    {
        try
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            await File.WriteAllTextAsync(configPath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save configuration to {configPath}: {ex.Message}", ex);
        }
    }

    private static ServerConfig GetDefaultServerConfig()
    {
        return new ServerConfig
        {
            Port = 8443,
            Certificate = new CertificateConfig
            {
                CertificateStorePath = "./certificates",
                AutoGenerateCertificates = true,
                ValidityDays = 365,
                Subject = "CN=SharpCAT-Server"
            },
            SerialPort = new SerialPortConfig
            {
                BaudRate = 9600,
                DataBits = 8,
                StopBits = System.IO.Ports.StopBits.One,
                Parity = System.IO.Ports.Parity.None,
                Handshake = System.IO.Ports.Handshake.None,
                ReadTimeoutMs = 1000,
                WriteTimeoutMs = 1000,
                AutoDetectPort = true
            },
            Logging = new LoggingConfig
            {
                LogLevel = "Information",
                EnableConsoleLogging = true,
                EnableOSLogging = true,
                IncludeTimestamps = true,
                IncludeLogLevel = true
            },
            MaxClients = 10
        };
    }

    private static ClientConfig GetDefaultClientConfig()
    {
        return new ClientConfig
        {
            ServerHost = "localhost",
            ServerPort = 8443,
            Certificate = new CertificateConfig
            {
                CertificateStorePath = "./certificates",
                AutoGenerateCertificates = true,
                ValidityDays = 365,
                Subject = "CN=SharpCAT-Client"
            },
            Logging = new LoggingConfig
            {
                LogLevel = "Information",
                EnableConsoleLogging = true,
                EnableOSLogging = true,
                IncludeTimestamps = true,
                IncludeLogLevel = true
            },
            ConnectionTimeoutMs = 5000
        };
    }
}