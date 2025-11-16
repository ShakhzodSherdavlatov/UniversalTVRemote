using System.Windows.Input;
using UniversalTVRemote.Models;
using UniversalTVRemote.Services;

namespace UniversalTVRemote.ViewModels;

/// <summary>
/// View model for manual IR code entry page
/// </summary>
public class ManualCodeEntryViewModel : BaseViewModel
{
    private readonly IInfraredService _infraredService;
    private readonly IIRCodeDatabase _database;

    private string _brand = string.Empty;
    private string _model = string.Empty;
    private string _function = string.Empty;
    private string _frequency = "38000";
    private string _protocol = "NEC";
    private string _hexCode = string.Empty;
    private string _pattern = string.Empty;
    private string _notes = string.Empty;
    private string _statusMessage = string.Empty;

    public ManualCodeEntryViewModel(IInfraredService infraredService, IIRCodeDatabase database)
    {
        _infraredService = infraredService;
        _database = database;

        Title = "Manual Code Entry";

        SaveCommand = new Command(async () => await SaveCodeAsync(), () => CanSave());
        TestCommand = new Command(async () => await TestCodeAsync(), () => CanTest());
        CancelCommand = new Command(async () => await CancelAsync());

        // Re-evaluate commands when properties change
        PropertyChanged += (s, e) =>
        {
            ((Command)SaveCommand).ChangeCanExecute();
            ((Command)TestCommand).ChangeCanExecute();
        };
    }

    public string Brand
    {
        get => _brand;
        set => SetProperty(ref _brand, value);
    }

    public string Model
    {
        get => _model;
        set => SetProperty(ref _model, value);
    }

    public string Function
    {
        get => _function;
        set => SetProperty(ref _function, value);
    }

    public string Frequency
    {
        get => _frequency;
        set => SetProperty(ref _frequency, value);
    }

    public string Protocol
    {
        get => _protocol;
        set => SetProperty(ref _protocol, value);
    }

    public string HexCode
    {
        get => _hexCode;
        set => SetProperty(ref _hexCode, value);
    }

    public string Pattern
    {
        get => _pattern;
        set => SetProperty(ref _pattern, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public List<string> AvailableProtocols { get; } = new() { "NEC", "Sony", "RC5", "RC6", "Raw" };

    public ICommand SaveCommand { get; }
    public ICommand TestCommand { get; }
    public ICommand CancelCommand { get; }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Brand) &&
               !string.IsNullOrWhiteSpace(Function) &&
               !string.IsNullOrWhiteSpace(Frequency) &&
               (!string.IsNullOrWhiteSpace(HexCode) || !string.IsNullOrWhiteSpace(Pattern));
    }

    private bool CanTest()
    {
        return !string.IsNullOrWhiteSpace(Frequency) &&
               (!string.IsNullOrWhiteSpace(HexCode) || !string.IsNullOrWhiteSpace(Pattern));
    }

    private async Task SaveCodeAsync()
    {
        try
        {
            IsBusy = true;
            StatusMessage = "Saving code...";

            var code = new IRCode
            {
                Brand = Brand.Trim(),
                Model = string.IsNullOrWhiteSpace(Model) ? null : Model.Trim(),
                Function = Function.Trim(),
                Frequency = int.Parse(Frequency),
                Protocol = Protocol,
                HexCode = string.IsNullOrWhiteSpace(HexCode) ? null : HexCode.Trim(),
                Pattern = Pattern.Trim(),
                Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                IsCustom = true
            };

            await _database.SaveCodeAsync(code);

            StatusMessage = "✓ Code saved successfully!";

            // Navigate back after a short delay
            await Task.Delay(1500);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving code: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task TestCodeAsync()
    {
        try
        {
            IsBusy = true;
            StatusMessage = "Testing code...";

            if (!_infraredService.HasIREmitter())
            {
                StatusMessage = "⚠️ No IR emitter available";
                return;
            }

            var frequency = int.Parse(Frequency);
            bool success;

            if (!string.IsNullOrWhiteSpace(HexCode))
            {
                // Test using hex code
                success = await _infraredService.TransmitHexAsync(frequency, HexCode.Trim(), Protocol);
            }
            else if (!string.IsNullOrWhiteSpace(Pattern))
            {
                // Test using pattern
                var pattern = Pattern.Split(',').Select(s => int.Parse(s.Trim())).ToArray();
                success = await _infraredService.TransmitAsync(frequency, pattern);
            }
            else
            {
                StatusMessage = "⚠️ Enter either Hex Code or Pattern";
                return;
            }

            if (success)
                StatusMessage = "✓ Code transmitted! Check if it worked.";
            else
                StatusMessage = "✗ Failed to transmit code";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error testing code: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
