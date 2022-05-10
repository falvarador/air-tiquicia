using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("api/crews")]
public class CrewController : ControllerBase
{
    private readonly IDbConnection _connection;
    private readonly ILogger<CrewController> _logger;

    public CrewController(IDbConnection connection, ILogger<CrewController> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCrew(string id)
    {
        _logger.LogInformation("Deleteing crew");

        var crew = await _connection.QueryFirstOrDefaultAsync<Crew>(@"
            select * from dbo.Crews where crew_id = @Id", new { Id = id });

        if (crew is null)
        {
            _logger.LogInformation("Crew not found");
            return NotFound();
        }

        await _connection.ExecuteAsync(@"delete from dbo.Crews where crew_id = @id", new { id });

        _logger.LogInformation("Crew deleted");

        return NoContent();
    }

    [HttpGet]
    public async Task<IEnumerable<CrewDto>> GetCrews()
    {
        _logger.LogInformation("Getting crews");

        var crews = await _connection.QueryAsync<CrewDto>(@"
            select crew_id as crewId, description from dbo.Crews");

        _logger.LogInformation("Crews retrieved");

        return crews;
    }

    [HttpGet("{id}")]
    public async Task<CrewDto> GetCrewById(string id)
    {
        _logger.LogInformation("Getting crew");

        var crew = await _connection.QueryFirstOrDefaultAsync<CrewDto>(@"
            select crew_id as crewId, description from dbo.Crews 
            where crew_id = @id", new { id });

        _logger.LogInformation("Crew retrieved");

        return crew;
    }

    [HttpPost]
    public async Task<IActionResult> AddCrew([FromBody] CrewDto crewDto)
    {
        Random rnd = new();

        _logger.LogInformation("Adding crew");

        var crew = new Crew
        {
            CrewId = $"{rnd.Next(1, 100000)}",
            Description = crewDto.Description
        };

        var crewId = await _connection.ExecuteScalarAsync<int>(@"
            insert into dbo.Crews (crew_id, description) 
            values (@CrewId, @Description)
            select cast(scope_identity() as int)", crew);

        _logger.LogInformation("Crew added");

        return Created($"api/crews/{crewId}", crew);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCrew([FromBody] CrewDto crewDto)
    {
        _logger.LogInformation("Updating crew");

        var crew = await _connection.QueryFirstOrDefaultAsync<Crew>(@"
            select * from dbo.Crews where crew_id = @Id", new { Id = crewDto.CrewId });

        if (crew is null)
            return await AddCrew(crewDto);

        await _connection.ExecuteAsync(@"
            update dbo.Crews set description = @Description
            where crew_id = @CrewId", new { crewDto.Description, crewDto.CrewId });

        return NoContent();
    }
}
