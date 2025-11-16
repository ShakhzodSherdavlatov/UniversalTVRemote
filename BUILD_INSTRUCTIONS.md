# Universal TV Remote - Build Instructions

## Prerequisites

1. **.NET 8 SDK** or later
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0

2. **Visual Studio 2022** (Windows/Mac) or **Visual Studio Code**
   - With .NET MAUI workload installed
   - For Visual Studio: Install "Mobile development with .NET" workload

3. **Android SDK**
   - API Level 21 (Android 5.0) or higher
   - Installed automatically with Visual Studio MAUI workload

## Building the Project

### Using Visual Studio 2022

1. Open `UniversalTVRemote.csproj` in Visual Studio 2022
2. Select **Android** as the target framework
3. Choose an Android emulator or physical device
4. Click **Build** → **Build Solution** (or press F6)
5. Click **Debug** → **Start Debugging** (or press F5)

### Using .NET CLI

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build -f net8.0-android

# Deploy to connected Android device
dotnet build -f net8.0-android -t:Run
```

### Creating APK for Distribution

```bash
# Create release APK
dotnet publish -f net8.0-android -c Release

# The APK will be in: bin/Release/net8.0-android/publish/
```

## Hardware Requirements

**IMPORTANT**: This app requires an Android device with an **IR blaster (infrared transmitter)**.

Devices with IR support include:
- Samsung Galaxy S series (S4, S5, S6)
- Xiaomi Redmi Note series
- Huawei Mate/P series (older models)
- LG G series

To check if your device has IR support:
- Look for "IR Blaster" in device specifications
- The app will display a warning if IR hardware is not detected

## Project Structure

```
UniversalTVRemote/
├── Models/              # Data models (IRCode, TVProfile)
├── Services/            # Business logic (IR transmission, database)
├── ViewModels/          # MVVM view models
├── Views/               # XAML UI pages
├── Platforms/
│   └── Android/        # Android-specific code
├── Resources/
│   ├── Fonts/          # App fonts
│   ├── Images/         # Images and icons
│   └── Styles/         # XAML styles
└── App.xaml            # Application entry point
```

## Features

1. **Pre-configured IR Codes**
   - Samsung, LG, and Sony TVs included
   - More brands can be added to the database

2. **Manual Code Entry**
   - Support for hex codes (NEC, Sony protocols)
   - Raw pattern input (comma-separated timings)
   - Test codes before saving

3. **SQLite Database**
   - Local storage of IR codes
   - Custom code management
   - TV profile support

## Troubleshooting

### "No IR emitter available" Error
- Your device doesn't have IR hardware
- Check device specifications for IR blaster support

### Codes Not Working
- Try different codes for your TV brand
- Use the manual code entry to add specific codes
- Verify the IR frequency (usually 38000 Hz for most TVs)

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build -f net8.0-android
```

## Adding More TV Brands

Edit `Services/IRCodeDatabase.cs` in the `SeedInitialDataAsync` method to add more IR codes.

Example:
```csharp
new IRCode
{
    Brand = "Panasonic",
    Function = "Power",
    Frequency = 38000,
    Pattern = "3500,1750,500,500...",  // Your IR pattern
    Protocol = "NEC",
    HexCode = "0x12345678",
    IsCustom = false
}
```

## License

This project is for personal use. IR code databases may have their own licenses.

## Support

For issues or questions:
- Check if your device has IR hardware
- Verify IR codes match your TV model
- Test codes using the manual entry feature
