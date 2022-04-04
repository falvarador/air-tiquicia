using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<UserController> _logger;
    private readonly JsonWebToken _jwt;

    public UserController(ICommand command, ILogger<UserController> logger, JsonWebToken jwt)
    {
        _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteUser(int id)
    {
        _logger.LogInformation("Delete specific user");

        await _command
            .Sql("delete from dbo.Users where user_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting user"))
            .Exec();
    }

    [HttpGet]
    public async Task GetUsers()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get users");

        await _command
            .Sql(@"select user_id as userId, us.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, us.role_id roleId, rl.name as rolName, username, password
                from dbo.Users us
                    inner join dbo.People pe on pe.id = us.person_pk_id
                    inner join dbo.Person_Types pt on pt.person_pk_id = us.person_pk_id
                        and pt.[type] = 3
                    inner join dbo.Roles rl on rl.id = us.role_pk_id FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting users"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("people")]
    public async Task GetPeopleToUsers()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get people to users");

        await _command
            .Sql(@"select pe.person_id as personId, pe.name, last_name as lastName
                from dbo.People pe
                    inner join dbo.Person_Types pt on pt.person_pk_id = pe.id
                        and pt.[type] = 3 FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting people to users"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetUserById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get user by id");

        await _command
            .Sql(@"select user_id as userId, us.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, us.role_id roleId, rl.name as rolName, username, password
                from dbo.Users us
                    inner join dbo.People pe on pe.id = us.person_pk_id
                    inner join dbo.Person_Types pt on pt.person_pk_id = us.person_pk_id
                        and pt.[type] = 3
                    inner join dbo.Roles rl on rl.id = us.role_pk_id
                                where user_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting user by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddUser([FromBody] UserDto userDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new user");

        User user = new()
        {
            UserId = rnd.Next(1, 100000),
            PersonId = userDto.PersonId,
            RoleId = userDto.RoleId,
            Username = userDto.Username,
            Password = userDto.Password
        };

        await _command
            .Sql(@"declare @PersonPkId int, @RolePkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                select @RolePkId = id from dbo.Roles
                where role_id = @RoleId

                insert into dbo.Users (user_id, person_pk_id, person_id, role_pk_id, role_id, username, password) 
                values (@UserId, @PersonPkId, @PersonId, @RolePkId, @RoleId, @Username, @Password)")
            .Param("UserId", user.UserId)
            .Param("PersonId", user.PersonId)
            .Param("RoleId", user.RoleId)
            .Param("Username", user.Username)
            .Param("Password", user.Password)
            .OnError(ex => _logger.LogError(ex, "Error creating user"))
            .Exec();
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
    {
        _logger.LogInformation("Login user");

        bool flag = false;
        IEnumerable<string> roles = new List<string>() { "admin", "user" };

        await _command
             .Sql(@"select username, password from dbo.Users
                where username = @Username
                    and [password] = @Password")
             .Param("Username", userInfo.Username)
             .Param("Password", userInfo.Password)
             .OnError(ex => _logger.LogError(ex, "Error login user"))
             .Map(reader =>
             {
                 flag = (true) ? true : false;
             });

        return (flag) ? _jwt.CreateJsonWebToken(userInfo, roles) : new UserToken();
    }

    [HttpPut]
    public async Task UpdateUser([FromBody] UserDto userDto)
    {
        _logger.LogInformation("Update an existing user");

        User user = new()
        {
            RoleId = userDto.RoleId,
            UserId = userDto.UserId,
            PersonId = userDto.PersonId,
            Username = userDto.Username,
            Password = userDto.Password,
        };

        await _command
            .Sql(@"declare @PersonPkId int, @RolePkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                select @RolePkId = id from dbo.Roles
                where role_id = @RoleId

                update dbo.Users set person_pk_id = @PersonPkId, person_id = @PersonId, role_pk_id = @RolePkId, role_id = @RoleId, username = @Username, password = @Password 
                where user_id = @UserId")
            .Param("UserId", user.UserId)
            .Param("PersonId", user.PersonId)
            .Param("RoleId", user.RoleId)
            .Param("Username", user.Username)
            .Param("Password", user.Password)
            .OnError(ex => _logger.LogError(ex, "Error updating user"))
            .Exec();
    }
}
