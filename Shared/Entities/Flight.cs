public class Flight
{
    public Flight()
    {
        Number = string.Empty;
    }

    public int Id { get; set; }
    public string Number { get; set; }
    public int AircraftId { get; set; }
    public Int16 DurationHours { get; set; }
    public Int16 DurationMinutes { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public Int16 DepartureHours { get; set; }
    public Int16 DepartureMinutes { get; set; }
    public Int16 ArrivalHours { get; set; }
    public Int16 ArrivalMinutes { get; set; }
    public int FlightTypeId { get; set; }
    public int DepartureDestinationId { get; set; }
    public int ArrivalDestinationId { get; set; }
    public decimal Price { get; set; }
}
