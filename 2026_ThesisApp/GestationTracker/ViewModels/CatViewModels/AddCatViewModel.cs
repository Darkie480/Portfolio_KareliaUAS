using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestationTracker.Models;
using GestationTracker.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GestationTracker.ViewModels
{
    [QueryProperty(nameof(CatId), "catId")]
    public partial class AddCatViewModel : BaseViewModel
    {
        private readonly ICatService _catService;
        private Cats _catToUpdate;

        public ObservableCollection<string> CatBreeds { get; set; } = new();
        public DateTime MinimumMatingDate => DateTime.Today.AddDays(-75);

        [ObservableProperty]
        public string catId;

        [ObservableProperty]
        private string? catName = string.Empty;

        [ObservableProperty]
        private string? catRegName = string.Empty;

        [ObservableProperty]
        private string? catRegNr = string.Empty;

        [ObservableProperty]
        private string? catBreed = string.Empty;

        [ObservableProperty]
        private DateTime catDateofBirth = DateTime.Today;

        [ObservableProperty]
        private string? photoPath = string.Empty;

        [ObservableProperty]
        private string catInsuranceNumber = string.Empty;

        [ObservableProperty]
        private string catInsuranceCompany = string.Empty;

        [ObservableProperty]
        private DateTime matingDate = DateTime.Today;

        [ObservableProperty]
        private bool isPregnant;


        public AddCatViewModel(ICatService catService)
        {
            _catService = catService;
            Title = "Lisää uusi kissa";

            Task.Run(LoadCatBreeds);
        }

        // Active when editing a cat
        partial void OnCatIdChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _ = LoadCat(value);
                Title = $"Muokkaa kissan tietoja."; 
            }
        }

        [RelayCommand]
        private async Task TakePhoto()
        {
            try
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await Shell.Current.DisplayAlert("Toiminto ei ole tuettu", "Kamera ei ole käytettävissä.", "OK");
                    return;
                }

                // If the machine in use supports taking pictures, uses already existing task and storing it to a variable
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    var localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using var sourceStream = await photo.OpenReadAsync();
                    using var localStream = File.OpenWrite(localPath);
                    await sourceStream.CopyToAsync(localStream);
                    PhotoPath = localPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task PickPhoto()
        {
            try
            {
                // Allows the user to search the gallery of used machine, uses already existing task and storing it to a variable
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    var localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using var sourceStream = await photo.OpenReadAsync();
                    using var localStream = File.OpenWrite(localPath);
                    await sourceStream.CopyToAsync(localStream);
                    PhotoPath = localPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        [RelayCommand]
        private async Task SaveCat()
        {
            if (string.IsNullOrEmpty(catName))
            {
                await Shell.Current.DisplayAlert("Huomio!", "Kissan nimi tarvitaan.", "OK");
                return;
            }

            await ExcecuteAsync(async () =>
            {
                var newCat = new Cats
                {
                    Id = catId,
                    Name = catName,
                    RegistrationName = catRegName,
                    RegistrationNumber = catRegNr,
                    Breed = catBreed,
                    DateOfBirth = catDateofBirth,
                    InsuranceCompany = catInsuranceCompany,
                    InsuranceNumber = catInsuranceNumber,
                    ImageUrl = photoPath ?? string.Empty

                };

                if (IsPregnant)
                {
                    newCat.CurrentPregnancy = new Pregnancy
                    {
                        MatingDate = MatingDate,
                        GestationLength = 65
                    };
                }

                // While editing a cat
                if (!string.IsNullOrWhiteSpace(CatId))
                {
                    await _catService.UpdateCatAsync(newCat);
                    await Shell.Current.DisplayAlert("Tiedot päivitetty", $"Kissan '{CatName}' tiedot päivitetty onnistuneesti.", "OK");
                }
                // While creating a new cat-entry
                else
                {
                    await _catService.AddCatAsync(newCat);
                    await Shell.Current.DisplayAlert("Tallennus onnistunut", $"{CatName} on onnistunesti lisätty listalle.", "OK");
                    
                }
                await Shell.Current.GoToAsync("//CatListPage");

            });
        }

        /// <summary>
        /// Loads the picker-element with JSON-list of cat breeds 
        /// when the view is first opened.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCatBreeds()
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("catbreeds.json");

            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var breeds = JsonSerializer.Deserialize<List<string>>(json);

            CatBreeds.Clear();

            foreach (var breed in breeds)
            {
                CatBreeds.Add(breed);
            }
        }

        public async Task LoadCat(string catId)
        {
            _catToUpdate = await _catService.GetCatsByIdAsync(catId);

            CatName = _catToUpdate.Name;
            CatRegName = _catToUpdate.RegistrationName;
            CatRegNr = _catToUpdate.RegistrationNumber;
            CatBreed = _catToUpdate.Breed;
            CatDateofBirth = _catToUpdate.DateOfBirth;
            CatInsuranceCompany = _catToUpdate.InsuranceCompany;
            CatInsuranceNumber = _catToUpdate.InsuranceNumber;
            PhotoPath = _catToUpdate.ImageUrl ?? string.Empty;

        }

        [RelayCommand]
        private async Task Cancel() => await Shell.Current.GoToAsync("//CatListPage");

    }
}
