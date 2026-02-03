namespace CityBike.Types;


public class WeatherDay
{
    public WeatherDay(DateTime date, float averageTemperature, float windSpeed)
    {
        Date = date;
        AverageTemperature = averageTemperature;
        WindSpeed = windSpeed;
    }
    public DateTime Date { get; set; }
    public float AverageTemperature { get; set; }
    public float WindSpeed { get; set; }

    public void Print()
    {
        Console.WriteLine($"Data: {Date} AvgTemp: {AverageTemperature} WindSpeed: {WindSpeed}");
    }
}