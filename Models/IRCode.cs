using SQLite;

namespace UniversalTVRemote.Models;

/// <summary>
/// Represents an infrared code for a specific TV function
/// </summary>
[Table("IRCodes")]
public class IRCode
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// TV Brand (e.g., Samsung, LG, Sony)
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// TV Model (optional, for specific models)
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Function name (e.g., Power, VolumeUp, VolumeDown, ChannelUp, ChannelDown)
    /// </summary>
    public string Function { get; set; } = string.Empty;

    /// <summary>
    /// IR Frequency in Hz (typically 38000 for most TVs)
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// IR Pattern as comma-separated integers (pulse/space timings in microseconds)
    /// Example: "9000,4500,560,560,560,1690,560,560..."
    /// </summary>
    public string Pattern { get; set; } = string.Empty;

    /// <summary>
    /// Protocol type (NEC, Sony, RC5, etc.) - optional
    /// </summary>
    public string? Protocol { get; set; }

    /// <summary>
    /// Hex code representation (for manual entry) - optional
    /// Example: "0x20DF10EF" for NEC protocol
    /// </summary>
    public string? HexCode { get; set; }

    /// <summary>
    /// User-defined code (marked as custom)
    /// </summary>
    public bool IsCustom { get; set; }

    /// <summary>
    /// Notes or description
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Date added
    /// </summary>
    public DateTime DateAdded { get; set; } = DateTime.Now;
}
