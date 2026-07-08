using GestationTracker.Models;
using GestationTracker.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GestationTracker.ViewModels
{
    public partial class CatListViewModel : BaseViewModel
    {
        private readonly ICatService _catService;

        [ObservableProperty]
        private ObservableCollection<Cats> cats = new();

        [ObservableProperty]
        private Cats? selectedCat;

        public CatListViewModel(ICatService catService)
        {
            _catService = catService;
            Title = "Kissalistaus";
        }

        [RelayCommand]
        private async Task LoadCats()
        {
            await ExcecuteAsync(async () =>
            {
                Cats.Clear();
                var cats = await _catService.GetCatsAsync();
                foreach (var cat in cats)
                    Cats.Add(cat);
            });
        }

        [RelayCommand]
        private async Task GoToDetail(Cats cats)
        {
            if (cats == null) return;
            await Shell.Current.GoToAsync($"CatDetailPage", new Dictionary<string, object> { { "CatId", cats.Id } });
            SelectedCat = null;
        }

        [RelayCommand]
        private async Task GoToAddCat()
        {
            await Shell.Current.GoToAsync("AddCatPage");
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadCats();
        }
    }
}
