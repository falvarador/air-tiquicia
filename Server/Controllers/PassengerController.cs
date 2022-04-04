using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/passengers")]
public class PassengerController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<PassengerController> _logger;

    public PassengerController(ICommand command, ILogger<PassengerController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task DeletePassenger(int id)
    {
        _logger.LogInformation("Delete specific passenger");

        await _command
            .Sql("delete from dbo.Passengers where passenger_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting passenger"))
            .Exec();
    }

    [HttpGet]
    public async Task GetPassengers()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get passengers");

        await _command
            .Sql(@"select passenger_id as passengerId, ps.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, quantity_baggage as quantityBaggage from dbo.Passengers ps
                    inner join dbo.People pe on pe.id = ps.person_pk_id
                    inner join dbo.Person_Types pt on pt.person_pk_id = ps.person_pk_id
                where pt.type = 2 FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting passengers"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("people")]
    public async Task GetPeopleToUsers()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get people to passengers");

        await _command
            .Sql(@"select pe.person_id as personId, pe.name, last_name as lastName
                from dbo.People pe
                    inner join dbo.Person_Types pt on pt.person_pk_id = pe.id
                        and pt.[type] = 2 FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting people to passe"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetPassengerById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get passenger by id");

        await _command
            .Sql(@"select passenger_id as passengerId, ps.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, quantity_baggage as quantityBaggage from dbo.Passengers ps
                    inner join dbo.People pe on pe.id = ps.person_pk_id
                    inner join dbo.Person_Types pt on pt.person_pk_id = ps.person_pk_id
                where pt.type = 2
                    and passenger_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting passenger by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddPassenger([FromBody] PassengerDto passengerDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new passenger");

        Passenger passenger = new()
        {
            PassengerId = rnd.Next(1, 100000),
            PersonId = passengerDto.PersonId,
            QuantityBaggage = passengerDto.QuantityBaggage
        };

        await _command
            .Sql(@"declare @PersonPkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                insert into dbo.Passengers (passenger_id, person_pk_id,  person_id, quantity_baggage) 
                values (@PassengerId, @PersonPkId,  @PersonId, @QuantityBaggage)")
                .Param("PassengerId", passenger.PassengerId)
                .Param("PersonId", passenger.PersonId)
                .Param("QuantityBaggage", passenger.QuantityBaggage)
                .OnError(ex => _logger.LogError(ex, "Error creating passenger"))
                .Exec();
    }

    [HttpPut]
    public async Task UpdatePassenger([FromBody] PassengerDto passengerDto)
    {
        _logger.LogInformation("Update an existing passenger");

        Passenger passenger = new()
        {
            PassengerId = passengerDto.PassengerId,
            PersonId = passengerDto.PersonId,
            QuantityBaggage = passengerDto.QuantityBaggage
        };

        await _command
            .Sql(@"declare @PersonPkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                update dbo.Passengers set person_pk_id = @PersonPkId, person_id = @PersonId, quantity_baggage = @QuantityBaggage 
                where passenger_id = @PassengerId")
            .Param("PassengerId", passenger.PassengerId)
            .Param("PersonId", passenger.PersonId)
            .Param("QuantityBaggage", passenger.QuantityBaggage)
            .OnError(ex => _logger.LogError(ex, "Error updating passenger"))
            .Exec();
    }
}
