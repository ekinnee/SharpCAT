# SharpCAT.Client

A .NET Standard 2.0 client library that provides a typed C# API for consuming the SharpCAT Server HTTP API endpoints. This library makes it easy to integrate CAT (Computer Aided Transceiver) control into your .NET applications.

## Features

- **Typed API**: Strongly-typed C# methods for all SharpCAT Server endpoints
- **Async/Await Support**: Modern async programming with cancellation token support
- **Error Handling**: Comprehensive error handling with custom exceptions
- **Cross-Platform**: .NET Standard 2.0 compatible (works with .NET Core, .NET Framework, .NET 5+)
- **HTTP Client Integration**: Uses System.Net.Http.Json for JSON serialization
- **Frequency Helpers**: Convenient methods for getting/setting radio frequencies
- **XML Documentation**: Full IntelliSense support with comprehensive documentation

## Installation

Add the SharpCAT.Client project reference to your application:

```xml
<ProjectReference Include="path/to/SharpCAT.Client/SharpCAT.Client.csproj" />
```

## Quick Start

### Basic Usage

```csharp
using SharpCAT.Client;
using SharpCAT.Client.Models;

// Create client instance
using var client = new SharpCATClient("http://localhost:5188");

// Get available serial ports
var ports = await client.GetPortsAsync();
Console.WriteLine($"Available ports: {string.Join(", ", ports.Ports)}");

// Open a serial port
var openRequest = new OpenPortRequest
{
    PortName = "COM3",
    BaudRate = 9600
};
var openResult = await client.OpenPortAsync(openRequest);
Console.WriteLine($"Port opened: {openResult.Success}");

// Send a raw CAT command
var commandResult = await client.SendCommandAsync("FA;");
Console.WriteLine($"Command response: {commandResult.Response}");

// Use convenience methods for frequency operations
var frequency = await client.GetFrequencyAsync();
Console.WriteLine($"Current frequency: {frequency} Hz");

await client.SetFrequencyAsync(14074000); // Set to 14.074 MHz
```

### Advanced Usage

```csharp
// Use with dependency injection and HttpClientFactory
services.AddHttpClient<SharpCATClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5188");
});

// Handle errors
try
{
    var result = await client.SendCommandAsync("invalid_command");
}
catch (SharpCATClientException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    if (ex.Details != null)
        Console.WriteLine($"Details: {ex.Details}");
}

// Use cancellation tokens
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var status = await client.GetStatusAsync(cts.Token);
```

## API Reference

### Core Methods

#### Port Management
- `GetPortsAsync()` - Get all available serial ports
- `OpenPortAsync(request)` - Open and configure a serial port
- `ClosePortAsync()` - Close the current serial port
- `GetStatusAsync()` - Get current port connection status

#### CAT Commands
- `SendCommandAsync(command)` - Send a raw CAT command string
- `SendCommandAsync(request)` - Send a CAT command with full request object

#### Frequency Operations (Convenience Methods)
- `GetFrequencyAsync()` - Get current frequency (VFO A)
- `SetFrequencyAsync(frequencyHz)` - Set frequency (VFO A)
- `GetFrequencyBAsync()` - Get current frequency (VFO B)
- `SetFrequencyBAsync(frequencyHz)` - Set frequency (VFO B)

### Configuration Options

When opening a port, you can configure:

```csharp
var request = new OpenPortRequest
{
    PortName = "COM3",           // Required
    BaudRate = 9600,             // Default: 9600
    Parity = Parity.None,        // Default: None
    StopBits = StopBits.One,     // Default: One
    Handshake = Handshake.None   // Default: None
};
```

### Error Handling

The client throws `SharpCATClientException` for various error conditions:

- HTTP request failures
- Network timeouts
- API error responses
- Deserialization failures

```csharp
try
{
    var result = await client.GetPortsAsync();
}
catch (SharpCATClientException ex)
{
    // Handle SharpCAT-specific errors
    Console.WriteLine($"SharpCAT Error: {ex.Message}");
}
catch (Exception ex)
{
    // Handle other errors
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

## Common CAT Commands

Here are some common CAT commands you can send using `SendCommandAsync()`:

| Command | Description | Example Response |
|---------|-------------|------------------|
| `FA;` | Get VFO A frequency | `FA00014074000;` |
| `FB;` | Get VFO B frequency | `FB00007074000;` |
| `FA14074000;` | Set VFO A frequency | No response or `FA14074000;` |
| `MD;` | Get operating mode | `MD2;` (USB) |
| `MD2;` | Set mode to USB | No response |
| `IF;` | Get radio information | `IF00014074000...;` |

**Note**: The exact commands and responses depend on your radio model. Consult your radio's CAT documentation for complete command reference.

## Thread Safety

The `SharpCATClient` class is thread-safe for concurrent read operations, but write operations (like sending commands) should be serialized to avoid conflicts at the radio level.

## Disposal

The client implements `IDisposable`. When you create a client with a base address string or URI, it owns the internal `HttpClient` and will dispose it. If you pass in your own `HttpClient`, the client will not dispose it.

```csharp
// Client owns HttpClient - will be disposed
using var client = new SharpCATClient("http://localhost:5188");

// You own HttpClient - manage disposal yourself
var httpClient = new HttpClient();
var client = new SharpCATClient(httpClient);
// Don't forget to dispose httpClient when done
```

## Requirements

- .NET Standard 2.0 or later
- SharpCAT Server running and accessible
- Network connectivity to the server

## Dependencies

- System.Net.Http.Json (5.0.0)
- System.Text.Json (5.0.2)
- System.ComponentModel.Annotations (5.0.0)

## License

This project follows the same license as the parent SharpCAT project.

## Contributing

Contributions are welcome! Please follow the existing code style and add appropriate documentation for new features.