using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeatherApp.ViewModels;

public class AboutViewModel : INotifyPropertyChanged
{
    // Форматированная строка с информацией о приложении.
    public FormattedString AboutInfo { get; } = new FormattedString
    {
        Spans =
        {
            new Span { Text = "Это приложение показывает погоду в выбранном городе.\n" },
            new Span { Text = "Для получения информации о погоде введите название города в поле ввода и нажмите кнопку 'Получить погоду'.\n" },
            new Span { Text = "Выполнил студент группы РИВ-320908у\n" },
            new Span { Text = "Свалухин Александр Владимирович\n" },
            new Span { Text = "Екатеринбург 2025\n" },
            new Span
            {
                Text = "svaluhinav@yandex.ru",
                TextColor = Colors.Blue,
                TextDecorations = TextDecorations.Underline, // Подчеркивание для обозначения интерактивного элемента
                GestureRecognizers = // Событие при нажатии 
                {
                    new TapGestureRecognizer
                    {
                        // Открытие почтового клиента с этим адресом.
                        Command = new Command(() => Launcher.OpenAsync("mailto:svaluhinav@yandex.ru"))
                    }
                }
            },
            new Span { Text = "\n" },
            new Span
            {
                Text = "ссылка на GitHub",
                TextColor = Colors.Blue,
                TextDecorations = TextDecorations.Underline,
                GestureRecognizers = // Событие при нажатии
                {
                    new TapGestureRecognizer
                    {   
                        // Открытие браузера с ссылкой на репозиторий.
                        Command = new Command(() => Launcher.OpenAsync("https://github.com/servsav-edu/WeatherApp"))
                    }
                }
            }
        }
    };
    public ICommand BackCommand { get; }

    public AboutViewModel()
    {
        BackCommand = new Command(async () => await GoBack());
    }

    // Метод для перехода на главную страницу приложения.
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    // Событие для уведомления об изменении свойств.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызываем событие, чтобы обновить UI при изменении данных. 
    }
}