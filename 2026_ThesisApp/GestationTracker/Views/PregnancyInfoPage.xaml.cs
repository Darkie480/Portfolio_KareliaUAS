using GestationTracker.ViewModels;

namespace GestationTracker.Views;

public partial class PregnancyInfoPage : ContentPage
{
	public PregnancyInfoPage(PregnancyInfoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}