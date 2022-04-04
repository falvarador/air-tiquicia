using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/aircrafts")]
public class AircraftController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<AircraftController> _logger;

    public AircraftController(ICommand command, ILogger<AircraftController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeleteAircraft(string id)
    {
        _logger.LogInformation("Delete specific aircraft");

        await _command
            .Sql("delete from dbo.Aircrafts where aircraft_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting aircraft"))
            .Exec();
    }

    [HttpGet]
    public async Task GetAircrafts()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get aircrafts");

        await _command
            .Sql(@"select aircraft_id as aircraftId, ac.description, act.type_id as typeId, act.description as typeDescription from dbo.Aircrafts ac 
                inner join dbo.Aircraft_Types act on ac.type_id = act.type_id FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting aircrafts"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id}")]
    public async Task GetAircraftById(string id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get aircraft by id");

        await _command
            .Sql(@"select aircraft_id as aircraftId, ac.description, act.type_id as typeId, act.description as typeDescription from dbo.Aircrafts ac 
                inner join dbo.Aircraft_Types act on ac.type_id = act.type_id
                where aircraft_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting aircraft by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddAircraft([FromBody] AircraftDto aircraftDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new aircraft");

        Aircraft aircraft = new()
        {
            AircraftId = $"{rnd.Next(1, 100000)}",
            Description = aircraftDto.Description,
            TypeId = aircraftDto.TypeId
        };

        await _command
            .Sql(@"insert into dbo.Aircrafts (aircraft_id, description, type_id) 
                values (@AircraftId, @Description, @TypeId)")
            .Param("AircraftId", aircraft.AircraftId)
            .Param("Description", aircraft.Description)
            .Param("TypeId", aircraft.TypeId)
            .OnError(ex => _logger.LogError(ex, "Error creating aircraft"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateAircraft([FromBody] AircraftDto aircraftDto)
    {
        _logger.LogInformation("Update an existing aircraft");

        Aircraft aircraft = new()
        {
            AircraftId = aircraftDto.AircraftId,
            Description = aircraftDto.Description,
            TypeId = aircraftDto.TypeId
        };

        await _command
            .Sql(@"update dbo.Aircrafts set description = @Description, type_id = @TypeId 
                where aircraft_id = @AircraftId")
            .Param("AircraftId", aircraft.AircraftId)
            .Param("Description", aircraft.Description)
            .Param("TypeId", aircraft.TypeId)
            .OnError(ex => _logger.LogError(ex, "Error updating aircraft"))
            .Exec();
    }
}
