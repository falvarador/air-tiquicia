using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<RoleController> _logger;

    public RoleController(ICommand command, ILogger<RoleController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeleteRole(string id)
    {
        _logger.LogInformation("Delete specific role");

        await _command
            .Sql("delete from dbo.Roles where role_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting role"))
            .Exec();
    }

    [HttpGet]
    public async Task GetRoles()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get roles");

        await _command
            .Sql(@"select role_id as roleId, name from dbo.Roles FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting roles"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id}")]
    public async Task GetCrewById(string id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get role by id");

        await _command
            .Sql(@"select role_id as roleId, name from dbo.Roles where role_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting role by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddRole([FromBody] RoleDto roleDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new role");

        Role role = new()
        {
            RoleId = $"{rnd.Next(1, 100000)}",
            Name = roleDto.Name
        };

        await _command
            .Sql(@"insert into dbo.Roles (role_id, name) values (@RoleId, @Name)")
            .Param("RoleId", role.RoleId)
            .Param("Name", role.Name)
            .OnError(ex => _logger.LogError(ex, "Error creating role"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateRole([FromBody] RoleDto roleDto)
    {
        _logger.LogInformation("Update an existing role");

        Role role = new()
        {
            RoleId = roleDto.RoleId,
            Name = roleDto.Name
        };

        await _command
            .Sql(@"update dbo.Roles set name = @Name where role_id = @RoleId")
            .Param("RoleId", role.RoleId)
            .Param("Name", role.Name)
            .OnError(ex => _logger.LogError(ex, "Error updating role"))
            .Exec();
    }
}
