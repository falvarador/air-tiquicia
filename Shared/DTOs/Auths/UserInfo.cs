using System.ComponentModel.DataAnnotations;

public class UserInfo
{
    public UserInfo()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
