using System.Windows.Input;
using UniversalTVRemote.Models;
using UniversalTVRemote.Services;
using UniversalTVRemote.Views;

namespace UniversalTVRemote.ViewModels;

/// <summary>
/// View model for the main remote control page
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly IInfraredService _infraredService;
    private readonly IIRCodeDatabase _database;

    private string _statusMessage = string.Empty;
    private string _selectedBrand = string.Empty;
    private bool _hasIRSupport;
    private List<string> _availableBrands = new();

    public MainViewModel(IInfraredService infraredService, IIRCodeDatabase database)
    {
        _infraredService = infraredService;
        _database = database;

        Title = "TV Remote";

        // Commands
        PowerCommand = new Command(async () => await SendCommandAsync("Power"));
        VolumeUpCommand = new Command(async () => await SendCommandAsync("VolumeUp"));
        VolumeDownCommand = new Command(async () => await SendCommandAsync("VolumeDown"));
        ChannelUpCommand = new Command(async () => await SendCommandAsync("ChannelUp"));
        ChannelDownCommand = new Command(async () => await SendCommandAsync("ChannelDown"));
        MuteCommand = new Command(async () => await SendCommandAsync("Mute"));
        MenuCommand = new Command(async () => await SendCommandAsync("Menu"));
        OkCommand = new Command(async () => await SendCommandAsync("OK"));
        BackCommand = new Command(async () => await SendCommandAsync("Back"));
        HomeCommand = new Command(async () => await SendCommandAsync("Home"));

        NavigateToManualEntryCommand = new Command(async () => await NavigateToManualEntry());

        InitializeAsync();
    }

    public bool HasIRSupport
    {
        get => _hasIRSupport;
        set => SetProperty(ref _hasIRSupport, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string SelectedBrand
    {
        get => _selectedBrand;
        set => SetProperty(ref _selectedBrand, value);
    }

    public List<string> AvailableBrands
    {
        get => _availableBrands;
        set => SetProperty(ref _availableBrands, value);
    }

    // Commands
    public ICommand PowerCommand { get; }
    public ICommand VolumeUpCommand { get; }
    public ICommand VolumeDownCommand { get; }
    public ICommand ChannelUpCommand { get; }
    public ICommand ChannelDownCommand { get; }
    public ICommand MuteCommand { get; }
    public ICommand MenuCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand HomeCommand { get; }
    public ICommand NavigateToManualEntryCommand { get; }

    private async void InitializeAsync()
    {
        IsBusy = true;

        try
        {
            // Check IR support
            HasIRSupport = _infraredService.HasIREmitter();

            if (!HasIRSupport)
            {
                StatusMessage = "⚠️ This device does not have IR support";
                return;
            }

            // Initialize database
            await _database.InitializeAsync();

            // Load brands
            AvailableBrands = await _database.GetBrandsAsync();

            // Try to load active profile
            var activeProfile = await _database.GetActiveProfileAsync();
            if (activeProfile != null)
            {
                SelectedBrand = activeProfile.Brand;
                StatusMessage = $"Ready - {activeProfile.Name}";
            }
            else if (AvailableBrands.Any())
            {
                SelectedBrand = AvailableBrands.First();
                StatusMessage = "Ready - Select your TV brand";
            }
            else
            {
                StatusMessage = "No IR codes available";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SendCommandAsync(string function)
    {
        if (!HasIRSupport)
        {
            StatusMessage = "⚠️ No IR support";
            return;
        }

        if (string.IsNullOrEmpty(SelectedBrand))
        {
            StatusMessage = "⚠️ Please select a TV brand";
            return;
        }

        try
        {
            IsBusy = true;
            StatusMessage = $"Sending {function}...";

            // Get the IR code from database
            var code = await _database.GetCodeAsync(SelectedBrand, function);

            if (code == null)
            {
                StatusMessage = $"⚠️ {function} code not found for {SelectedBrand}";
                return;
            }

            // Parse pattern string to int array
            var pattern = code.Pattern.Split(',').Select(int.Parse).ToArray();

            // Transmit
            var success = await _infraredService.TransmitAsync(code.Frequency, pattern);

            if (success)
                StatusMessage = $"✓ Sent {function}";
            else
                StatusMessage = $"✗ Failed to send {function}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task NavigateToManualEntry()
    {
        await Shell.Current.GoToAsync(nameof(ManualCodeEntryPage));
    }
}
