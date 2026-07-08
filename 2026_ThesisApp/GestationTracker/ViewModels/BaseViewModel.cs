using GestationTracker.Views;
using GestationTracker.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        protected async Task ExcecuteAsync(Func<Task> operation)
        {
            if (isBusy) return;
            try
            {
                isBusy = true;
                await operation();
            }
            finally
            {
                isBusy = false;
            }
        }
    }
}
