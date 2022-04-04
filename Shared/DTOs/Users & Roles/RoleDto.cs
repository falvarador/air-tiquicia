using System.ComponentModel.DataAnnotations;

public class RoleDto
{
    public RoleDto()
    {
        RoleId = string.Empty;
        Name = string.Empty;
    }

    public string RoleId { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; }
}
