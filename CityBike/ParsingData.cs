using System.Globalization;
using CityBike.Types;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace CityBike;

public class ParsingData
{
    public const string WeatherData = "/home/matt/laby_P3/CityBike/CityBike/CSV/weather.csv";
    public const string CityBikeData = "/home/matt/laby_P3/CityBike/CityBike/CSV/2023-citibike-tripdata";

    public static IEnumerable<WeatherDay> WeatherDays(string data = WeatherData)
    {
        using var reader = new StreamReader(data);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        while (csv.Read())
        {
            string? sDate = csv.GetField<string>(0);
            if (sDate != null)
            {
                DateTime date = DateTime.ParseExact(sDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                yield return new WeatherDay(date, csv.GetField<float>(1), csv.GetField<float>(7));
            }
        }
    }

    public static IEnumerable<BikeRide> BikeRides(string data = CityBikeData)
    {
        var dirs = Directory.GetDirectories(data);
        StreamReader reader;
        CsvReader csv;
        foreach (var dir in dirs)
        {
            var csvFiles = Directory.GetFiles(dir);
            foreach (var csvFile in csvFiles)
            {
                reader = new StreamReader(csvFile);
                csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                try
                {
                    csv.Read();
                    while (csv.Read())
                    {
                        string? sStartDate = csv.GetField<string>(2);
                        string? sEndDate = csv.GetField<string>(3);
                        if (sStartDate != null && sEndDate != null)
                        {
                            DateTime startDate = DateTime.ParseExact(
                                sStartDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            DateTime endDate = DateTime.ParseExact(
                                sEndDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            yield return new BikeRide(
                                csv.GetField<string>(0), startDate, endDate,
                                csv.GetField<string>(5), csv.GetField<string>(4), //station start
                                csv.GetField<string>(7), csv.GetField<string>(6), //station end
                                csv.GetField<string>(12));
                        }
                    }
                }
                finally
                {
                    csv.Dispose();
                    reader.Dispose();
                }
            }
        }

    }
}