using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WeatherApp.Models;
using Newtonsoft.Json.Linq;
using System.Timers;
using Timer = System.Timers.Timer;
namespace WeatherApp.ViewModels;

// Основной ViewModel для приложения. Отвечает за связь между UI и логикой приложения.
public class MainViewModel : INotifyPropertyChanged
{
    private string _city;
    public string City
    {
        get => _city; // Геттер возвращает текущее значение города.
        set
        {
            string trimmedValue = value?.Trim(); // Убираем лишние пробелы, если они есть.
            if (_city != trimmedValue) // Проверяем, изменилось ли значение.
            {
                _city = trimmedValue; // Присваиваем новое значение.
                OnPropertyChanged(); // Уведомляем об изменении свойства.
                UpdateTypingIndicator(); // Обновляем индикатор печати.
            }
        }
    }

    private string _weatherInfo;
    public string WeatherInfo
    {
        get => _weatherInfo;
        set
        {
            _weatherInfo = value; // Устанавливаем новое значение.
            OnPropertyChanged(); // Уведомляем об изменении свойства.
        }
    }

    private string _typingIndicator; // Поле для индикатора ввода текста.
    public string TypingIndicator
    {
        get => _typingIndicator;
        set
        {
            _typingIndicator = value;
            OnPropertyChanged();
        }
    }

    private Timer _typingTimer; // таймер для анимации "печатает..."
    private Timer _stopTypingTimer; // таймер для остановки индикатора
    private int _dotCount; // Счетчик точек для анимации.

    public ICommand GetWeatherCommand { get; } // Команда для получения данных о погоде.
    public ICommand NavigateToAboutCommand { get; } // Команда для перехода на страницу "О приложении".

    // Конструктор класса. Инициализируем команды и таймеры.
    public MainViewModel()
    {
        GetWeatherCommand = new Command(async () => await GetWeather()); // Команда для вызова метода GetWeather.
        NavigateToAboutCommand = new Command(async () => await NavigateToAbout()); // Команда для навигации.

        _typingTimer = new Timer(500); // Таймер обновляется каждые 500 мс.
        _typingTimer.Elapsed += OnTypingTimerElapsed; // Привязываем обработчик события.
        _typingTimer.AutoReset = true; // Таймер перезапускается автоматически.

        _stopTypingTimer = new Timer(1000); // Таймер остановки индикатора печати.
        _stopTypingTimer.Elapsed += OnStopTypingTimerElapsed; // Привязываем обработчик события.
        _stopTypingTimer.AutoReset = false; // Таймер срабатывает только один раз.
    }


    private async Task GetWeather()
    {
        string apiKey = "Ваш ключ";
        string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={City}&appid={apiKey}&units=metric&lang=ru";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl); // Выполняем GET-запрос.
                response.EnsureSuccessStatusCode(); // Проверяем успешность ответа.
                string responseBody = await response.Content.ReadAsStringAsync(); // Получаем тело ответа.
                JObject jsonResponse = JObject.Parse(responseBody); // Парсим JSON-ответ.


                WeatherModel weather = new WeatherModel
                {
                    City = jsonResponse["name"].ToString(),
                    Temperature = (int)Math.Round(jsonResponse["main"]["temp"].Value<double>()), // Округляем температуру.
                    Description = jsonResponse["weather"][0]["description"].ToString(), // Описание погоды.
                    Pressure = (double)jsonResponse["main"]["pressure"], // Давление впаскалях.
                    Humidity = (int)jsonResponse["main"]["humidity"], // Влажность в %.
                    WindSpeed = (double)jsonResponse["wind"]["speed"], // Скорость ветра.
                    WindDirection = (int)jsonResponse["wind"]["deg"] // Направление ветра.
                };

                // Преобразуем давление в мм рт. ст.
                double pressureInMmHg = weather.Pressure * 0.750062;

                // Получаем текстовое описание направления ветра.
                string windDirectionDescription = GetWindDirection(weather.WindDirection);


                WeatherInfo = $"Город: {weather.City}\nТемпература: {weather.Temperature}°C\nОписание: {weather.Description}\nДавление: {Math.Round(pressureInMmHg, 2)} мм.рт.ст.\n" +
                    $"Влажность: {weather.Humidity}%\nСкорость ветра: {Math.Round(weather.WindSpeed, 1)} м/с\nНаправление ветра: {windDirectionDescription}";
            }
            catch (HttpRequestException e)
            {
                WeatherInfo = $"Ошибка запроса: {e.Message}"; // Выводим сообщение об ошибке.
            }
        }
    }

    // Метод преобразует направление ветра в градусах в текстовое описание.
    private string GetWindDirection(int degrees)
    {
        string[] directions = { "Северный", "Северо-восточный", "Восточный", "Юго-восточный", "Южный", "Юго-западный", "Западный", "Северо-западный" };
        return directions[(int)(((degrees + 22.5) % 360) / 45)]; // Рассчитываем индекс для массива направлений.
    }

    // Метод для обновления индикатора печати.
    private void UpdateTypingIndicator()
    {
        if (string.IsNullOrWhiteSpace(City)) // Проверяем, пустое ли поле ввода.
        {
            _typingTimer.Stop();
            TypingIndicator = string.Empty; // Очищаем индикатор.
        }
        else
        {
            if (!_typingTimer.Enabled) // Если таймер еще не запущен.
            {
                _typingTimer.Start(); // Запускаем таймер.
            }
            _stopTypingTimer.Stop(); // Перезапускаем таймер остановки индикатора.
            _stopTypingTimer.Start();
        }
    }

    // Обработчик события для таймера печати.
    private void OnTypingTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _dotCount = (_dotCount + 1) % 4;
        TypingIndicator = "Печатает" + new string('.', _dotCount);
    }

    // Обработчик события для остановки индикатора печати.
    private void OnStopTypingTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _typingTimer.Stop();
        TypingIndicator = string.Empty;
    }

    // Метод для перехода на страницу "О приложении".
    private async Task NavigateToAbout()
    {
        await Shell.Current.GoToAsync("//AboutPage");
    }

    // Событие для уведомления об изменении свойств.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызываем событие изменения свойства.
    }
}