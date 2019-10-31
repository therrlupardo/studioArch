CREATE DATABASE StudioArchitektoniczne
GO

USE StudioArchitektoniczne
GO

SET DATEFORMAT dmy;  
GO  

CREATE TABLE Pracownicy
(
    ID INTEGER PRIMARY KEY NOT NULL CHECK (ID >= 0),
    Specjalizacja NVARCHAR(60) NOT NULL CHECK (Specjalizacja in ('OBIEKT_MIESZKALNY', 'OBIEKT_USLUGOWY', 'OBIEKT_BIUROWY')),
)
GO

CREATE TABLE Klienci
(
    ID INTEGER PRIMARY KEY NOT NULL CHECK (ID >= 0),
    Imie NVARCHAR(30), 
    Nazwisko NVARCHAR(60),
    Nazwa NVARCHAR(30),
    "Numer kontaktowy" NCHAR(9) NOT NULL,
    Email NVARCHAR(100) NOT NULL CHECK (Email LIKE '%_@__%.__%'),
)
GO

CREATE TABLE "Projekty architektoniczne"
(
    ID INTEGER PRIMARY KEY NOT NULL CHECK (ID >= 0),
    Adres NVARCHAR(100) NOT NULL,
    "Typ architektury" NVARCHAR(20) NOT NULL CHECK ("Typ architektury" in ('OBIEKT_MIESZKALNY', 'OBIEKT_USLUGOWY', 'OBIEKT_BIUROWY')),
    Cena INTEGER NOT NULL CHECK (Cena >= 0),
    "Data przyjęcia do realizacji" DATE NOT NULL,
    "Termin rozpoczęcia" DATE NULL,
    "Termin zakonczenia" DATE NULL,
    Status NVARCHAR(30) NOT NULL CHECK (Status in ('PRZYJETO_DO_REALIZACJI', 'W_TRAKCIE_PRAC', 'UKONCZONY')),
    "Wielkosc projektu" SMALLINT NOT NULL CHECK ("Wielkosc projektu" > 0),
    "ID klienta" INTEGER FOREIGN KEY REFERENCES Klienci NOT NULL,
)
GO

CREATE TABLE "Wykonane projekty"
(
    "ID pracownika" INTEGER NOT NULL FOREIGN KEY REFERENCES Pracownicy,
    "ID projektu" INTEGER NOT NULL FOREIGN KEY REFERENCES "Projekty architektoniczne",
    PRIMARY KEY ("ID pracownika", "ID projektu"),
)
GO

CREATE TABLE "Nadzory architektoniczne"
(
    ID INTEGER PRIMARY KEY NOT NULL CHECK (ID >= 0),
    "Termin rozpoczęcia" DATE NOT NULL,
    "Termin zakonczenia" DATE NULL,
    Koszt INTEGER NOT NULL CHECK (Koszt >= 0),
    "ID kierownika budowy" INTEGER NOT NULL,
    "ID pracownika" INTEGER NOT NULL FOREIGN KEY REFERENCES Pracownicy,
    "ID projektu" INTEGER NOT NULL FOREIGN KEY REFERENCES "Projekty architektoniczne",
)
GO
