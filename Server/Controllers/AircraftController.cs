using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("api/aircrafts")]
public class AircraftController : ControllerBase
{
    private readonly IDbConnection _connection;
    private readonly ILogger<AircraftController> _logger;

    public AircraftController(IDbConnection connection, ILogger<AircraftController> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAircraft(string id)
    {
        _logger.LogInformation("Deleting aircraft");

        var aircraft = await _connection.QueryFirstOrDefaultAsync<Aircraft>("select * from dbo.Aircrafts where aircraft_id = @Id", new { Id = id });

        if (aircraft is null)
        {
            _logger.LogInformation("Aircraft not found");
            return NotFound();
        }

        await _connection.ExecuteAsync(@"delete from dbo.Aircrafts where aircraft_id = @id", new { id });

        _logger.LogInformation("Aircraft deleted");

        return NoContent();
    }

    [HttpGet]
    public async Task<IEnumerable<AircraftWithDescriptionDto>> GetAircrafts()
    {
        _logger.LogInformation("Getting aircrafts");

        var aircrafts = await _connection.QueryAsync<AircraftWithDescriptionDto>(@"
            select aircraft_id as aircraftId, ac.description, act.type_id as typeId, act.description as typeDescription 
            from dbo.Aircrafts ac 
                inner join dbo.Aircraft_Types act on ac.type_id = act.type_id");

        _logger.LogInformation("Aircrafts retrieved");

        return aircrafts;
    }

    [HttpGet("{id}")]
    public async Task<AircraftWithDescriptionDto> GetAircraftById(string id)
    {
        _logger.LogInformation("Getting aircraft");

        var aircraft = await _connection.QueryFirstOrDefaultAsync<AircraftWithDescriptionDto>(@"
            select aircraft_id as aircraftId, ac.description, act.type_id as typeId, act.description as typeDescription 
            from dbo.Aircrafts ac 
                inner join dbo.Aircraft_Types act on ac.type_id = act.type_id
            where aircraft_id = @id", new { Id = id });

        _logger.LogInformation("Aircraft retrieved");

        return aircraft;
    }

    [HttpPost]
    public async Task<IActionResult> AddAircraft([FromBody] AircraftDto aircraftDto)
    {
        Random rnd = new();

        _logger.LogInformation("Adding aircraft");

        Aircraft aircraft = new()
        {
            AircraftId = $"{rnd.Next(1, 100000)}",
            Description = aircraftDto.Description,
            TypeId = aircraftDto.TypeId
        };

        var aircraftId = await _connection.ExecuteScalarAsync<string>(@"
            insert into dbo.Aircrafts (aircraft_id, description, type_id) 
            values (@AircraftId, @Description, @TypeId)
            select cast(scope_identity() as varchar(10))", aircraft);

        _logger.LogInformation("Aircraft added");

        return Created($"/api/aircrafts/{aircraftId}", aircraft);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAircraft([FromBody] AircraftDto aircraftDto)
    {
        _logger.LogInformation("Update an existing aircraft");

        var aircraft = await _connection.QueryFirstOrDefaultAsync<Aircraft>("select * from dbo.Aircrafts where aircraft_id = @Id", new { Id = aircraftDto.AircraftId });

        if (aircraft is null)
            return await AddAircraft(aircraftDto);

        await _connection.ExecuteAsync(@"
            update dbo.Aircrafts set description = @Description, type_id = @TypeId 
                where aircraft_id = @AircraftId", new
        {
            aircraftDto.AircraftId,
            aircraftDto.Description,
            aircraftDto.TypeId
        });

        _logger.LogInformation("Aircraft updated");

        return NoContent();
    }
}
