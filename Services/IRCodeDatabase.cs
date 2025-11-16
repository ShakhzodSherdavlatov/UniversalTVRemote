using SQLite;
using UniversalTVRemote.Models;

namespace UniversalTVRemote.Services;

/// <summary>
/// SQLite-based IR code database implementation
/// </summary>
public class IRCodeDatabase : IIRCodeDatabase
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public IRCodeDatabase()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "ircodes.db3");
    }

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database != null)
            return _database;

        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<IRCode>();
        await _database.CreateTableAsync<TVProfile>();

        return _database;
    }

    public async Task InitializeAsync()
    {
        var db = await GetDatabaseAsync();

        // Check if we need to seed initial data
        var count = await db.Table<IRCode>().CountAsync();
        if (count == 0)
        {
            await SeedInitialDataAsync(db);
        }
    }

    private async Task SeedInitialDataAsync(SQLiteAsyncConnection db)
    {
        // Seed some common IR codes for popular TV brands
        var initialCodes = new List<IRCode>
        {
            // Samsung TV codes (NEC protocol, 38kHz)
            new IRCode
            {
                Brand = "Samsung",
                Function = "Power",
                Frequency = 38000,
                Pattern = "4500,4500,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,560,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,560,560,560,560,1690,560,1690,560,1690,560,46890",
                Protocol = "NEC",
                HexCode = "0xE0E040BF",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "Samsung",
                Function = "VolumeUp",
                Frequency = 38000,
                Pattern = "4500,4500,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,46890",
                Protocol = "NEC",
                HexCode = "0xE0E0E01F",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "Samsung",
                Function = "VolumeDown",
                Frequency = 38000,
                Pattern = "4500,4500,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,560,560,1690,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,560,560,1690,560,1690,560,1690,560,1690,560,46890",
                Protocol = "NEC",
                HexCode = "0xE0E0D02F",
                IsCustom = false
            },

            // LG TV codes (NEC protocol, 38kHz)
            new IRCode
            {
                Brand = "LG",
                Function = "Power",
                Frequency = 38000,
                Pattern = "9000,4500,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,1690,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,560,560,1690,560,1690,560,1690,560,1690,560,40000",
                Protocol = "NEC",
                HexCode = "0x20DF10EF",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "LG",
                Function = "VolumeUp",
                Frequency = 38000,
                Pattern = "9000,4500,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,560,560,1690,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,40000",
                Protocol = "NEC",
                HexCode = "0x20DF40BF",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "LG",
                Function = "VolumeDown",
                Frequency = 38000,
                Pattern = "9000,4500,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,560,1690,560,1690,560,1690,560,1690,560,1690,560,1690,560,40000",
                Protocol = "NEC",
                HexCode = "0x20DFC03F",
                IsCustom = false
            },

            // Sony TV codes (Sony protocol, 40kHz)
            new IRCode
            {
                Brand = "Sony",
                Function = "Power",
                Frequency = 40000,
                Pattern = "2400,600,1200,600,1200,600,600,600,600,600,1200,600,600,600,600,600,600,600,600,600,600,600,1200,600,19800",
                Protocol = "Sony",
                HexCode = "0xA90",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "Sony",
                Function = "VolumeUp",
                Frequency = 40000,
                Pattern = "2400,600,600,600,1200,600,600,600,600,600,1200,600,600,600,600,600,600,600,600,600,600,600,1200,600,19800",
                Protocol = "Sony",
                HexCode = "0x490",
                IsCustom = false
            },
            new IRCode
            {
                Brand = "Sony",
                Function = "VolumeDown",
                Frequency = 40000,
                Pattern = "2400,600,1200,600,1200,600,600,600,600,600,1200,600,600,600,600,600,600,600,600,600,600,600,1200,600,19800",
                Protocol = "Sony",
                HexCode = "0xC90",
                IsCustom = false
            }
        };

        await db.InsertAllAsync(initialCodes);
    }

    public async Task<List<string>> GetBrandsAsync()
    {
        var db = await GetDatabaseAsync();
        var codes = await db.Table<IRCode>().ToListAsync();
        return codes.Select(c => c.Brand).Distinct().OrderBy(b => b).ToList();
    }

    public async Task<IRCode?> GetCodeAsync(string brand, string function, string? model = null)
    {
        var db = await GetDatabaseAsync();

        if (!string.IsNullOrEmpty(model))
        {
            var code = await db.Table<IRCode>()
                .Where(c => c.Brand == brand && c.Function == function && c.Model == model)
                .FirstOrDefaultAsync();

            if (code != null)
                return code;
        }

        // Fallback to brand-level code
        return await db.Table<IRCode>()
            .Where(c => c.Brand == brand && c.Function == function && c.Model == null)
            .FirstOrDefaultAsync();
    }

    public async Task<List<IRCode>> GetCodesForBrandAsync(string brand, string? model = null)
    {
        var db = await GetDatabaseAsync();

        if (!string.IsNullOrEmpty(model))
        {
            var codes = await db.Table<IRCode>()
                .Where(c => c.Brand == brand && (c.Model == model || c.Model == null))
                .ToListAsync();

            return codes;
        }

        return await db.Table<IRCode>()
            .Where(c => c.Brand == brand && c.Model == null)
            .ToListAsync();
    }

    public async Task<int> SaveCodeAsync(IRCode code)
    {
        var db = await GetDatabaseAsync();

        if (code.Id != 0)
            return await db.UpdateAsync(code);
        else
            return await db.InsertAsync(code);
    }

    public async Task<int> DeleteCodeAsync(int id)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync<IRCode>(id);
    }

    public async Task<List<IRCode>> GetCustomCodesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<IRCode>()
            .Where(c => c.IsCustom)
            .ToListAsync();
    }

    public async Task<int> SaveProfileAsync(TVProfile profile)
    {
        var db = await GetDatabaseAsync();

        if (profile.Id != 0)
            return await db.UpdateAsync(profile);
        else
            return await db.InsertAsync(profile);
    }

    public async Task<List<TVProfile>> GetProfilesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<TVProfile>().ToListAsync();
    }

    public async Task<TVProfile?> GetActiveProfileAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<TVProfile>()
            .Where(p => p.IsActive)
            .FirstOrDefaultAsync();
    }

    public async Task SetActiveProfileAsync(int profileId)
    {
        var db = await GetDatabaseAsync();

        // Deactivate all profiles
        var profiles = await db.Table<TVProfile>().ToListAsync();
        foreach (var profile in profiles)
        {
            profile.IsActive = false;
            await db.UpdateAsync(profile);
        }

        // Activate selected profile
        var activeProfile = await db.Table<TVProfile>()
            .Where(p => p.Id == profileId)
            .FirstOrDefaultAsync();

        if (activeProfile != null)
        {
            activeProfile.IsActive = true;
            activeProfile.LastUsed = DateTime.Now;
            await db.UpdateAsync(activeProfile);
        }
    }
}
