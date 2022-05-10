using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("api/aircraft-types")]
public class AircraftTypeController : ControllerBase
{
    private readonly IDbConnection _connection;
    private readonly ILogger<AircraftTypeController> _logger;

    public AircraftTypeController(IDbConnection connection, ILogger<AircraftTypeController> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAircraftType(string id)
    {
        _logger.LogInformation("Deleteing aircraft type");

        var aircraftType = await _connection.QueryFirstOrDefaultAsync<AircraftType>("select * from dbo.Aircraft_Types where type_id = @Id", new { Id = id });

        if (aircraftType is null)
        {
            _logger.LogInformation("Aircraft type not found");
            return NotFound();
        }

        await _connection.ExecuteAsync(@"delete from dbo.Aircraft_Types where type_id = @id", new { id });

        _logger.LogInformation("Aircraft type deleted");

        return NoContent();
    }

    [HttpGet]
    public async Task<IEnumerable<AircraftTypeDto>> GetAircraftTypes()
    {
        _logger.LogInformation("Getting aircraft types");

        var aircraftTypes = await _connection.QueryAsync<AircraftTypeDto>(@"
            select type_id as typeId, description, rows, seats 
            from dbo.Aircraft_Types");

        _logger.LogInformation("Aircraft types retrieved");

        return aircraftTypes;
    }

    [HttpGet("{id}")]
    public async Task<AircraftTypeDto> GetAircraftTypeById(string id)
    {
        _logger.LogInformation("Getting aircraft type");

        var aircraftType = await _connection.QueryFirstOrDefaultAsync<AircraftTypeDto>(@"
            select type_id as typeId, description, rows, seats 
            from dbo.Aircraft_Types 
                where type_id = @id");

        _logger.LogInformation("Aircraft type retrieved");

        return aircraftType;
    }

    [HttpPost]
    public async Task<IActionResult> AddAircraftType([FromBody] AircraftTypeDto aircraftTypeDto)
    {
        Random rnd = new();

        _logger.LogInformation("Adding aircraft type");

        var aircraftType = new
        {
            TypeId = $"{rnd.Next(1, 100000)}",
            Description = aircraftTypeDto.Description,
            Rows = aircraftTypeDto.Rows,
            Seats = aircraftTypeDto.Seats
        };

        var aircraftTypeId = await _connection.ExecuteScalarAsync<int>(@"
            insert into dbo.Aircraft_Types (description, rows, seats) 
            values (@Description, @Rows, @Seats) 
            select cast(scope_identity() as varchar(10))", aircraftType);

        _logger.LogInformation("Aircraft type added");

        return Created($"api/aircraft-types/{aircraftTypeId}", aircraftType);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAircraftType([FromBody] AircraftTypeDto aircraftTypeDto)
    {
        _logger.LogInformation("Updating aircraft type");

        var aircraft = await _connection.QueryFirstOrDefaultAsync<AircraftType>(@"
            select * from dbo.Aircraft_Types where type_id = @Id", new { Id = aircraftTypeDto.TypeId });

        if (aircraft is null)
            return await AddAircraftType(aircraftTypeDto);

        await _connection.ExecuteAsync(@"
            update dbo.Aircraft_Types set description = @Description, rows = @Rows, seats = @Seats 
            where type_id = @TypeId", new
        {
            aircraftTypeDto.TypeId,
            aircraftTypeDto.Description,
            aircraftTypeDto.Rows,
            aircraftTypeDto.Seats,
        });

        _logger.LogInformation("Aircraft type updated");

        return NoContent();
    }
}
