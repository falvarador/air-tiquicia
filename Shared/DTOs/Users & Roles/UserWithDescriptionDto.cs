public class UserWithDescription
{
    public UserWithDescription()
    {
        PersonFullName = string.Empty;
        RolName = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
    }

    public int UserId { get; set; }
    public int PersonId { get; set; }
    public int RoleId { get; set; }
    public string PersonFullName { get; set; }
    public string RolName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
