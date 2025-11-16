using SQLite;

namespace UniversalTVRemote.Models;

/// <summary>
/// Represents a saved TV profile with associated IR codes
/// </summary>
[Table("TVProfiles")]
public class TVProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// User-friendly name for this TV (e.g., "Living Room TV", "Bedroom Samsung")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// TV Brand
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// TV Model
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Whether this is the currently active profile
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date created
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;

    /// <summary>
    /// Last used
    /// </summary>
    public DateTime LastUsed { get; set; } = DateTime.Now;
}
