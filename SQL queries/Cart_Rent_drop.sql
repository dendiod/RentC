-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2018-04-26 02:00:00.8
use academy_net
-- foreign keys
ALTER TABLE Reservations DROP CONSTRAINT Reservations_Coupons;

ALTER TABLE Reservations DROP CONSTRAINT Reservations_ReservationStatuses;

ALTER TABLE RolesPermissions DROP CONSTRAINT RolesPermissions_Permissions;

ALTER TABLE RolesPermissions DROP CONSTRAINT ScreenPermissions_Roles;

ALTER TABLE Users DROP CONSTRAINT Users_Roles;

-- tables
DROP TABLE Coupons;

DROP TABLE Permissions;

DROP TABLE ReservationStatuses;

DROP TABLE Reservations;

DROP TABLE Roles;

DROP TABLE RolesPermissions;

DROP TABLE Users;

DROP TABLE Customers;

DROP TABLE Cars;

DROP TABLE Locations;

DROP TABLE Models;

DROP TABLE Manufacturers;

-- End of file.