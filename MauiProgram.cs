using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp
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
                });

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddTransient<AboutViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddSingleton<HttpClient>();

            return builder.Build();
        }
    }
}