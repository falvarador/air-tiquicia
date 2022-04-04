public class CrewFlight
{
    public CrewFlight()
    {
        CrewFlightId = string.Empty;
        PersonCrewId = string.Empty;
    }

    public int Id { get; set; }
    public string CrewFlightId { get; set; }
    public string PersonCrewId { get; set; }
}
