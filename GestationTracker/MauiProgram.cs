using GestationTracker.Views.CatViews;
using GestationTracker.ViewModels;
using GestationTracker.Services;

using Microsoft.Extensions.Logging;
using GestationTracker.Views;
using GestationTracker.Models;

namespace GestationTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("CustomPicker", (handler, view) =>
            {
#if ANDROID
    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });


#if DEBUG
            builder.Logging.AddDebug();
#endif



            //Register services
            builder.Services.AddSingleton<ICatService, CatService>();
            builder.Services.AddSingleton<PregnancyService>();
            builder.Services.AddSingleton<PregnancyStageRoot>();

            builder.Services.AddTransient<CatListPage>();
            builder.Services.AddTransient<CatListViewModel>();

            builder.Services.AddTransient<CatDetailPage>();
            builder.Services.AddTransient<CatDetailViewModel>();

            builder.Services.AddTransient<EditCatPage>();
            builder.Services.AddTransient<AddCatPage>();
            builder.Services.AddTransient<AddCatViewModel>();

            builder.Services.AddTransient<PregnancyInfoPage>();
            builder.Services.AddTransient<PregnancyInfoViewModel>();

            builder.Services.AddTransient<InfoPage>();
            builder.Services.AddTransient<InfoPageViewModel>();


            // BUILDER TRANSIENT tänne
            // ROUTING APPSHELL.XAML.CS !!!
            // Älä unohda!!



            return builder.Build();


        }
    }
}
