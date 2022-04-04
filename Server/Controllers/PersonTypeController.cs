using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/person-types")]
public class PersonTypeController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<PersonTypeController> _logger;

    public PersonTypeController(ICommand command, ILogger<PersonTypeController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id:int}")]
    public async Task DeletePersonType(int id)
    {
        _logger.LogInformation("Delete specific person type");

        await _command
            .Sql("delete from dbo.Person_Types where person_type_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting person type"))
            .Exec();
    }

    [HttpGet]
    public async Task GetPersonTypes()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get person types");

        await _command
            .Sql(@"select person_type_id as personTypeId, pt.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, type from dbo.Person_Types pt
                    inner join dbo.People pe on pe.id = person_pk_id FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting person types"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id:int}")]
    public async Task GetPersonTypeById(int id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get person type by id");

        await _command
            .Sql(@"select person_type_id as personTypeId, pt.person_id as personId, pe.name + ' ' + pe.last_name as personFullName, type from dbo.Person_Types pt
                    inner join dbo.People pe on pe.id = person_pk_id 
                where person_type_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting person type by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddPersonType([FromBody] PersonTypeDto personTypeDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new person type");

        PersonType personType = new()
        {
            PersonTypeId = rnd.Next(1, 100000),
            PersonId = personTypeDto.PersonId,
            Type = personTypeDto.Type
        };

        await _command
            .Sql(@"declare @PersonPkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                insert into dbo.Person_Types (person_type_id, person_pk_id, person_id, type) 
                values (@PersonTypeId, @PersonPkId, @PersonId, @Type)")
            .Param("PersonTypeId", personType.PersonTypeId)
            .Param("PersonId", personType.PersonId)
            .Param("Type", personType.Type)
            .OnError(ex => _logger.LogError(ex, "Error creating person type"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdatePersonType([FromBody] PersonTypeDto personTypeDto)
    {
        _logger.LogInformation("Update an existing person type");

        PersonType personType = new()
        {
            PersonTypeId = personTypeDto.PersonTypeId,
            PersonId = personTypeDto.PersonId,
            Type = personTypeDto.Type
        };

        await _command
            .Sql(@"declare @PersonPkId int

                select @PersonPkId = id from dbo.People
                where person_id = @PersonId

                update dbo.Person_Types set person_pk_id = @PersonPkId, person_id = @PersonId, type = @Type 
                where person_type_id = @PersonTypeId")
            .Param("PersonTypeId", personType.PersonTypeId)
            .Param("PersonId", personType.PersonId)
            .Param("Type", personType.Type)
            .OnError(ex => _logger.LogError(ex, "Error updating person type"))
            .Exec();
    }
}
