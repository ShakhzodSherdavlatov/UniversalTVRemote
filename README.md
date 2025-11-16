# Universal TV Remote

A modern Android TV remote control app built with .NET MAUI and C#. Control your TV using your phone's infrared (IR) transmitter.

## Features

- **IR Remote Control**: Full TV control using your phone's IR blaster
- **Pre-configured Codes**: Built-in support for Samsung, LG, and Sony TVs
- **Manual Code Entry**: Add custom IR codes for any TV brand
  - Hex code input (NEC, Sony, RC5, RC6 protocols)
  - Raw pattern input (comma-separated timings)
  - Test codes before saving
- **SQLite Database**: Local storage for all IR codes
- **Modern UI**: Clean, dark-themed interface optimized for one-handed use
- **Multiple Profiles**: Save different TV configurations

## Tech Stack

- **.NET MAUI** (Multi-platform App UI)
- **C# 12** with .NET 8
- **SQLite** for local data storage
- **MVVM Architecture** (Model-View-ViewModel)
- **Android ConsumerIrManager API** for IR transmission

## Remote Features

- Power On/Off
- Volume Up/Down
- Channel Up/Down
- Mute
- Navigation (Menu, OK, Back, Home)
- Custom button mapping

## Requirements

**Hardware:**
- Android phone with IR blaster (infrared transmitter)
- Examples: Samsung Galaxy S4/S5/S6, Xiaomi Redmi Note series, Huawei Mate/P series

**Software:**
- Android 5.0 (API 21) or higher
- .NET 8 SDK (for building)

## Quick Start

1. Clone the repository
2. Open `UniversalTVRemote.csproj` in Visual Studio 2022
3. Select Android as target platform
4. Build and deploy to your Android device

See [BUILD_INSTRUCTIONS.md](BUILD_INSTRUCTIONS.md) for detailed build instructions.

## How to Use

1. **First Launch**: Select your TV brand from the dropdown
2. **Test Buttons**: Try the Power or Volume buttons
3. **If Codes Don't Work**:
   - Tap "Add Custom IR Code"
   - Enter codes found online or from your TV manual
   - Test and save

## Manual Code Entry

If pre-configured codes don't work for your TV:

1. Find IR codes online (LIRC database, manufacturer sites, forums)
2. Use the "Add Custom IR Code" button
3. Enter code in either format:
   - **Hex Code**: `0xE0E040BF` (for NEC/Sony protocols)
   - **Raw Pattern**: `9000,4500,560,560...` (timing in microseconds)
4. Test the code before saving

## Project Structure

```
UniversalTVRemote/
├── Models/              # IRCode, TVProfile data models
├── Services/            # InfraredService, IRCodeDatabase
├── ViewModels/          # MainViewModel, ManualCodeEntryViewModel
├── Views/               # MainPage, ManualCodeEntryPage (XAML)
├── Platforms/Android/   # Android-specific IR implementation
└── Resources/           # Styles, fonts, images
```

## Why .NET MAUI?

- **C# Native**: Perfect for C# developers
- **Cross-platform**: Can expand to iOS (for WiFi/Bluetooth remotes)
- **Modern**: Actively supported by Microsoft
- **Native Performance**: Direct access to Android APIs

## Adding More TV Brands

Edit `Services/IRCodeDatabase.cs` and add codes to the `SeedInitialDataAsync` method:

```csharp
new IRCode
{
    Brand = "YourBrand",
    Function = "Power",
    Frequency = 38000,
    Pattern = "9000,4500,560,560...",
    Protocol = "NEC",
    HexCode = "0x12345678",
    IsCustom = false
}
```

## Known Limitations

- Requires physical IR hardware (most modern phones don't have IR)
- Cannot learn codes from existing remotes (most phones lack IR receivers)
- Code compatibility varies by TV model

## Future Enhancements

- [ ] WiFi/Network remote support (for smart TVs)
- [ ] Online IR code database integration
- [ ] Macros (combine multiple commands)
- [ ] Widget support
- [ ] More TV brands pre-configured
- [ ] Import/Export code databases

## Contributing

Feel free to contribute by:
- Adding IR codes for more TV brands
- Reporting bugs
- Suggesting features
- Improving documentation

## License

This project is for personal use. IR codes may be subject to their respective manufacturers' rights.

## Acknowledgments

- IR codes sourced from LIRC database and community contributions
- Built for learning .NET MAUI and Android IR APIs
