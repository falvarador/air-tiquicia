using Belgrade.SqlClient;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/flights")]
public class FlightController : ControllerBase
{
    private readonly ICommand _command;
    private readonly ILogger<FlightController> _logger;

    public FlightController(ICommand command, ILogger<FlightController> logger)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpDelete("{number}")]
    public async Task DeleteFlight(string number)
    {
        _logger.LogInformation("Delete specific flight");

        await _command
            .Sql("delete from dbo.Flights where number = @number")
            .Param("number", number)
            .OnError(ex => _logger.LogError(ex, "Error deleting flight"))
            .Exec();
    }

    [HttpGet]
    public async Task GetFlights()
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get users");

        await _command
            .Sql(@"select number, aircraft_id as aircraftId, duration_hours as durationHours, duration_minutes as durationMinutes, departure_date as departureDate,
                        arrival_date as arrivalDate, departure_hours departureHours, departure_minutes as departureMinutes, arrival_hours as arrivalHours, arrival_minutes as arrivalMinutes,
                        flight_type_id as flightTypeId, departure_destination_id as departureDestinationId, arrival_destination_id as arrivalDestinationId, price
                    from dbo.Flights FOR JSON PATH")
            .OnError(ex => _logger.LogError(ex, "Error geting users"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("{number}")]
    public async Task GetFlightByNumber(string number)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Get flight by number");

        await _command
            .Sql(@"select number, aircraft_id as aircraftId, duration_hours as durationHours, duration_minutes as durationMinutes, departure_date as departureDate,
                        arrival_date as arrivalDate, departure_hours departureHours, departure_minutes as departureMinutes, arrival_hours as arrivalHours, arrival_minutes as arrivalMinutes,
                        flight_type_id as flightTypeId, departure_destination_id as departureDestinationId, arrival_destination_id as arrivalDestinationId, price
                    from dbo.Flights where number = @number FOR JSON PATH")
            .Param("number", number)
            .OnError(ex => _logger.LogError(ex, "Error geting flight by number"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpGet("search/{departureDestinationId}/{arrivalDestinationId}/{departureDate:datetime}/{arrivalDate:datetime}/{passengers:int}")]
    public async Task GetFlightByNumber(string departureDestinationId, string arrivalDestinationId, string departureDate, string arrivalDate, int passengers)
    {
        Response.ContentType = "application/json";

        _logger.LogInformation("Search flights");

        await _command
            .Sql(@"declare @DepartureDestinationPkId int, @ArrivalDestinationPkId int
          
                select @DepartureDestinationPkId = id from dbo.Destinations
                where destination_id = @departureDestinationId

                select @ArrivalDestinationPkId = id from dbo.Destinations
                where destination_id = @arrivalDestinationId

                select number, aircraft_id as aircraftId, duration_hours as durationHours, duration_minutes as durationMinutes, departure_date as departureDate,
                    arrival_date as arrivalDate, departure_hours departureHours, departure_minutes as departureMinutes, arrival_hours as arrivalHours, arrival_minutes as arrivalMinutes,
                    flight_type_id as flightTypeId, departure_destination_id as departureDestinationId, arrival_destination_id as arrivalDestinationId, price
                from dbo.Flights 
                where departure_destination_id = @DepartureDestinationPkId
                and arrival_destination_id = coalesce(@ArrivalDestinationPkId, arrival_destination_id)
                and convert(date, departure_date) = @departureDate
                and convert(date, arrival_date) = @arrivalDate FOR JSON PATH")
            .Param("departureDestinationId", departureDestinationId)
            .Param("arrivalDestinationId", arrivalDestinationId)
            .Param("departureDate", departureDate)
            .Param("arrivalDate", arrivalDate)
            .OnError(ex => _logger.LogError(ex, "Error searching flights"))
            .Stream(Response.Body, defaultOutput: "[]");
    }

    [HttpPost]
    public async Task AddFlight([FromBody] FlightDto flightDto)
    {
        Random rnd = new();

        _logger.LogInformation("Create new flight");

        Flight flight = new()
        {
            Number = flightDto.Number,
            AircraftId = flightDto.AircraftId,
            DurationHours = flightDto.DurationHours,
            DurationMinutes = flightDto.DurationMinutes,
            DepartureDate = flightDto.DepartureDate,
            ArrivalDate = flightDto.ArrivalDate,
            DepartureHours = flightDto.DepartureHours,
            DepartureMinutes = flightDto.DepartureMinutes,
            ArrivalHours = flightDto.ArrivalHours,
            ArrivalMinutes = flightDto.ArrivalMinutes,
            FlightTypeId = flightDto.FlightTypeId,
            DepartureDestinationId = flightDto.DepartureDestinationId,
            ArrivalDestinationId = flightDto.ArrivalDestinationId,
            Price = flightDto.Price
        };

        await _command
            .Sql(@"
                    declare @AircraftPkId int, @FlightTypePkId int, @DepartureDestinationPkId int, @ArrivalDestinationPkId int

                    select @AircraftPkId = id from dbo.Aircrafts
                    where aircraft_id = @AircraftId

                    select @FlightTypePkId = id from dbo.Flight_Types
                    where flight_type_id = @FlightTypeId

                    select @DepartureDestinationPkId = id from dbo.Destinations
                    where destination_id = @DepartureDestinationId

                    select @ArrivalDestinationPkId = id from dbo.Destinations
                    where destination_id = @ArrivalDestinationId

                    insert into dbo.Flights (number, aircraft_id, duration_hours, duration_minutes, departure_date,
                        arrival_date, departure_hours, departure_minutes, arrival_hours, arrival_minutes,
                        flight_type_id, departure_destination_id, arrival_destination_id, price) 
                    values (@Number, @AircraftPkId, @DurationHours, @DurationMinutes, @DepartureDate, @ArrivalDate,
                        @DepartureHours, @DepartureMinutes, @ArrivalHours, @ArrivalMinutes, @FlightTypePkId,
                        @DepartureDestinationPkId, @ArrivalDestinationPkId, @Price)")
                .Param("Number", flightDto.Number)
                .Param("AircraftId", flightDto.AircraftId)
                .Param("DurationHours", flightDto.DurationHours)
                .Param("DurationMinutes", flightDto.DurationMinutes)
                .Param("DepartureDate", flightDto.DepartureDate.ToString("yyyy-MM-dd"))
                .Param("ArrivalDate", flightDto.ArrivalDate.ToString("yyyy-MM-dd"))
                .Param("DepartureHours", flightDto.DepartureHours)
                .Param("DepartureMinutes", flightDto.DepartureMinutes)
                .Param("ArrivalHours", flightDto.ArrivalHours)
                .Param("ArrivalMinutes", flightDto.ArrivalMinutes)
                .Param("FlightTypeId", flightDto.FlightTypeId)
                .Param("DepartureDestinationId", flightDto.DepartureDestinationId)
                .Param("ArrivalDestinationId", flightDto.ArrivalDestinationId)
                .Param("Price", flightDto.Price)
                .OnError(ex => _logger.LogError(ex, "Error creating flight"))
                .Exec();
    }

    [HttpPut]
    public async Task UpdateFlight([FromBody] FlightDto flightDto)
    {
        _logger.LogInformation("Update an existing flight");

        Flight flight = new()
        {
            Number = flightDto.Number,
            AircraftId = flightDto.AircraftId,
            DurationHours = flightDto.DurationHours,
            DurationMinutes = flightDto.DurationMinutes,
            DepartureDate = flightDto.DepartureDate,
            ArrivalDate = flightDto.ArrivalDate,
            DepartureHours = flightDto.DepartureHours,
            DepartureMinutes = flightDto.DepartureMinutes,
            ArrivalHours = flightDto.ArrivalHours,
            ArrivalMinutes = flightDto.ArrivalMinutes,
            FlightTypeId = flightDto.FlightTypeId,
            DepartureDestinationId = flightDto.DepartureDestinationId,
            ArrivalDestinationId = flightDto.ArrivalDestinationId,
            Price = flightDto.Price
        };

        await _command
            .Sql(@"
                    declare @AircraftPkId int, @FlightTypePkId int, @DepartureDestinationPkId int, @ArrivalDestinationPkId int

                    select @AircraftPkId = id from dbo.Aircrafts
                    where aircraft_id = @AircraftId

                    select @FlightTypePkId = id from dbo.Flight_Types
                    where flight_type_id = @FlightTypeId

                    select @DepartureDestinationPkId = id from dbo.Destinations
                    where destination_id = @DepartureDestinationId

                    select @ArrivalDestinationPkId = id from dbo.Destinations
                    where destination_id = @ArrivalDestinationId

                    insert into dbo.Flights (number, aircraft_id, duration_hours, duration_minutes, departure_date,
                        arrival_date, departure_hours, departure_minutes, arrival_hours, arrival_minutes,
                        flight_type_id, departure_destination_id, arrival_destination_id, price) 
                    values (@Number, @AircraftPkId, @DurationHours, @DurationMinutes, @DepartureDate, @ArrivalDate,
                        @DepartureHours, @DepartureMinutes, @ArrivalHours, @ArrivalMinutes, @FlightTypePkId,
                        @DepartureDestinationPkId, @ArrivalDestinationPkId, @Price
                    where number = @Number")
            .Param("Number", flightDto.Number)
            .Param("AircraftId", flightDto.AircraftId)
            .Param("DurationHours", flightDto.DurationHours)
            .Param("DurationMinutes", flightDto.DurationMinutes)
            .Param("DepartureDate", flightDto.DepartureDate.ToString("yyyy-MM-dd"))
            .Param("ArrivalDate", flightDto.ArrivalDate.ToString("yyyy-MM-dd"))
            .Param("DepartureHours", flightDto.DepartureHours)
            .Param("DepartureMinutes", flightDto.DepartureMinutes)
            .Param("ArrivalHours", flightDto.ArrivalHours)
            .Param("ArrivalMinutes", flightDto.ArrivalMinutes)
            .Param("FlightTypeId", flightDto.FlightTypeId)
            .Param("DepartureDestinationId", flightDto.DepartureDestinationId)
            .Param("ArrivalDestinationId", flightDto.ArrivalDestinationId)
            .Param("Price", flightDto.Price)
            .OnError(ex => _logger.LogError(ex, "Error updating flight"))
            .Exec();
    }
}
