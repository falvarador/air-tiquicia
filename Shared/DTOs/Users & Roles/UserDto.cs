using System.ComponentModel.DataAnnotations;

public class UserDto
{
    public UserDto()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    [Required]
    public int UserId { get; set; }
    [RequiredInteger]
    public int PersonId { get; set; }
    [RequiredInteger]
    public int RoleId { get; set; }
    [Required, StringLength(50)]
    public string Username { get; set; }
    [Required, StringLength(250)]
    public string Password { get; set; }
}
