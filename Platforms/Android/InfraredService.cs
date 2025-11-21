using Android.Content;
using Android.Hardware;

namespace UniversalTVRemote.Services;

/// <summary>
/// Android-specific implementation of the infrared service using ConsumerIrManager
/// </summary>
public partial class InfraredService : IInfraredService
{
    private readonly ConsumerIrManager? _irManager;

    public InfraredService()
    {
        try
        {
            var context = Platform.CurrentActivity?.ApplicationContext;
            if (context != null)
            {
                _irManager = (ConsumerIrManager?)context.GetSystemService(Context.ConsumerIrService);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing IR service: {ex.Message}");
        }
    }

    public bool HasIREmitter()
    {
        return _irManager?.HasIrEmitter ?? false;
    }

    public int[] GetCarrierFrequencies()
    {
        // Return common IR frequencies that work for most TVs
        // 38kHz is the most common, followed by 36kHz, 40kHz, and 56kHz
        return new[] { 30000, 33000, 36000, 38000, 40000, 56000 };
    }

    public Task<bool> TransmitAsync(int frequency, int[] pattern)
    {
        if (_irManager?.HasIrEmitter != true)
        {
            System.Diagnostics.Debug.WriteLine("No IR emitter available");
            return Task.FromResult(false);
        }

        if (pattern == null || pattern.Length == 0)
        {
            System.Diagnostics.Debug.WriteLine("Invalid IR pattern");
            return Task.FromResult(false);
        }

        try
        {
            // Transmit the IR signal
            _irManager.Transmit(frequency, pattern);
            System.Diagnostics.Debug.WriteLine($"Transmitted IR signal: Freq={frequency}Hz, Pattern length={pattern.Length}");
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error transmitting IR signal: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public async Task<bool> TransmitHexAsync(int frequency, string hexCode, string protocol)
    {
        try
        {
            // Convert hex code to pattern based on protocol
            var pattern = ConvertHexToPattern(hexCode, protocol);
            if (pattern != null)
            {
                return await TransmitAsync(frequency, pattern);
            }

            System.Diagnostics.Debug.WriteLine($"Failed to convert hex code: {hexCode} with protocol: {protocol}");
            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in TransmitHexAsync: {ex.Message}");
            return false;
        }
    }

    private int[]? ConvertHexToPattern(string hexCode, string protocol)
    {
        // Remove "0x" prefix if present
        hexCode = hexCode.Replace("0x", "").Replace("0X", "");

        try
        {
            switch (protocol.ToUpper())
            {
                case "NEC":
                    return ConvertNECHexToPattern(hexCode);
                case "SONY":
                    return ConvertSonyHexToPattern(hexCode);
                default:
                    System.Diagnostics.Debug.WriteLine($"Unsupported protocol: {protocol}");
                    return null;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error converting hex to pattern: {ex.Message}");
            return null;
        }
    }

    private int[] ConvertNECHexToPattern(string hexCode)
    {
        // NEC protocol: 32-bit code
        // Format: 9ms burst + 4.5ms space + data bits + stop bit
        if (hexCode.Length != 8)
        {
            throw new ArgumentException("NEC hex code must be 8 characters (32 bits)");
        }

        var code = Convert.ToUInt32(hexCode, 16);
        var pattern = new List<int>();

        // Start burst (9ms) and space (4.5ms)
        pattern.Add(9000);
        pattern.Add(4500);

        // Data bits (LSB first)
        for (int i = 0; i < 32; i++)
        {
            pattern.Add(560); // Pulse

            // Space duration depends on bit value
            if ((code & (1 << i)) != 0)
                pattern.Add(1690); // Logic 1: 1.69ms space
            else
                pattern.Add(560);  // Logic 0: 0.56ms space
        }

        // Stop bit
        pattern.Add(560);

        return pattern.ToArray();
    }

    private int[] ConvertSonyHexToPattern(string hexCode)
    {
        // Sony protocol: 12, 15, or 20-bit code
        // Format: 2.4ms burst + data bits (no stop bit)
        var code = Convert.ToUInt32(hexCode, 16);
        var pattern = new List<int>();

        // Determine bit count based on hex length
        int bitCount = hexCode.Length <= 3 ? 12 : (hexCode.Length <= 4 ? 15 : 20);

        // Start burst
        pattern.Add(2400);

        // Data bits (LSB first)
        for (int i = 0; i < bitCount; i++)
        {
            if ((code & (1 << i)) != 0)
            {
                pattern.Add(1200); // Logic 1: 1.2ms pulse
                pattern.Add(600);  // 0.6ms space
            }
            else
            {
                pattern.Add(600);  // Logic 0: 0.6ms pulse
                pattern.Add(600);  // 0.6ms space
            }
        }

        return pattern.ToArray();
    }
}
