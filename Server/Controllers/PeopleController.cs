using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/people")]
public class PeopleController : ControllerBase
{
    private readonly ICommand _command;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(ICommand command, IMemoryCache cache, ILogger<PeopleController> logger)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{id}")]
    public async Task DeletePerson(string id)
    {
        _logger.LogInformation("Delete specific person");

        await _command
            .Sql("delete from dbo.People where person_id = @id")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error deleting person"))
            .Exec();
    }

    [HttpGet]
    public async Task GetPeople()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get people");

        await _command
            .Sql(@"select person_id as personId, name, last_name as lastName, telephone, direction, email from dbo.People FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting people"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{id}")]
    public async Task GetPersonById(string id)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get person by id");

        await _command
            .Sql(@"select person_id as personId, name, last_name as lastName, telephone, direction, email from dbo.People 
                where person_id = @id FOR JSON PATH")
            .Param("id", id)
            .OnError(ex => _logger.LogError(ex, "Error geting person by id"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddPerson([FromBody] PersonDto personDto)
    {
        PersonDto cacheEntry = new PersonDto();
        _logger.LogInformation("Create new person");

        if (!_cache.TryGetValue("people", out cacheEntry))
        {
            cacheEntry = personDto;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set("people", cacheEntry, cacheEntryOptions);
        }

        People person = new()
        {
            PersonId = personDto.PersonId,
            Direction = personDto.Direction,
            Email = personDto.Email,
            LastName = personDto.LastName,
            Name = personDto.Name,
            Telephone = personDto.Telephone
        };



        await _command
            .Sql(@"insert into dbo.People (person_id, name, last_name, telephone, direction, email) 
                values (@PersonId, @Name, @LastName, @Telephone, @Direction, @Email)")
            .Param("PersonId", person.PersonId)
            .Param("Name", person.Name)
            .Param("LastName", person.LastName)
            .Param("Telephone", person.Telephone)
            .Param("Direction", person.Direction)
            .Param("Email", person.Email)
            .OnError(ex => _logger.LogError(ex, "Error creating person"))
            .Exec();
    }

    [HttpPut]
    public async Task UpdatePerson([FromBody] PersonDto personDto)
    {
        _logger.LogInformation("Update an existing person");

        People person = new()
        {
            PersonId = personDto.PersonId,
            Name = personDto.Name,
            LastName = personDto.LastName,
            Telephone = personDto.Telephone,
            Direction = personDto.Direction,
            Email = personDto.Email
        };

        await _command
            .Sql(@"update dbo.People set name = @Name, last_name = @LastName, telephone = @Telephone, 
                    direction = @Direction, email = @Email 
                where person_id = @PersonId")
            .Param("PersonId", person.PersonId)
            .Param("Name", person.Name)
            .Param("LastName", person.LastName)
            .Param("Telephone", person.Telephone)
            .Param("Direction", person.Direction)
            .Param("Email", person.Email)
            .OnError(ex => _logger.LogError(ex, "Error updating person"))
            .Exec();
    }
}
