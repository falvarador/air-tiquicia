use AirTiquiciaBD

-- Aircraft_Types
insert into dbo.Aircraft_Types (type_id, description, rows, seats) 
values ('87501', 'Pequeño', '20', '4')
insert into dbo.Aircraft_Types (type_id, description, rows, seats) 
values ('98809', 'Mediano', '38', '6')
insert into dbo.Aircraft_Types (type_id, description, rows, seats) 
values ('70101', 'Grande', '34', '9')

-- Aircrafts
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('87645', 'Boeing 737-800', '70101')
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('9017', 'Airbus A380', '70101')
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('6308', 'Airbus A330-300', '98809')
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('70051', 'Boeing 777-300ER', '98809')
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('32012', 'Dash 8', '87501')
insert into dbo.Aircrafts (aircraft_id, description, type_id)
values ('82748', 'Saab 2000', '87501')

-- Baggages_Weight
insert into dbo.Baggages_Weight (weight, price)
values ('10', '35')
insert into dbo.Baggages_Weight (weight, price)
values ('23', '48')
insert into dbo.Baggages_Weight (weight, price)
values ('32', '67')

-- Flight_Types
insert into dbo.Flight_Types (flight_type_id, [description])
values ('10429', 'Vuelo directo')
insert into dbo.Flight_Types (flight_type_id, [description])
values ('84901', 'Con escalas')

-- Destinations
insert into dbo.Destinations (destination_id, [name], [location])
values ('43867', 'Aeropuerto Internacional Juan Santamaría', 'Alajuela, Costa Rica')
insert into dbo.Destinations (destination_id, [name], [location])
values ('51321', 'Aeropuerto Benito Juarez', 'CDMX, México')
insert into dbo.Destinations (destination_id, [name], [location])
values ('90486', 'Aeropuerto Internacional Daniel Oduber', 'Liberia, Costa Rica')
insert into dbo.Destinations (destination_id, [name], [location])
values ('43867', 'Aeropuerto Internacional Charles de Gaulle', 'París, Francia')
 
-- Roles
insert into dbo.Roles (role_id, [name])
values ('96445', 'Administrador')

-- People
insert into dbo.People (person_id, [name], last_name, telephone, direction, email)
values ('69395', 'Julián Alberto', 'Gómez Castro', '(506) 8765 3489', '1959 NE 153 ST, GL (Suite) - San Ramón Norte', 'jgomez@email.com')
insert into dbo.People (person_id, [name], last_name, telephone, direction, email)
values ('18626', 'Marisol', 'Campos Granados', '(506) 7563 2390', 'De la antiguo casa Matute Gómez, 300 este y 75 norte frente a la Iglesia Sagrado Corazón', 'mcampos@email.com')

-- Users
insert into dbo.Users (user_id. person_pk_id, person_id, role_pk_id, role_id, username, [password])
values ('75493', 1, '69395', 1, '96445', 'jgomez', 'azsxdcfvgb')
insert into dbo.Users (user_id. person_pk_id, person_id, role_pk_id, role_id, username, [password])
values ('99266', 2, '18626', 1, '96445', 'mcampos', 'qawsedrftg')

-- Person_Types
insert into dbo.Person_Types (person_type_id, person_pk_id, [person_id], [type])
values ('56421', 1, '69395', 'Empleado')
