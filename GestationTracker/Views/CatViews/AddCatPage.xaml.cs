using GestationTracker.ViewModels;

namespace GestationTracker.Views.CatViews;

public partial class AddCatPage : ContentPage
{
	public AddCatPage(AddCatViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void Label_BindingContextChanged(object sender, EventArgs e)
    {

    }
}