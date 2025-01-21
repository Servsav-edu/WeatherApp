namespace WeatherApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        if (Current.RequestedTheme == AppTheme.Dark)
        {
            Current.Resources["BackgroundColor"] = Current.Resources["BackgroundColorDark"];
            Current.Resources["TextColor"] = Current.Resources["TextColorDark"];
            Current.Resources["ToggleThemeButtonColor"] = Colors.White;
            Current.Resources["BorderColor"] = Colors.White;
        }
        else
        {
            Current.Resources["BackgroundColor"] = Current.Resources["BackgroundColorLight"];
            Current.Resources["TextColor"] = Current.Resources["TextColorLight"];
            Current.Resources["ToggleThemeButtonColor"] = Colors.Black;
            Current.Resources["BorderColor"] = Colors.Gray;
        }
    }

    public void ToggleTheme()
    {
        if (Current.UserAppTheme == AppTheme.Dark)
        {
            Current.UserAppTheme = AppTheme.Light;
        }
        else
        {
            Current.UserAppTheme = AppTheme.Dark;
        }
        ApplyTheme();
    }
}