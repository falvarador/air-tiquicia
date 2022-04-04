using System.ComponentModel.DataAnnotations;

public class CrewDto
{
    public CrewDto()
    {
        CrewId = string.Empty;
        Description = string.Empty;
    }

    public string CrewId { get; set; }
    [Required, StringLength(250)]
    public string Description { get; set; }
}
