namespace UniversalTVRemote.Services;

/// <summary>
/// Cross-platform infrared service
/// The actual implementation is in Platforms/Android/InfraredService.cs
/// This file exists for non-Android platforms (stub implementation)
/// </summary>
public partial class InfraredService : IInfraredService
{
#if !ANDROID
    public bool HasIREmitter()
    {
        return false;
    }

    public int[] GetCarrierFrequencies()
    {
        return Array.Empty<int>();
    }

    public Task<bool> TransmitAsync(int frequency, int[] pattern)
    {
        return Task.FromResult(false);
    }

    public Task<bool> TransmitHexAsync(int frequency, string hexCode, string protocol)
    {
        return Task.FromResult(false);
    }
#endif
}
