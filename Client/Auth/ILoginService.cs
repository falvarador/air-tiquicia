public interface ILoginService
{
    Task Login(UserToken userToken);
    Task Logout();
    Task<string> RenewToken(string token);
}
