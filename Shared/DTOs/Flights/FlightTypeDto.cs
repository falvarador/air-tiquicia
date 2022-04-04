using System.ComponentModel.DataAnnotations;

public class FlightTypeDto
{
    public FlightTypeDto()
    {
        Description = string.Empty;
    }

    public int FlightTypeId { get; set; }
    [Required, StringLength(50)]
    public string Description { get; set; }
}
