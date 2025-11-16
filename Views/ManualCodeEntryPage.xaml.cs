using UniversalTVRemote.ViewModels;

namespace UniversalTVRemote.Views;

public partial class ManualCodeEntryPage : ContentPage
{
    public ManualCodeEntryPage(ManualCodeEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
