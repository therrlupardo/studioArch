USE StudioArchitektoniczne
GO

BULK INSERT dbo.Pracownicy FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\architects_t1.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo.Klienci FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\clients_t1.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Projekty architektoniczne" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\projects_t1.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Wykonane projekty" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\projects_done_t1.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')

BULK INSERT dbo."Nadzory architektoniczne" FROM 'E:\studia\studioArch\StudioArchitektoniczne\data\project_overwatches_t1.bulk' WITH (FIELDTERMINATOR='|', ROWTERMINATOR = '0x0a')