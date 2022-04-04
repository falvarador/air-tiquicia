using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/crews")]
public class CrewController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<CrewController> _logger;

    public CrewController(ICommand command, ILogger<CrewController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeleteCrew(string id)
    {
        _logger.LogInformation("Delete specific crew");

        await _command
            .Sql("delete from dbo.Crews where crew_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting crew"))
            .Exec();
    }

    [HttpGet]
    public async Task GetCrews()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get crews");

        await _command
            .Sql(@"select crew_id as crewId, description from dbo.Crews FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting crews"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id}")]
    public async Task GetCrewById(string id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get crew by id");

        await _command
            .Sql(@"select crew_id as crewId, description from dbo.Crews where crew_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting crew by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddCrew([FromBody] CrewDto crewDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new crew");

        Crew crew = new()
        {
            CrewId = $"{rnd.Next(1, 100000)}",
            Description = crewDto.Description
        };

        await _command
            .Sql(@"insert into dbo.Crews (crew_id, description) values (@CrewId, @Description)")
            .Param("CrewId", crew.CrewId)
            .Param("Description", crew.Description)
            .OnError(ex => _logger.LogError(ex, "Error creating crew"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateCrew([FromBody] CrewDto crewDto)
    {
        _logger.LogInformation("Update an existing crew");

        Crew crew = new()
        {
            CrewId = crewDto.CrewId,
            Description = crewDto.Description
        };

        await _command
            .Sql(@"update dbo.Crews set description = @Description where crew_id = @CrewId")
            .Param("CrewId", crew.CrewId)
            .Param("Description", crew.Description)
            .OnError(ex => _logger.LogError(ex, "Error updating crew"))
            .Exec();
    }
}
