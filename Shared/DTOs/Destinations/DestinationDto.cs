using System.ComponentModel.DataAnnotations;

public class DestinationDto
{
    public DestinationDto()
    {
        DestinationId = string.Empty;
        Name = string.Empty;
        Location = string.Empty;
    }

    public int Id { get; set; }
    public string DestinationId { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; }
    [Required, StringLength(50)]
    public string Location { get; set; }
}
