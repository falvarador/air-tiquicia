public class FlightType
{
    public FlightType()
    {
        Description = string.Empty;
    }

    public int Id { get; set; }
    public int FlightTypeId { get; set; }
    public string Description { get; set; }
}
