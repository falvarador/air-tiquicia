USE AirTiquiciaBD
GO 

-- Se crea la tabla que almacena la información de una aerolínea.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Airlines]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Airlines]
GO

CREATE TABLE dbo.Airlines
(
    id INT IDENTITY(1, 1) NOT NULL,
    airline_id VARCHAR(25) NOT NULL,
    name VARCHAR(50) NOT NULL,
    location VARCHAR(250) NOT NULL,
)
ALTER TABLE dbo.Airlines ADD CONSTRAINT [PK_Airline] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de una persona.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[People]') 
    AND type in (N'U'))
DROP TABLE [dbo].[People]
GO

CREATE TABLE dbo.People
(
    id INT IDENTITY(1, 1) NOT NULL,
    person_id VARCHAR(25) NOT NULL,
    name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    telephone VARCHAR(25) NULL,
    direction VARCHAR(250) NULL,
    email VARCHAR(100) NOT NULL
)
ALTER TABLE dbo.People ADD CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de un tipo de persona.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Person_Types]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Person_Types]
GO

CREATE TABLE dbo.Person_Types
(
    id INT IDENTITY(1, 1) NOT NULL,
    person_type_id INT NOT NULL,
    person_pk_id INT NOT NULL,
    person_id INT NOT NULL,
    type TINYINT NOT NULL
)
ALTER TABLE dbo.Person_Types ADD CONSTRAINT [PK_Person_Types] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Person_Types]  WITH CHECK ADD  CONSTRAINT [FK_Person_Types_People_Id] FOREIGN KEY([person_pk_id])
REFERENCES [dbo].[People] ([id])

-- Se crea la tabla que almacena la información de un rol de usuario.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Roles]
GO

CREATE TABLE dbo.Roles
(
    id INT IDENTITY(1, 1) NOT NULL,
    role_id VARCHAR(25) NOT NULL,
    name VARCHAR(50) NOT NULL
)
ALTER TABLE dbo.Roles ADD CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de un usuario.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Users]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO

CREATE TABLE dbo.Users
(
    id INT IDENTITY(1, 1) NOT NULL,
    user_id INT NOT NULL,
    person_pk_id INT NOT NULL,
    person_id INT NOT NULL,
    role_id INT NOT NULL,
    role_pk_id INT NOT NULL,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(250) NOT NULL,
)
ALTER TABLE dbo.Users ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_People_Id] FOREIGN KEY([person_pk_id])
REFERENCES [dbo].[People] ([person_id])
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Role_Id] FOREIGN KEY([role_pk_id])
REFERENCES [dbo].[Roles] ([role_id])

-- Se crea la tabla que almacena la información del peso del equipaje.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Baggages_Weight]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Baggages_Weight]
GO

CREATE TABLE dbo.Baggages_Weight
(
    id INT IDENTITY(1, 1) NOT NULL,
    weight TINYINT NOT NULL,
    price DECIMAL NOT NULL
)
ALTER TABLE dbo.Baggages_Weight ADD CONSTRAINT [PK_Baggage_Weight] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información del equipaje de un pasajero.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Baggages_Passenger]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Baggages_Passenger]
GO

CREATE TABLE dbo.Baggages_Passenger
(
    id INT IDENTITY(1, 1) NOT NULL,
    passenger_id INT NOT NULL,
    baggage_weight_id INT NOT NULL
)
ALTER TABLE dbo.Baggages_Passenger ADD CONSTRAINT [PK_Baggage_Passenger] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Baggages_Passenger]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_Baggage_Passenger_Id] FOREIGN KEY([passenger_id])
REFERENCES [dbo].[Passengers] ([id])
ALTER TABLE [dbo].[Baggages_Passenger]  WITH CHECK ADD  CONSTRAINT [FK_Baggages_Passenger_Baggages_Weight_Id] FOREIGN KEY([baggage_weight_id])
REFERENCES [dbo].[Baggages_Weight] ([id])

-- Se crea la tabla que almacena la información de un pasajero.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Passengers]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Passengers]
GO

CREATE TABLE dbo.Passengers
(
    id INT IDENTITY(1, 1) NOT NULL,
    passenger_id INT NOT NULL,
    person_pk_id INT NOT NULL,
    person_id INT NOT NULL,
    type varchar(50) NOT NULL,
    quantity_baggage TINYINT NOT NULL
)
ALTER TABLE dbo.Passengers ADD CONSTRAINT [PK_Passenger] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Passengers]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_People_Id] FOREIGN KEY([person_pk_id])
REFERENCES [dbo].[People] ([id])

-- Se crea la tabla que almacena la información del equipaje de un pasajero.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Baggages_Passenger]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Baggages_Passenger]
GO

CREATE TABLE dbo.Baggages_Passenger
(
    id INT IDENTITY(1, 1) NOT NULL,
    passenger_id INT NOT NULL,
    baggage_weight_id INT NOT NULL
)
ALTER TABLE dbo.Baggages_Passenger ADD CONSTRAINT [PK_Baggage_Passenger] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Baggages_Passenger]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_Baggage_Weight_Id] FOREIGN KEY([passenger_id])
REFERENCES [dbo].[Passenger] ([id])
ALTER TABLE [dbo].[Baggages_Passenger]  WITH CHECK ADD  CONSTRAINT [FK_Baggage_Passenger_Baggage_Weight_Id] FOREIGN KEY([baggage_weight_id])
REFERENCES [dbo].[Baggage_Weight] ([id])

-- Se crea la tabla que almacena la información de un tipo de avión.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Types]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Aircraft_Types]
GO

CREATE TABLE dbo.Aircraft_Types
(
    id INT IDENTITY(1, 1) NOT NULL,
    type_id VARCHAR(25) NOT NULL,
    description VARCHAR(50) NOT NULL,
    rows TINYINT NOT NULL,
    seats TINYINT NOT NULL,
)
ALTER TABLE dbo.Aircraft_Types ADD CONSTRAINT [PK_Aircraft_Type] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de un avión.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Aircrafts]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Aircrafts]
GO

CREATE TABLE dbo.Aircrafts
(
    id INT IDENTITY(1, 1) NOT NULL,
    aircraft_id VARCHAR(50) NOT NULL,
    description VARCHAR(250) NOT NULL,
    type_id VARCHAR(25) NOT NULL
)
ALTER TABLE dbo.Aircrafts ADD CONSTRAINT [PK_Aircrafts] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircraft_Aircraft_Type_Id] FOREIGN KEY([type_id])
REFERENCES [dbo].[Aircraft_Types] ([type_id])

-- Se crea la tabla que almacena la información de un destino.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Destinations]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Destinations]
GO

CREATE TABLE dbo.Destinations
(
    id INT IDENTITY(1, 1) NOT NULL,
    destination_id VARCHAR(25) NOT NULL,
    name VARCHAR(50) NOT NULL,
    location VARCHAR(50) NOT NULL
)
ALTER TABLE dbo.Destinations ADD CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de la tripulación.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Crews]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Crews]
GO

CREATE TABLE dbo.Crews
(
    id INT IDENTITY(1, 1) NOT NULL,
    crew_id VARCHAR(50) NOT NULL,
    description VARCHAR(250) NOT NULL
)
ALTER TABLE dbo.Crews ADD CONSTRAINT [PK_Crew] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de las personas de la tripulación.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[People_Crew]') 
    AND type in (N'U'))
DROP TABLE [dbo].[People_Crew]
GO

CREATE TABLE dbo.People_Crew
(
    id INT IDENTITY(1, 1) NOT NULL,
    crew_id INT NOT NULL,
    person_id INT NOT NULL,
)
ALTER TABLE dbo.People_Crew ADD CONSTRAINT [PK_People_Crew] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[People_Crew]  WITH CHECK ADD  CONSTRAINT [FK_People_Crew_Crew_Id] FOREIGN KEY([crew_id])
REFERENCES [dbo].[Crews] ([id])
ALTER TABLE [dbo].[People_Crew]  WITH CHECK ADD  CONSTRAINT [FK_People_Crew_Person_Id] FOREIGN KEY([person_id])
REFERENCES [dbo].[People] ([id])

-- Se crea la tabla que almacena la información de la tripulación de un vuelo.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Crew_Flights]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Crew_Flights]
GO

CREATE TABLE dbo.Crew_Flights
(
    id INT IDENTITY(1, 1) NOT NULL,
    crew_flight_id VARCHAR(50) NOT NULL,
    person_crew_id INT NOT NULL
)
ALTER TABLE dbo.Crew_Flights ADD CONSTRAINT [PK_Crew_Flight] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Crew_Flights]  WITH CHECK ADD  CONSTRAINT [FK_Crew_Flights_Person_Crew] FOREIGN KEY([person_crew_id])
REFERENCES [dbo].[People_Crew] ([id])

-- Se crea la tabla que almacena la información de un pasajero en un vuelo.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Passenger_Flights]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Passenger_Flights]
GO

CREATE TABLE dbo.Passenger_Flights
(
    id INT IDENTITY(1, 1) NOT NULL,
    flight_id INT NOT NULL,
    passenger_id INT NOT NULL
)
ALTER TABLE dbo.Passenger_Flights ADD CONSTRAINT [PK_Passenger_Flight] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Passenger_Flights]  WITH CHECK ADD  CONSTRAINT [FK_Passenger_Flights_Flights] FOREIGN KEY([flight_id])
REFERENCES [dbo].[Flights] ([id])

-- Se crea la tabla que almacena la información de un tipo de vuelo.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Flight_Types]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Flight_Types]
GO

CREATE TABLE dbo.Flight_Types
(
    id INT IDENTITY(1, 1) NOT NULL,
    flight_type_id INT NOT NULL,
    description VARCHAR(50) NOT NULL
)
ALTER TABLE dbo.Flight_Types ADD CONSTRAINT [PK_Flight_Type] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Se crea la tabla que almacena la información de un vuelo.
IF  EXISTS (SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[Flights]') 
    AND type in (N'U'))
DROP TABLE [dbo].[Flights]
GO

CREATE TABLE dbo.Flights
(
    id INT IDENTITY(1, 1) NOT NULL,
    number VARCHAR(50) NOT NULL,
    aircraft_id INT NOT NULL,
    duration_hours TINYINT NOT NULL,
    duration_minutes TINYINT NOT NULL,
    departure_date DATETIME2 NOT NULL,
    arrival_date DATETIME2 NOT NULL,
    departure_hours TINYINT NOT NULL,
    departure_minutes TINYINT NOT NULL,
    arrival_hours TINYINT NOT NULL,
    arrival_minutes TINYINT NOT NULL,
    flight_type_id INT NOT NULL,
    departure_destination_id INT NOT NULL,
    arrival_destination_id INT NOT NULL,
    price DECIMAL(18, 2) NOT NULL,
)
ALTER TABLE dbo.Flights ADD CONSTRAINT [PK_Flight] PRIMARY KEY CLUSTERED
(
   id ASC 
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Aircraft] FOREIGN KEY([aircraft_id])
REFERENCES [dbo].[Aircrafts] ([id])
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Flight_Type] FOREIGN KEY([flight_type_id])
REFERENCES [dbo].[Flight_Types] ([id])
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Departure_Destination] FOREIGN KEY([departure_destination_id])
REFERENCES [dbo].[Destinations] ([id])
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flight_Arrival_Destination] FOREIGN KEY([arrival_destination_id])
REFERENCES [dbo].[Destinations] ([id])
