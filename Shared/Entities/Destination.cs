public class Destination
{
    public Destination()
    {
        DestinationId = string.Empty;
        Name = string.Empty;
        Location = string.Empty;
    }

    public int Id { get; set; }
    public string DestinationId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
}
