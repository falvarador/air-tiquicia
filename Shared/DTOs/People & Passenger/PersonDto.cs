using System.ComponentModel.DataAnnotations;

public class PersonDto
{
    public PersonDto()
    {
        PersonId = string.Empty;
        Name = string.Empty;
        LastName = string.Empty;
        Telephone = string.Empty;
        Direction = string.Empty;
        Email = string.Empty;
    }

    [Required]
    public string PersonId { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, MaxLength(100)]
    public string LastName { get; set; }
    [Required, MaxLength(25)]
    public string Telephone { get; set; }
    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; }
    [Required, MaxLength(250)]
    public string Direction { get; set; }
}
