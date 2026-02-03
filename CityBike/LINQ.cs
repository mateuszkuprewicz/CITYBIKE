using CityBike.Types;

namespace CityBike;

public class Linq
{
    private static readonly List<BikeRide> _bikeRides = ParsingData.BikeRides().ToList();
    private static readonly List<WeatherDay> _weatherDays = ParsingData.WeatherDays().ToList();
    
    //From which stations more bikes are rented than returned
    public static List<string> GetLackingBikesStations()
    {
        var startBikeRides = _bikeRides
            .Select(x => new
            {
                StationId = x.StartStationId,
                StationName = x.StartStationName,
                x.RideId
            })
            .GroupBy(x => (x.StationId, x.StationName))
            .Select(x => new
            {
                x.Key.StationId,
                x.Key.StationName,
                Count = x.Count()
            });

        var endBikeRides = _bikeRides
            .Select(x => new
            {
                StationId = x.EndStationId,
                StationName = x.EndStationName,
                x.RideId
            })
            .GroupBy(x => (x.StationId, x.StationName))
            .Select(x => new
            {
                x.Key.StationId,
                x.Key.StationName,
                Count = x.Count()
            });

        return startBikeRides.GroupJoin(
                endBikeRides,
                startRide => startRide.StationId,
                endRide => endRide.StationId,
                (startRide, endRides) => new
                {
                    Id = startRide.StationId,
                    Name = startRide.StationName,
                    StartCount = startRide.Count,
                    EndCount = endRides.Sum(x => x.Count)
                })
            .Where(x => x.StartCount > x.EndCount)
            .Select(x => x.Name).ToList();
    }
    
    //At what average temperature the average number of hours driven per ride is the highest?
    public static int MostSpentTemp()
    {
        return _weatherDays.GroupJoin(
                _bikeRides,
            day => DateOnly.FromDateTime(day.Date),
            ride => DateOnly.FromDateTime(ride.StartDate),
            (day, rides) => new
            {
                Temperature = (int)Math.Round(day.AverageTemperature, MidpointRounding.AwayFromZero),
                Time = rides.Any() 
                    ? rides.Average(r => (r.EndDate - r.StartDate).TotalSeconds)
                    : 0
            })
            .OrderByDescending(x => x.Time)
            .Select(x => x.Temperature)
            .First();
    }
    
    //At what average wind speed the average number of hours driven per ride is the highest?
    public static int MostSpentWindSpeed()
    {
        return _weatherDays.GroupJoin(
                _bikeRides,
                day => DateOnly.FromDateTime(day.Date),
                ride => DateOnly.FromDateTime(ride.StartDate),
                (day, rides) => new
                {
                    WindSpeed = (int)Math.Round(day.WindSpeed, MidpointRounding.AwayFromZero),
                    Time = rides.Any() 
                        ? rides.Average(r => (r.EndDate - r.StartDate).TotalSeconds)
                        : 0
                })
            .OrderByDescending(x => x.Time)
            .Select(x => x.WindSpeed)
            .First();
    }
    
    //How much time on average on rides spend member users and casual users?
    public static List<(string key, TimeSpan avgTime)> GetAvgMeasurementsPerUserType()
    {
        return _bikeRides
            .GroupBy(x => x.MemberType)
            .Select(x =>
            (
                key: x.Key,
                avgTime: TimeSpan.FromSeconds(x.Average(y => (y.EndDate - y.StartDate).TotalSeconds))
            ))
            .ToList();
    }
    
    //The number of hours cycled by a member and a casual user each month, respectively, and their average temperature
    public static List<(int Month, float AvgTemperature, string Member, TimeSpan AvgTime)> GetMonthlyCyclingData()
    {
        var avgTempByMonth = _weatherDays
            .GroupBy(w => w.Date.Month)
            .ToDictionary(
                g => g.Key,
                g => g.Average(w => w.AverageTemperature)
            );

        var rideStats =
            _bikeRides
                .GroupBy(r => new {
                    Month = r.StartDate.Month,
                    r.MemberType
                })
                .Select(g => (
                    Month: g.Key.Month,
                    Member: g.Key.MemberType,
                    AvgTime: TimeSpan.FromSeconds(
                        g.Average(r => (r.EndDate - r.StartDate).TotalSeconds)
                    )
                ));

        return rideStats
            .Select(r => (
                r.Month,
                AvgTemperature: avgTempByMonth[r.Month],
                r.Member,
                r.AvgTime
            ))
            .OrderBy(x => x.Month)
            .ToList();
    }
}