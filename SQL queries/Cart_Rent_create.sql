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

-- Table: Coupons
CREATE TABLE Coupons (
    CouponCode varchar(10)  NOT NULL,
    Description ntext  NOT NULL,
    Discount decimal(4,2)  NOT NULL,
    CONSTRAINT Coupons_pk PRIMARY KEY  (CouponCode)
);

-- Table: Customers
CREATE TABLE Customers (
    Id int  IDENTITY PRIMARY KEY,
    Name varchar(50)  NOT NULL,
    BirthDate date  NOT NULL,
    LocationId INT REFERENCES Locations(Id) ON DELETE CASCADE
);

-- Table: Permissions
CREATE TABLE Permissions (
    PermissionID int  NOT NULL IDENTITY(1, 1),
    Name varchar(10)  NOT NULL,
    Description varchar(100)  NOT NULL,
    CONSTRAINT Permissions_pk PRIMARY KEY  (PermissionID)
);

-- Table: ReservationStatuses
CREATE TABLE ReservationStatuses (
    ReservStatsID tinyint  NOT NULL IDENTITY(1, 1),
    Name varchar(20)  NOT NULL,
    Description varchar(100)  NOT NULL,
    CONSTRAINT ReservationStatuses_pk PRIMARY KEY  (ReservStatsID)
);

-- Table: Reservations
CREATE TABLE Reservations (
	Id int identity PRIMARY KEY,
    CarId INT NOT NULL REFERENCES Cars(Id),
    CustomerId int  NOT NULL REFERENCES Customers(Id),
    ReservStatsID tinyint  NOT NULL,
    StartDate date  NOT NULL,
    EndDate date  NOT NULL,
    LocationId INT NOT NULL REFERENCES Locations(Id) ON DELETE CASCADE,
    CouponCode varchar(10)  NULL
);

-- Table: Roles
CREATE TABLE Roles (
    RoleID int  NOT NULL IDENTITY(1, 1),
    Name varchar(50)  NOT NULL,
    Description varchar(200)  NOT NULL,
    CONSTRAINT Roles_pk PRIMARY KEY  (RoleID)
);

-- Table: RolesPermissions
CREATE TABLE RolesPermissions (
    RoleID int  NOT NULL,
    PermissionID int  NOT NULL,
    CONSTRAINT RolesPermissions_pk PRIMARY KEY  (RoleID,PermissionID)
);

-- Table: Users
CREATE TABLE Users (
    UserID int  NOT NULL IDENTITY(1, 1),
    Password varchar(100)  NOT NULL,
    Enabled bit  NOT NULL,
    RoleID int  NOT NULL,
    CONSTRAINT Users_pk PRIMARY KEY  (UserID)
);

-- foreign keys
-- Reference: Reservations_Coupons (table: Reservations)
ALTER TABLE Reservations ADD CONSTRAINT Reservations_Coupons
    FOREIGN KEY (CouponCode)
    REFERENCES Coupons (CouponCode);

-- Reference: Reservations_ReservationStatuses (table: Reservations)
ALTER TABLE Reservations ADD CONSTRAINT Reservations_ReservationStatuses
    FOREIGN KEY (ReservStatsID)
    REFERENCES ReservationStatuses (ReservStatsID);

-- Reference: RolesPermissions_Permissions (table: RolesPermissions)
ALTER TABLE RolesPermissions ADD CONSTRAINT RolesPermissions_Permissions
    FOREIGN KEY (PermissionID)
    REFERENCES Permissions (PermissionID);

-- Reference: ScreenPermissions_Roles (table: RolesPermissions)
ALTER TABLE RolesPermissions ADD CONSTRAINT ScreenPermissions_Roles
    FOREIGN KEY (RoleID)
    REFERENCES Roles (RoleID);

-- Reference: Users_Roles (table: Users)
ALTER TABLE Users ADD CONSTRAINT Users_Roles
    FOREIGN KEY (RoleID)
    REFERENCES Roles (RoleID);

-- End of file.

