using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
        UpdateToggleThemeButtonColor();
    }

    private void OnToggleThemeClicked(object sender, EventArgs e)
    {
        ((App)Application.Current).ToggleTheme();
        UpdateToggleThemeButtonColor();
    }

    private void UpdateToggleThemeButtonColor()
    {
        if (Application.Current.UserAppTheme == AppTheme.Dark)
        {
            ToggleThemeButton.BackgroundColor = Colors.White;
        }
        else
        {
            ToggleThemeButton.BackgroundColor = Colors.Black;
        }
    }
}