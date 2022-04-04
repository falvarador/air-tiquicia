public class PersonTypeDto
{
    public int PersonTypeId { get; set; }
    [RequiredInteger]
    public int PersonId { get; set; }
    [RequiredEnumerable]
    public PersonTypeEnum Type { get; set; }
}
