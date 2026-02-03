// See https://aka.ms/new-console-template for more information

using CityBike;

var tasks = new List<Task<object>>
{
    Task.Run(() => (object)Linq.GetLackingBikesStations()),
    Task.Run(() => (object)Linq.MostSpentTemp()),
    Task.Run(() => (object)Linq.MostSpentWindSpeed()),
    Task.Run(() => (object)Linq.GetAvgMeasurementsPerUserType()),
    Task.Run(() => (object)Linq.GetMonthlyCyclingData())
};
var results = await Task.WhenAll(tasks);

// using FileStream fs = File.Create("/home/matt/laby_P3/CityBike/CityBike/results.txt");
// using StreamWriter sw = new StreamWriter(fs);

Console.WriteLine("From which stations more bikes are rented than returned:");
foreach (string station in (List<string>)results[0])
{
    Console.WriteLine(station);
} Console.WriteLine("");

Console.WriteLine("At what average temperature the average number of hours driven per ride is the highest?");
Console.WriteLine((int)results[1]);
Console.WriteLine("At what average wind speed the average number of hours driven per ride is the highest?");
Console.WriteLine((int)results[2]); Console.WriteLine("");

Console.WriteLine("How much time on average on rides spend member users and casual users?");
foreach (var tuple in (List<(string key,TimeSpan avgTime)>)results[3])
{
    Console.WriteLine($"{tuple.key}: {tuple.avgTime.Hours} hours, {tuple.avgTime.Minutes} minutes");
} Console.WriteLine("");

Console.WriteLine("The number of hours cycled by a member and a casual user each month, respectively, " +
             "and their average temperature");
foreach (var tuple in (List<(int Month, float AvgTemperature, string Member, TimeSpan AvgTime)>)results[4])
{
    Console.WriteLine($"{tuple.Month} month, {tuple.AvgTemperature} degrees, " +
                 $"{tuple.Member}, {tuple.AvgTime.Hours} hours , {tuple.AvgTime.Minutes} minutes ");
}

//Console.WriteLine("Hello, World!");

