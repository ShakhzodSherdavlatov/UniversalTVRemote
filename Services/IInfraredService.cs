namespace UniversalTVRemote.Services;

/// <summary>
/// Interface for infrared transmission service
/// </summary>
public interface IInfraredService
{
    /// <summary>
    /// Check if the device has IR transmitter hardware
    /// </summary>
    bool HasIREmitter();

    /// <summary>
    /// Get available IR carrier frequencies
    /// </summary>
    int[] GetCarrierFrequencies();

    /// <summary>
    /// Transmit an IR signal
    /// </summary>
    /// <param name="frequency">Carrier frequency in Hz</param>
    /// <param name="pattern">Pattern of pulses and spaces in microseconds</param>
    /// <returns>True if transmission was successful</returns>
    Task<bool> TransmitAsync(int frequency, int[] pattern);

    /// <summary>
    /// Transmit an IR signal from hex code (converts to pattern)
    /// </summary>
    /// <param name="frequency">Carrier frequency in Hz</param>
    /// <param name="hexCode">Hex code string</param>
    /// <param name="protocol">Protocol type (NEC, Sony, etc.)</param>
    /// <returns>True if transmission was successful</returns>
    Task<bool> TransmitHexAsync(int frequency, string hexCode, string protocol);
}
