public class PersonTypeWithDescriptionDto
{
    public PersonTypeWithDescriptionDto()
    {
        PersonFullName = string.Empty;
    }

    public int PersonTypeId { get; set; }
    public int PersonId { get; set; }
    public string PersonFullName { get; set; }
    public PersonTypeEnum Type { get; set; }
}
