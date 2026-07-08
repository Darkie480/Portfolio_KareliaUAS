using GestationTracker.ViewModels;

namespace GestationTracker.Views;

public partial class InfoPage : ContentPage
{

    public InfoPage(InfoPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		
	}

    
}