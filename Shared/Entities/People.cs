public class People
{
    public People()
    {
        PersonId = string.Empty;
        Name = string.Empty;
        LastName = string.Empty;
        Telephone = string.Empty;
        Direction = string.Empty;
        Email = string.Empty;
    }

    public int Id { get; set; }
    public string PersonId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Direction { get; set; }
    public string Email { get; set; }
}
