public interface ILoginService
{
    Task Login(string token);
    Task Logout();
    Task<string> RenewToken(string token);
}
