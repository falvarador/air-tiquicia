public class Aircraft
{
    public Aircraft()
    {
        AircraftId = string.Empty;
        Description = string.Empty;
        TypeId = string.Empty;
    }

    public int Id { get; set; }
    public string AircraftId { get; set; }
    public string Description { get; set; }
    public string TypeId { get; set; }
}
