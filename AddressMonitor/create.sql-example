IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'AddressMonitor')
BEGIN
DROP DATABASE AddressMonitor
END

CREATE DATABASE AddressMonitor

USE AddressMonitor

CREATE TABLE dbo.Users(
	UserID int NOT NULL IDENTITY(1,1)
	,Email varchar(100) NOT NULL
	CONSTRAINT PK_UserId PRIMARY KEY (UserID)
	)

CREATE TABLE dbo.Addresses(
	UserID int NOT NULL IDENTITY(1,1)
	,Network int NOT NULL
	,WalletAddress varchar(100) NOT NULL

	FOREIGN KEY (UserID) REFERENCES Users(UserID)
	)
