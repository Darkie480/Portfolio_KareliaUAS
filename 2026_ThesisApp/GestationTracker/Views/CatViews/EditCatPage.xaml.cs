using GestationTracker.ViewModels;

namespace GestationTracker.Views.CatViews;

public partial class EditCatPage : ContentPage
{
	public EditCatPage(AddCatViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


   