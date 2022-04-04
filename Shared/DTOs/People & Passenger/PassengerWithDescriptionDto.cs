public class PassengerWithDescriptionDto
{
    public PassengerWithDescriptionDto()
    {
        PersonFullName = string.Empty;
    }

    public int PassengerId { get; set; }
    public int PersonId { get; set; }
    public string PersonFullName { get; set; }
    public Int16 QuantityBaggage { get; set; }
}
