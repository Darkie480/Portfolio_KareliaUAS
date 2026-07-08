using GestationTracker.ViewModels;

namespace GestationTracker.Views.CatViews;

public partial class CatListPage : ContentPage
{

	private readonly CatListViewModel _viewModel;

	public CatListPage(CatListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadCatsCommand.ExecuteAsync(null);
	}
}