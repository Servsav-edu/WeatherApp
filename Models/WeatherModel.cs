namespace WeatherApp.Models;

public class WeatherModel
{
    public string City { get; set; }
    public double Temperature { get; set; }
    public string Description { get; set; }
    public double Pressure { get; set; } // Давление в паскалях.
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int WindDirection { get; set; } //  Ветер в градусах
}