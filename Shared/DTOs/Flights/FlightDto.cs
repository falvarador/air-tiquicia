using System.ComponentModel.DataAnnotations;

public class FlightDto
{
    public FlightDto()
    {
        Number = string.Empty;
        DepartureDate = DateTime.Now;
        ArrivalDate = DateTime.Now + TimeSpan.FromDays(1);
    }

    [Required, StringLength(50)]
    public string Number { get; set; }
    public int AircraftId { get; set; }
    [Required, Range(1, 23)]
    public Int16 DurationHours { get; set; }
    [Required, Range(0, 60)]
    public Int16 DurationMinutes { get; set; }
    [Required]
    public DateTime DepartureDate { get; set; }
    [Required]
    public DateTime ArrivalDate { get; set; }
    [Required, Range(0, 23)]
    public Int16 DepartureHours { get; set; }
    [Required, Range(0, 60)]
    public Int16 DepartureMinutes { get; set; }
    [Required, Range(0, 23)]
    public Int16 ArrivalHours { get; set; }
    [Required, Range(0, 60)]
    public Int16 ArrivalMinutes { get; set; }
    [RequiredInteger]
    public int FlightTypeId { get; set; }
    [RequiredInteger]
    public int DepartureDestinationId { get; set; }
    [RequiredInteger]
    public int ArrivalDestinationId { get; set; }
    // [RequiredInteger]
    public int CrewFlightId { get; set; }
}
