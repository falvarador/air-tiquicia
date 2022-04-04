using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JsonWebToken
{
    private readonly IConfiguration _configuration;

    public JsonWebToken(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
    }

    public UserToken CreateJsonWebToken(UserInfo userInfo, IEnumerable<string> roles)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Username),
            new Claim(ClaimTypes.Name, userInfo.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(1);

        JwtSecurityToken token = new JwtSecurityToken(
           issuer: null,
           audience: null,
           claims: claims,
           expires: expiration,
           signingCredentials: credentials);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
