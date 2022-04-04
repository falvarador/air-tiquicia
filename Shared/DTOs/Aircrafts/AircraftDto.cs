using System.ComponentModel.DataAnnotations;

public class AircraftDto
{
    public AircraftDto()
    {
        AircraftId = string.Empty;
        Description = string.Empty;
        TypeId = string.Empty;
    }

    public string AircraftId { get; set; }
    [Required, MaxLength(250)]
    public string Description { get; set; }
    [RequiredInteger]
    public string TypeId { get; set; }
}
