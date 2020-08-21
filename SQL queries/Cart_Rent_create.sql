-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2018-04-26 02:00:00.8
USE academy_net
-- tables
-- Table: Models
CREATE TABLE Models (
    Id INT IDENTITY PRIMARY KEY,
	Name varchar(50)  NOT NULL
);

-- Table: Manufacturers
CREATE TABLE Manufacturers (
    Id INT IDENTITY PRIMARY KEY,
	Name varchar(30)  NOT NULL
);

-- Table: Locations
CREATE TABLE Locations (
    Id INT IDENTITY PRIMARY KEY,
	Name varchar(50)  NOT NULL
);

-- Table: Cars
CREATE TABLE Cars (
    Id int  IDENTITY PRIMARY KEY,
    Plate varchar(10)  NOT NULL,
    ManufacturerId INT NOT NULL REFERENCES Manufacturers(Id) ON DELETE CASCADE,
    ModelId INT NOT NULL REFERENCES Models(Id) ON DELETE CASCADE,
    PricePerDay money  NOT NULL,
	LocationId INT NOT NULL REFERENCES Locations(Id) ON DELETE CASCADE
);

-- Table: Customers
CREATE TABLE Customers (
    Id int  IDENTITY PRIMARY KEY,
	CustomId int NOT NULL,
    Name varchar(50)  NOT NULL,
    BirthDate date  NOT NULL,
    LocationId INT REFERENCES Locations(Id) ON DELETE CASCADE
);

-- Table: Reservations
CREATE TABLE Reservations (
	Id int identity PRIMARY KEY,
    CarId INT NOT NULL REFERENCES Cars(Id),
    CustomerId int  NOT NULL,
    StartDate date  NOT NULL,
    EndDate date  NOT NULL,
    LocationId INT NOT NULL REFERENCES Locations(Id) ON DELETE CASCADE
);

-- End of file.