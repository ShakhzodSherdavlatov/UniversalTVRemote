using Microsoft.Extensions.Logging;
using UniversalTVRemote.Services;
using UniversalTVRemote.Views;
using UniversalTVRemote.ViewModels;

namespace UniversalTVRemote;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<IInfraredService, InfraredService>();
        builder.Services.AddSingleton<IIRCodeDatabase, IRCodeDatabase>();

        // Register pages and view models
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddTransient<ManualCodeEntryPage>();
        builder.Services.AddTransient<ManualCodeEntryViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
