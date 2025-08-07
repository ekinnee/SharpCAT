# SharpCAT

**SharpCAT** is a C#/.NET cross-platform library and Web API server for CAT (Computer Aided Transceiver) radio control. It supports serial port CAT operations, modern async programming, and provides a REST API for remote radio control.

---

## Features

- **Core Library**: .NET Standard-based, works with .NET Core and .NET Framework
- **Cross-platform Server**: ASP.NET Core Web API server, works on Windows, Linux, and macOS
- **REST API**: Endpoints for serial port management and sending arbitrary CAT commands
- **Swagger/OpenAPI**: Interactive API docs and testing interface
- **Async/Await**: Modern async support for non-blocking IO
- **Extensible**: Designed for adding radios and protocols (FT818, ID-4100A, TH-D74A, etc.)
- **Built-in Logging and Error Handling**

---

## Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later

### Running the Web API Server

```bash
cd Server/SharpCAT.Server
dotnet run
```
The server listens on `http://localhost:5188` by default.  
Open [http://localhost:5188](http://localhost:5188) in your browser for the Swagger UI.

---

## Usage Examples

### List Available Serial Ports

```bash
curl http://localhost:5188/api/cat/ports
```

### Open a Serial Port

```bash
curl -X POST http://localhost:5188/api/cat/open \
  -H "Content-Type: application/json" \
  -d '{"portName": "COM3", "baudRate": 9600}'
```

### Send a CAT Command

```bash
curl -X POST http://localhost:5188/api/cat/command \
  -H "Content-Type: application/json" \
  -d '{"command": "FA;"}'
```

---

## Project Structure

```
Library/                # Core CAT library (cross-platform)
Server/SharpCAT.Server/  # ASP.NET Core Web API server
```

---

## Development

### Build Everything

```bash
dotnet build
```

### Build Just the Core Library

```bash
dotnet build Library/Library.csproj
```

### VS Code Tasks

This repo includes VS Code tasks for building, cleaning, restoring, publishing, and watching changes.

---

## Roadmap & Ideas

- Implement generic IRadio interface and radio-specific drivers
- Support multiple concurrent radios
- Fully async serial operations
- Remote sharing of serial ports
- Implement flrig protocol support
- Potential AGWPE integration
- Service/daemon operation support
- Additional radio protocols (CAT, CI-V, etc.)

---

## Why?

While Hamlib and HamLibSharp exist, there is a need for a modern, pure .NET CAT control library and server for direct integration with C# projects, without C++/PInvoke dependencies.

---

## Contributing

Contributions are very welcome! Please open issues, submit PRs, or share your ideas.

---

## License

This project follows the same license as the parent SharpCAT project.

---

## Credits

- Inspired by FT818, ID-4100A, TH-D74A, Yaesu FT991a, and others
- Thanks to all open source contributors and radio amateurs for protocol documentation and ideas

---

*If you wish to help, let me know or create a pull request. I'm not a pro developer, just a hobbyist building tools for myself and others.*
