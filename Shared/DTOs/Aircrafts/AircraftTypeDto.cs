using System.ComponentModel.DataAnnotations;

public class AircraftTypeDto
{
    public AircraftTypeDto()
    {
        TypeId = string.Empty;
        Description = string.Empty;
    }

    public string TypeId { get; set; }
    [Required, MaxLength(50)]
    public string Description { get; set; }
    [Required]
    public Int16 Rows { get; set; }
    [Required]
    public Int16 Seats { get; set; }
}
