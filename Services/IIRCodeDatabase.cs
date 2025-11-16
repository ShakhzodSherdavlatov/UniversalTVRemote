using UniversalTVRemote.Models;

namespace UniversalTVRemote.Services;

/// <summary>
/// Interface for IR code database operations
/// </summary>
public interface IIRCodeDatabase
{
    /// <summary>
    /// Initialize the database
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Get all available brands
    /// </summary>
    Task<List<string>> GetBrandsAsync();

    /// <summary>
    /// Get IR codes for a specific brand and function
    /// </summary>
    Task<IRCode?> GetCodeAsync(string brand, string function, string? model = null);

    /// <summary>
    /// Get all IR codes for a brand
    /// </summary>
    Task<List<IRCode>> GetCodesForBrandAsync(string brand, string? model = null);

    /// <summary>
    /// Save a custom IR code
    /// </summary>
    Task<int> SaveCodeAsync(IRCode code);

    /// <summary>
    /// Delete an IR code
    /// </summary>
    Task<int> DeleteCodeAsync(int id);

    /// <summary>
    /// Get all custom codes
    /// </summary>
    Task<List<IRCode>> GetCustomCodesAsync();

    /// <summary>
    /// Save a TV profile
    /// </summary>
    Task<int> SaveProfileAsync(TVProfile profile);

    /// <summary>
    /// Get all TV profiles
    /// </summary>
    Task<List<TVProfile>> GetProfilesAsync();

    /// <summary>
    /// Get active profile
    /// </summary>
    Task<TVProfile?> GetActiveProfileAsync();

    /// <summary>
    /// Set active profile
    /// </summary>
    Task SetActiveProfileAsync(int profileId);
}
