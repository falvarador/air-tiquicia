using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/flight-types")]
public class FlightTypeController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<FlightTypeController> _logger;

    public FlightTypeController(ICommand command, ILogger<FlightTypeController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteFlightType(int id)
    {
        _logger.LogInformation("Delete specific flight type");

        await _command
            .Sql("delete from dbo.Flight_Types where flight_type_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting flight type"))
            .Exec();
    }

    [HttpGet]
    public async Task GetFlightTypes()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get flight types");

        await _command
            .Sql(@"select flight_type_id as flightTypeId, description from dbo.Flight_Types FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting flight types"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetFlightTypeById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get flight type by id");

        await _command
            .Sql(@"select flight_type_id as flightTypeId, description from dbo.Flight_Types
                where flight_type_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting flight type by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddFlightType([FromBody] FlightTypeDto flightTypeDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new flight type");

        FlightType flightType = new()
        {
            FlightTypeId = rnd.Next(1, 100000),
            Description = flightTypeDto.Description
        };

        await _command
            .Sql(@"insert into dbo.Flight_Types (flight_type_id, description) 
                values (@FlightTypeId, @Description)")
            .Param("FlightTypeId", flightType.FlightTypeId)
            .Param("Description", flightType.Description)
            .OnError(ex => _logger.LogError(ex, "Error creating flight type"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateFlightType([FromBody] FlightTypeDto roleDto)
    {
        _logger.LogInformation("Update an existing flight type");

        FlightType flightType = new()
        {
            FlightTypeId = roleDto.FlightTypeId,
            Description = roleDto.Description
        };

        await _command
            .Sql(@"update dbo.Flight_Types set description = @Description 
                where flight_type_id = @FlightTypeId")
            .Param("FlightTypeId", flightType.FlightTypeId)
            .Param("Description", flightType.Description)
            .OnError(ex => _logger.LogError(ex, "Error updating flight type"))
            .Exec();
    }
}
