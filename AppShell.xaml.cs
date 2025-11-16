using UniversalTVRemote.Views;

namespace UniversalTVRemote;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(ManualCodeEntryPage), typeof(ManualCodeEntryPage));
    }
}
