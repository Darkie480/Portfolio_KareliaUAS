using GestationTracker.Models;
using GestationTracker.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.ViewModels
{
    public partial class PregnancyInfoViewModel : BaseViewModel
    {
        private readonly PregnancyService _pregnancyService;
        public bool HasSelectedStage => SelectedStage != null;

        [ObservableProperty]
        private int selectedSection;

        [ObservableProperty]
        private PregnancyStage selectedStage = null;

        [ObservableProperty]
        private List<PregnancyStage> stages;

        [ObservableProperty]
        private string title = "Kissan raskauden vaiheet";

        public bool IsButton1Visible => SelectedSection == 1;
        public bool IsButton2Visible => SelectedSection == 2;
        public bool IsButton3Visible => SelectedSection == 3;
        public bool IsButton4Visible => SelectedSection == 4;

        partial void OnSelectedSectionChanged(int value)
        {
            OnPropertyChanged(nameof(IsButton1Visible));
            OnPropertyChanged(nameof(IsButton2Visible));
            OnPropertyChanged(nameof(IsButton3Visible));
            OnPropertyChanged(nameof(IsButton4Visible));
        }

        [RelayCommand]
        private void ShowSection(object section)
        {
            var value = Convert.ToInt32(section);

            System.Diagnostics.Debug.WriteLine($"BUTTON CLICKED: {value}");

            SelectedSection = value;
        }

        [RelayCommand]
        private void SelectStage(object id)
        {
            int stageId = Convert.ToInt32(id);

            SelectedStage = Stages?.FirstOrDefault(x => x.Id == stageId);
        }

        private async void LoadData()
        {
            Stages = await _pregnancyService.GetStagesAsync();

            System.Diagnostics.Debug.WriteLine($"Stage count: {Stages.Count}");

        }

        public PregnancyInfoViewModel (PregnancyService pregnancyService)
        {
            _pregnancyService = pregnancyService;

            LoadData();
        }

        partial void OnSelectedStageChanged(PregnancyStage value)
        {
            OnPropertyChanged(nameof(HasSelectedStage));
        }
    }
}
