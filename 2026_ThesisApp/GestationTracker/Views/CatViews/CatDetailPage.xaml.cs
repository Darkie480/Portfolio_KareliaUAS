using GestationTracker.ViewModels;

namespace GestationTracker.Views.CatViews;

public partial class CatDetailPage : ContentPage
{
	public CatDetailPage(CatDetailViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
       

    }

}