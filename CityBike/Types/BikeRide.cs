namespace CityBike.Types;

public class BikeRide
{
    public BikeRide(string rideId, DateTime startDate, DateTime endDate, string startStationId, string startStationName,
        string endStationId, string endStationName, string memberType)
    {
        RideId = rideId;
        StartDate = startDate;
        EndDate = endDate;
        StartStationId = startStationId;
        StartStationName = startStationName;
        EndStationId = endStationId;
        EndStationName = endStationName;
        MemberType = memberType;
    }
    public string RideId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartStationId {get; set;}
    public string StartStationName {get; set;}
    public string EndStationId {get; set;}
    public string EndStationName {get; set;}
    public string MemberType { get; set; }

    public void Print()
    {
        Console.WriteLine($"RideId: {RideId} \nStartDate: {StartDate}, EndDate: {EndDate} " +
                          $"\nStartStationId:{StartStationId}, StartStationName: {StartStationName}, " +
                          $"EndStationId: {EndStationId}, EndStationName: {EndStationName}" +
                          $"\n MemberType: {MemberType} \n");
    }
}