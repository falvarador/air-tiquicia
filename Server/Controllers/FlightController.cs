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
            .Sql(@"select number, aircraft_id, duration_hours, duration_minutes, departure_date,
                        arrival_date, departure_hours, departure_minutes, arrival_hours, arrival_minutes,
                        flight_type_id, departure_destination_id, arrival_destination_id, crew_flight_id 
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
            .Sql(@"select number, aircraft_id, duration_hours, duration_minutes, departure_date,
                        arrival_date, departure_hours, departure_minutes, arrival_hours, arrival_minutes,
                        flight_type_id, departure_destination_id, arrival_destination_id, crew_flight_id 
                    from dbo.Flights where number = @number FOR JSON PATH")
            .Param("number", number)
            .OnError(ex => _logger.LogError(ex, "Error geting flight by number"))
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
            CrewFlightId = flightDto.CrewFlightId
        };

        // await _command
        //     .Sql(@"insert into dbo.Flights (number, aircraft_id, duration_hours, duration_minutes, departure_date,
        //                 arrival_date, departure_hours, departure_minutes, arrival_hours, arrival_minutes,
        //                 flight_type_id, departure_destination_id, arrival_destination_id, crew_flight_id) 
        //             values (@Number, @AircraftId, @DurationHours, @DurationMinutes, @DepartureDate, @ArrivalDate,
        //                 @DepartureHours, @DepartureMinutes, @ArrivalHours, @ArrivalMinutes, @FlightTypeId,
        //                 @DepartureDestinationId, @ArrivalDestinationId, @CrewFlightId)")
        //         .Param("Number", flightDto.Number)
        //         .Param("AircraftId", flightDto.AircraftId)
        //         .Param("DurationHours", flightDto.DurationHours)
        //         .Param("DurationMinutes", flightDto.DurationMinutes)
        //         .Param("DepartureDate", flightDto.DepartureDate)
        //         .Param("ArrivalDate", flightDto.ArrivalDate)
        //         .Param("DepartureHours", flightDto.DepartureHours)
        //         .Param("DepartureMinutes", flightDto.DepartureMinutes)
        //         .Param("ArrivalHours", flightDto.ArrivalHours)
        //         .Param("ArrivalMinutes", flightDto.ArrivalMinutes)
        //         .Param("FlightTypeId", flightDto.FlightTypeId)
        //         .Param("DepartureDestinationId", flightDto.DepartureDestinationId)
        //         .Param("ArrivalDestinationId", flightDto.ArrivalDestinationId)
        //         .Param("CrewFlightId", flightDto.CrewFlightId)
        //         .OnError(ex => _logger.LogError(ex, "Error creating flight"))
        //         .Exec();
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
            CrewFlightId = flightDto.CrewFlightId
        };

        // await _command
        //     .Sql(@"update dbo.Flights set aircraft_id = @AircraftId, duration_hours = @DurationHours,
        //             duration_minutes = @DurationMinutes, departure_date = @DepartureDate, 
        //             arrival_date = @ArrivalDate, departure_hours = @DepartureHours, 
        //             departure_minutes = @DepartureMinutes, arrival_hours = @ArrivalHours, 
        //             arrival_minutes = @ArrivalMinutes, flight_type_id = @FlightTypeId,
        //             departure_destination_id = @DepartureDestinationId, 
        //             arrival_destination_id = @ArrivalDestinationId, crew_flight_id = @CrewFlightId
        //         where number = @Number")
        //     .Param("Number", flightDto.Number)
        //     .Param("AircraftId", flightDto.AircraftId)
        //     .Param("DurationHours", flightDto.DurationHours)
        //     .Param("DurationMinutes", flightDto.DurationMinutes)
        //     .Param("DepartureDate", flightDto.DepartureDate)
        //     .Param("ArrivalDate", flightDto.ArrivalDate)
        //     .Param("DepartureHours", flightDto.DepartureHours)
        //     .Param("DepartureMinutes", flightDto.DepartureMinutes)
        //     .Param("ArrivalHours", flightDto.ArrivalHours)
        //     .Param("ArrivalMinutes", flightDto.ArrivalMinutes)
        //     .Param("FlightTypeId", flightDto.FlightTypeId)
        //     .Param("DepartureDestinationId", flightDto.DepartureDestinationId)
        //     .Param("ArrivalDestinationId", flightDto.ArrivalDestinationId)
        //     .Param("CrewFlightId", flightDto.CrewFlightId)
        //     .OnError(ex => _logger.LogError(ex, "Error updating flight"))
        //     .Exec();
    }
}
