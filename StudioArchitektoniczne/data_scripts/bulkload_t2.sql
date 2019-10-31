USE StudioArchitektoniczne
GO

BULK INSERT dbo.Pracownicy FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\architects_t2.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo.Klienci FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\clients_t2.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Projekty architektoniczne" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\projects_t2.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Wykonane projekty" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\projects_done_t2.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Nadzory architektoniczne" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\project_overwatches_t2.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')