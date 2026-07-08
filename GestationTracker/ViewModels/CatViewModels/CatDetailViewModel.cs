using GestationTracker.Models;
using GestationTracker.Services;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace GestationTracker.ViewModels
{
    [QueryProperty(nameof(CatId), nameof(CatId))]
    public partial class CatDetailViewModel : BaseViewModel
    {
        private readonly ICatService _catService;
        public DateTime MinimumMatingDate => DateTime.Today.AddDays(-75);

        [ObservableProperty]
        private string catId = string.Empty;

        [ObservableProperty]
        private Cats? cat;

        [ObservableProperty]
        private string catRegNr = string.Empty;

        [ObservableProperty]
        private string catRegName = string.Empty;

        [ObservableProperty]
        private string catBreed = string.Empty;

        [ObservableProperty]
        private DateTime matingDate = DateTime.Today;

        [ObservableProperty]
        private string catInsuranceNumber = string.Empty;

        [ObservableProperty]
        private string catInsuranceCompany = string.Empty;

        public bool IsRegistered => !string.IsNullOrWhiteSpace(Cat?.RegistrationNumber);
        public bool IsInsured => !string.IsNullOrWhiteSpace(Cat?.InsuranceNumber);

        public bool IsPregnant
        {
            get => Cat?.CurrentPregnancy != null;
            set
            {
                if (Cat == null) return;

                if (value && Cat.CurrentPregnancy == null)
                {
                    Cat.CurrentPregnancy = new Pregnancy
                    {
                        MatingDate = MatingDate,
                        GestationLength = 65
                    };
                }
                else if (!value)
                {
                    Cat.CurrentPregnancy = null;
                }

                OnPropertyChanged(nameof(IsPregnant));
                OnPropertyChanged(nameof(Cat));
            }
        }

        public CatDetailViewModel(ICatService catService)
        {
            _catService = catService;

        }

        partial void OnCatChanged(Cats? selectedCat)
        {
            if (selectedCat == null)
                return;

            Title = $"Kissan '{Cat.Name}' tiedot";

            Cat.PropertyChanged += Cat_PropertyChanged;
            OnPropertyChanged(nameof(IsRegistered));
            OnPropertyChanged(nameof(IsInsured));

        }

        partial void OnCatIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
                LoadCatCommand.Execute(null);
        }

        private void Cat_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Cats.InsuranceNumber))
            {
                OnPropertyChanged(nameof(IsInsured));
            }
            if (e.PropertyName != nameof(Cats.RegistrationNumber))
            {
                OnPropertyChanged(nameof(IsRegistered));
            }
        }

        [RelayCommand]
        private async Task LoadCat()
        {
            if (string.IsNullOrEmpty(CatId)) return;

            await ExcecuteAsync(async () =>
            {
                Cat = await _catService.GetCatsByIdAsync(CatId);

                if (Cat != null)
                {
                    if (!string.IsNullOrEmpty(Cat.RegistrationNumber))
                        catRegNr = Cat.RegistrationNumber;

                    if (!string.IsNullOrEmpty(Cat.RegistrationName))
                        catRegName = Cat.RegistrationName;

                    if (!string.IsNullOrEmpty(Cat.Breed))
                        catBreed = Cat.Breed;

                    if (Cat?.CurrentPregnancy != null)
                    {
                        MatingDate = Cat.CurrentPregnancy.MatingDate;
                    }

                    OnPropertyChanged(nameof(IsPregnant));
                    OnPropertyChanged(nameof(IsRegistered));
                    OnPropertyChanged(nameof(IsInsured));
                }
            });
        }

        [RelayCommand]
        private async Task UpdatePregnancy()
        {
            if (Cat == null)
                return;

            if (IsPregnant && Cat.CurrentPregnancy == null)
            {
                Cat.CurrentPregnancy = new Pregnancy
                {
                    MatingDate = MatingDate,
                    GestationLength = 65
                };
            }
            else if (!IsPregnant)
            {
                Cat.CurrentPregnancy = null;
            }
            else
            {
                Cat.CurrentPregnancy.MatingDate = MatingDate;
            }

            OnPropertyChanged(nameof(Cat));
            OnPropertyChanged(nameof(IsPregnant));
            
            await _catService.UpdateCatAsync(Cat);

        }

        [RelayCommand]
        private async Task EditCat()
        {
            await Shell.Current.GoToAsync($"EditCatPage?catId={Cat.Id}");
        }


        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task DeleteCat()
        {
            if (Cat == null) return;

            bool confirmed = await Shell.Current.DisplayAlert(
                "Poista kissa",
                $"Poista {Cat.Name}?",
                "Poista",
                "Peruuta");

            //If boolean comes back false, return without deleting the cat.
            if (!confirmed) return;

            await ExcecuteAsync(async () =>
            {
                await _catService.DeleteCatAsync(Cat.Id);
                await GoBack();
            });
        }
    }
}
