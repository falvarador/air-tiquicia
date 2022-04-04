using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/aircraft-types")]
public class AircraftTypeController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<AircraftTypeController> _logger;

    public AircraftTypeController(ICommand command, ILogger<AircraftTypeController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeleteAircraftType(string id)
    {
        _logger.LogInformation("Delete specific aircraft type");

        await _command
            .Sql("delete from dbo.Aircraft_Types where type_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting aircraft type"))
            .Exec();
    }

    [HttpGet]
    public async Task GetAircraftTypes()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get aircrafts types");

        await _command
            .Sql(@"select type_id as typeId, description, rows, seats from dbo.Aircraft_Types FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting aircrafts types"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id}")]
    public async Task GetAircraftTypeById(string id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get aircraft type by id");

        await _command
            .Sql(@"select type_id as typeId, description, rows, seats from dbo.Aircraft_Types 
                where type_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting aircraft type by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddAircraftType([FromBody] AircraftTypeDto aircraftTypeDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new aircraft type");

        AircraftType aircraftType = new()
        {
            TypeId = $"{rnd.Next(1, 100000)}",
            Description = aircraftTypeDto.Description,
            Rows = aircraftTypeDto.Rows,
            Seats = aircraftTypeDto.Seats
        };

        await _command
            .Sql(@"insert into dbo.Aircraft_Types (type_id, description, rows, seats) 
                values (@TypeId, @Description, @Rows, @Seats)")
            .Param("TypeId", aircraftType.TypeId)
            .Param("Description", aircraftType.Description)
            .Param("Rows", aircraftType.Rows)
            .Param("Seats", aircraftType.Seats)
            .OnError(ex => _logger.LogError(ex, "Error creating aircraft"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateAircraftType([FromBody] AircraftTypeDto aircraftTypeDto)
    {
        _logger.LogInformation("Update an existing aircraft type");

        AircraftType aircraftType = new()
        {
            TypeId = aircraftTypeDto.TypeId,
            Description = aircraftTypeDto.Description,
            Rows = aircraftTypeDto.Rows,
            Seats = aircraftTypeDto.Seats,
        };

        await _command
            .Sql(@"update dbo.Aircraft_Types set description = @Description, rows = @Rows, seats = @Seats 
                where type_id = @TypeId")
            .Param("TypeId", aircraftType.TypeId)
            .Param("Description", aircraftType.Description)
            .Param("Rows", aircraftType.Rows)
            .Param("Seats", aircraftType.Seats)
            .OnError(ex => _logger.LogError(ex, "Error updating aircraft type"))
            .Exec();
    }
}
