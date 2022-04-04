public class User
{
    public User()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public int PersonId { get; set; }
    public int RoleId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
