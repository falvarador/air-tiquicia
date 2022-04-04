using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/destinations")]
public class DestinationController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<DestinationController> _logger;

    public DestinationController(ICommand command, ILogger<DestinationController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeleteDestination(string id)
    {
        _logger.LogInformation("Delete specific destination");

        await _command
            .Sql("delete from dbo.Destinations where destination_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting destination"))
            .Exec();
    }

    [HttpGet]
    public async Task GetDestinations()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get destinations");

        await _command
            .Sql(@"select destination_id as destinationId, name, location from dbo.Destinations FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting destinations"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetDestinationById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get destination by id");

        await _command
            .Sql(@"select destination_id as destinationId, name, location from dbo.Destinations where destination_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting destination by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddDestination([FromBody] DestinationDto destinationDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new destination");

        Destination destination = new()
        {
            DestinationId = $"{rnd.Next(1, 100000)}",
            Name = destinationDto.Name,
            Location = destinationDto.Location,
        };

        await _command
            .Sql(@"insert into dbo.Destinations (destination_id, name, location) values (@DestinationId, @Name, @Location)")
                .Param("DestinationId", destination.DestinationId)
                .Param("Name", destination.Name)
                .Param("Location", destination.Location)
                .OnError(ex => _logger.LogError(ex, "Error creating destination"))
                .Exec();
    }

    [HttpPut]
    public async Task UpdateDestination([FromBody] DestinationDto destinationDto)
    {
        _logger.LogInformation("Update an existing destination");

        Destination destination = new()
        {
            DestinationId = destinationDto.DestinationId,
            Name = destinationDto.Name,
            Location = destinationDto.Location,
        };

        await _command
            .Sql(@"update dbo.Destinations set name = @Name, location = @Location where destination_id = @Id")
           .Param("Id", destinationDto.DestinationId)
                .Param("Name", destinationDto.Name)
                .Param("Location", destinationDto.Location)
            .OnError(ex => _logger.LogError(ex, "Error updating destination"))
            .Exec();
    }
}
