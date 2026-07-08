using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.ViewModels
{
    public partial class InfoPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string title = "Tietoa sovelluksesta.";
    }
}
