using UniversalTVRemote.ViewModels;

namespace UniversalTVRemote.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
