using System.ComponentModel.DataAnnotations;

public class SearchFlightDto
{
    public SearchFlightDto()
    {
        DepartureDate = DateTime.Now;
        ReturnDate = DateTime.Now + TimeSpan.FromDays(3);
        NumberOfPassengers = 1;
    }

    [RequiredInteger]
    public int Departure { get; set; }
    [RequiredInteger]
    public int Arrival { get; set; }
    [Required]
    public DateTime DepartureDate { get; set; }
    [Required]
    public DateTime ReturnDate { get; set; }
    [RequiredInteger]
    public int NumberOfPassengers { get; set; }
}
