use AirTiquiciaBD

select * from Airlines
select * from Aircrafts
select * from Aircraft_Types
select * from Baggages_Weight
select * from Flight_Types
select * from Destinations
select * from Roles
select * from People
select * from Users
select * from Person_Types
select * from Passengers
select * from Passenger_Flights
select * from Flights

select number, aircraft_id as aircraftId, duration_hours as durationHours, duration_minutes as durationMinutes, departure_date as departureDate,
    arrival_date as arrivalDate, departure_hours departureHours, departure_minutes as departureMinutes, arrival_hours as arrivalHours, arrival_minutes as arrivalMinutes,
    flight_type_id as flightTypeId, departure_destination_id as departureDestinationId, arrival_destination_id as arrivalDestinationId, price
from dbo.Flights FOR JSON PATH
