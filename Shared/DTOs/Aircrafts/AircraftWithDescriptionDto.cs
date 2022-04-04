public class AircraftWithDescriptionDto
{
    public AircraftWithDescriptionDto()
    {
        AircraftId = string.Empty;
        Description = string.Empty;
        TypeDescription = string.Empty;
    }

    public string AircraftId { get; set; }
    public string Description { get; set; }
    public string TypeDescription { get; set; }
}
