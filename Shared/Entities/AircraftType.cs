public class AircraftType
{
    public AircraftType()
    {
        TypeId = string.Empty;
        Description = string.Empty;
    }

    public int Id { get; set; }
    public string TypeId { get; set; }
    public string Description { get; set; }
    public Int16 Rows { get; set; }
    public Int16 Seats { get; set; }
}
