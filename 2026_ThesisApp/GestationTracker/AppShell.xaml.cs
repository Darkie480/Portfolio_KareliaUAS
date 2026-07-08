using GestationTracker.Views;
using GestationTracker.Views.CatViews;

namespace GestationTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CatDetailPage), typeof(CatDetailPage));
            Routing.RegisterRoute(nameof(AddCatPage), typeof(AddCatPage));
            Routing.RegisterRoute(nameof(EditCatPage), typeof(EditCatPage));
            Routing.RegisterRoute(nameof(PregnancyInfoPage), typeof(PregnancyInfoPage));
            Routing.RegisterRoute(nameof(InfoPage), typeof(InfoPage));

        }
    }
}
