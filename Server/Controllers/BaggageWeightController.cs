using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("api/baggages-weight")]
public class BaggageWeightController : ControllerBase
{
    private readonly IDbConnection _connection;
    private readonly ILogger<BaggageWeightController> _logger;

    public BaggageWeightController(IDbConnection connection, ILogger<BaggageWeightController> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBaggageWeight(int id)
    {
        _logger.LogInformation("Deleteing baggage weight");

        var baggageWeight = await _connection.QueryFirstOrDefaultAsync<BaggageWeight>(@"
            select * from dbo.Baggage_Weights where id = @Id", new { Id = id });

        if (baggageWeight is null)
        {
            _logger.LogInformation("Baggage weight not found");
            return NotFound();
        }

        await _connection.ExecuteAsync(@"
            delete from dbo.Baggages_Weight where id = @id", new { id });

        _logger.LogInformation("Baggage weight deleted");

        return NoContent();
    }

    [HttpGet]
    public async Task<IEnumerable<BaggageWeightDto>> GetBaggagesWeight()
    {
        _logger.LogInformation("Getting baggage weights");

        var baggageWeights = await _connection.QueryAsync<BaggageWeightDto>(@"
            select id, weight, price from dbo.Baggages_Weight");

        _logger.LogInformation("Baggage weights retrieved");

        return baggageWeights;
    }

    [HttpGet("{id:int}")]
    public async Task<BaggageWeightDto> GetBaggageWeightById(int id)
    {
        _logger.LogInformation("Getting baggage weight");

        var baggageWeight = await _connection.QueryFirstOrDefaultAsync<BaggageWeightDto>(@"
            select id, weight, price from dbo.Baggages_Weight where id = @id", new { id });

        _logger.LogInformation("Baggage weight retrieved");

        return baggageWeight;
    }

    [HttpPost]
    public async Task<IActionResult> AddBaggageWeight([FromBody] BaggageWeightDto baggageWeightDto)
    {
        Random rnd = new();

        _logger.LogInformation("Adding baggage weight");

        var baggageWeightId = await _connection.ExecuteScalarAsync<int>(@"
            insert into dbo.Baggages_Weight (weight, price) 
            values (@Weight, @Price)
            select cast(scope_identity() as varchar(10))", baggageWeightDto);

        _logger.LogInformation("Baggage weight added");

        return Created($"api/baggages-weight/{baggageWeightId}",
            new BaggageWeight
            {
                Id = baggageWeightId,
                Weight = baggageWeightDto.Weight,
                Price = baggageWeightDto.Price
            });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBaggageWeight([FromBody] BaggageWeightDto baggageWeightDto)
    {
        _logger.LogInformation("Updating baggage weight");

        var baggageWeight = await _connection.QueryFirstOrDefaultAsync<BaggageWeight>(@"
            select * from dbo.Baggages_Weight where id = @Id", new { Id = baggageWeightDto.Id });

        if (baggageWeight is null)
            return await AddBaggageWeight(baggageWeightDto);

        await _connection.ExecuteAsync(@"update dbo.Baggages_Weight set weight = @Weight, price = @Price where id = @Id",
            new
            {
                baggageWeightDto.Id,
                baggageWeightDto.Price,
                baggageWeightDto.Weight
            });

        return NoContent();
    }
}
