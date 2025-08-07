# SharpCAT Server

A cross-platform ASP.NET Core Web API server that provides REST endpoints for CAT (Computer Aided Transceiver) control using the SharpCAT library.

## Features

- **Cross-platform**: Works on Windows, Linux, and macOS
- **REST API**: Clean, documented REST endpoints for radio control
- **Serial Communication**: Leverages the SharpCAT core library for serial port operations
- **Swagger Documentation**: Built-in API documentation and testing interface
- **Error Handling**: Comprehensive error handling and logging

## API Endpoints

### Serial Port Management

- `GET /api/cat/ports` - List all available serial ports
- `POST /api/cat/open` - Open and configure a serial port
- `POST /api/cat/close` - Close the currently opened port
- `GET /api/cat/status` - Get current port connection status

### CAT Commands

- `POST /api/cat/command` - Send arbitrary CAT commands to the radio

### Health Check

- `GET /health` - Simple health check endpoint

## Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later

### Running the Server

1. Navigate to the server directory:
   ```bash
   cd Server/SharpCAT.Server
   ```

2. Build and run the server:
   ```bash
   dotnet run
   ```

3. The server will start and listen on `http://localhost:5188` by default

4. Open your browser to `http://localhost:5188` to access the Swagger UI for interactive API documentation

### Example Usage

#### List Available Ports
```bash
curl http://localhost:5188/api/cat/ports
```

#### Open a Serial Port
```bash
curl -X POST http://localhost:5188/api/cat/open \
  -H "Content-Type: application/json" \
  -d '{
    "portName": "COM3",
    "baudRate": 9600,
    "parity": "None",
    "stopBits": "One",
    "handshake": "None"
  }'
```

#### Send a CAT Command
```bash
curl -X POST http://localhost:5188/api/cat/command \
  -H "Content-Type: application/json" \
  -d '{"command": "FA;"}'
```

#### Check Status
```bash
curl http://localhost:5188/api/cat/status
```

## Configuration

### Port Configuration Options

When opening a port, you can configure:

- **Port Name**: Serial port name (e.g., "COM1" on Windows, "/dev/ttyUSB0" on Linux)
- **Baud Rate**: Communication speed (default: 9600)
- **Parity**: Error checking method (None, Odd, Even, Mark, Space)
- **Stop Bits**: Number of stop bits (None, One, Two, OnePointFive)
- **Handshake**: Flow control method (None, XOnXOff, RequestToSend, RequestToSendXOnXOff)

### Logging

The server uses ASP.NET Core's built-in logging. Log levels can be configured in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "SharpCAT.Server": "Debug"
    }
  }
}
```

## Development

### Building from Source

1. Ensure you have the .NET 8.0 SDK installed
2. Clone the repository
3. Navigate to the project root
4. Build the solution:
   ```bash
   dotnet build
   ```

### Project Structure

```
Server/SharpCAT.Server/
├── Controllers/
│   └── CatController.cs          # Main API controller
├── Models/
│   └── ApiModels.cs             # Request/response models
├── Services/
│   ├── ISerialCommunicationService.cs    # Service interface
│   └── SerialCommunicationService.cs     # Service implementation
├── Program.cs                   # Application entry point
└── SharpCAT.Server.csproj      # Project file
```

### Dependencies

- **Microsoft.AspNetCore.OpenApi** - OpenAPI support
- **Swashbuckle.AspNetCore** - Swagger documentation
- **SharpCAT** - Core CAT control library

## License

This project follows the same license as the parent SharpCAT project.

## Contributing

Contributions are welcome! Please follow the existing code style and add appropriate tests for new features.