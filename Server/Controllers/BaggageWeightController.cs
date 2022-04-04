using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/baggages-weight")]
public class BaggageWeightController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<BaggageWeightController> _logger;

    public BaggageWeightController(ICommand command, ILogger<BaggageWeightController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteBaggageWeight(int id)
    {
        _logger.LogInformation("Delete specific baggage weight");

        await _command
            .Sql("delete from dbo.Baggages_Weight where id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting baggage weight"))
            .Exec();
    }

    [HttpGet]
    public async Task GetBaggagesWeight()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get baggage weight");

        await _command
            .Sql(@"select id, weight, price from dbo.Baggages_Weight FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting baggage weight"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetBaggageWeightById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get baggage weight by id");

        await _command
            .Sql(@"select id, weight, price from dbo.Baggages_Weight where id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting baggage weight by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddBaggageWeight([FromBody] BaggageWeightDto baggageWeightDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new baggage weight");

        BaggageWeight baggageWeight = new()
        {
            Price = baggageWeightDto.Price,
            Weight = baggageWeightDto.Weight,
        };

        await _command
            .Sql(@"insert into dbo.Baggages_Weight (weight, price) values (@Weight, @Price)")
            .Param("Weight", baggageWeightDto.Weight)
            .Param("Price", baggageWeightDto.Price)
            .OnError(ex => _logger.LogError(ex, "Error creating baggage weight"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdateBaggageWeight([FromBody] BaggageWeightDto baggageWeightDto)
    {
        _logger.LogInformation("Update an existing baggage weight");

        BaggageWeight baggageWeight = new()
        {
            Id = baggageWeightDto.Id,
            Price = baggageWeightDto.Price,
            Weight = baggageWeightDto.Weight
        };

        await _command
            .Sql(@"update dbo.Baggages_Weight set weight = @Weight, price = @Price where id = @Id")
            .Param("Id", baggageWeight.Id)
            .Param("Weight", baggageWeight.Weight)
            .Param("Price", baggageWeight.Price)
            .OnError(ex => _logger.LogError(ex, "Error updating baggage weight"))
            .Exec();
    }
}
