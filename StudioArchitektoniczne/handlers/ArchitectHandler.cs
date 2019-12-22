using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;
using static ArchitecturalStudio.models.enums.ArchitectureTypeEnum;

namespace ArchitecturalStudio.handlers
{
    public class ArchitectHandler : AbstractHandler, IGenerator, IWritable
    {
        public List<Architect> Architects { get; set; }
        public Dictionary<ArchitectureTypeEnum, List<Architect>> AvailableArchitects { get; set; }
        private readonly Dictionary<int, int> _idMapper = new Dictionary<int, int>();
        private int _lastArchitectId;
        private readonly Random _rand;
        private List<string> _updates = new List<string>();

        public ArchitectHandler()
        {
            _lastArchitectId = 0;
            _rand = new Random(int.Parse(Resources.Global_Random_Seed));
            Architects = new List<Architect>();
            AvailableArchitects = new Dictionary<ArchitectureTypeEnum, List<Architect>>
            {
                {OBIEKT_MIESZKALNY, new List<Architect>()},
                {OBIEKT_BIUROWY, new List<Architect>()},
                {OBIEKT_USLUGOWY, new List<Architect>()}
            };
        }
        public void Generate(int amount, params object[] parameters)
        {
            if (!DateTime.TryParse(parameters[0].ToString(), out var currentDate))
            {
                throw new ArgumentException();
            }
            GenerateArchitects(amount, currentDate);
            ShuffleArchitects();
            CreateHierarchy();
        }
        private void GenerateArchitects(int amount, DateTime currentDate)
        {
            var baseIndex = Architects.Any() ? Architects.Count : 0;
            for (var i = 0; i < amount; i++)
            {
                Architects.Add(new Architect(baseIndex + i, currentDate, new DateTime(2999, 12, 31)));
            }
        }
        private void ShuffleArchitects()
        {
            ShuffleArchitects(OBIEKT_BIUROWY);
            ShuffleArchitects(OBIEKT_MIESZKALNY);
            ShuffleArchitects(OBIEKT_USLUGOWY);
        }
        private void ShuffleArchitects(ArchitectureTypeEnum type)
        {
            AvailableArchitects[type] = Architects.FindAll(a => a.Specialization == type);
            AvailableArchitects[type] = ShuffleList(AvailableArchitects[type]);
            var tmp = AvailableArchitects[type].ToList().GroupBy(a => a.Id).ToList();
        }
        private static List<T> ShuffleList<T>(List<T> list)
        {
            var rand = new Random(2137);
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        private void CreateHierarchy()
        {
            var architectsWithoutPrincipal =
                Architects.FindAll(a => a.PrincipalId == -2).OrderByDescending(a => a.Pesel).ToList();
            architectsWithoutPrincipal.ForEach(CalculatePrincipalId);
            _lastArchitectId = architectsWithoutPrincipal.Last().Id;
        }
        private void CalculatePrincipalId(Architect architect)
        {
            architect.PrincipalId = _lastArchitectId + (int)Math.Floor((double)(architect.Id - 1) / 10);
        }
        public void Write(string time)
        {
            File.AppendAllLines(
                $"{Resources.Global_Data_Path}architects_{time}.csv",
                new List<string>() { Resources.Generator_Architects_Csv_Header },
                Encoding.UTF8);
            var dataModels = Architects.Cast<AbstractDataModel>().ToList();
            WriteToCsv(dataModels, $"{Resources.Global_Data_Path}architects_{time}.csv");
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}architects_{time}.bulk");
        }

        public void Update(DateTime currentDate)
        {
            UpdateSpecialization(currentDate);
            UpdateCanSupervise(currentDate);
        }

        private void UpdateCanSupervise(DateTime currentDate)
        {
            var amount = _rand.Next(1, 4);
            var architectsToUpdate = GetArchitectsToUpdate(amount);
            architectsToUpdate.ForEach(architect =>
            {
                var updatedArchitect = architect.Copy();
                updatedArchitect.CanSupervise = !updatedArchitect.CanSupervise;
                updatedArchitect.InsertDate = currentDate;
                updatedArchitect.Id = Architects.Count();
                Architects.Add(updatedArchitect);
                _idMapper.Add(architect.Id, updatedArchitect.Id);
                DeactivateArchitect(currentDate, architect);
            });
        }

        private void UpdateSpecialization(DateTime currentDate)
        {
            var amount = _rand.Next(1, 4);
            var architectsToUpdate = GetArchitectsToUpdate(amount);
            var updatedArchitects = new List<Architect>();
            architectsToUpdate.ForEach(architect => UpdateSpecializationOfArchitect(currentDate, architect, updatedArchitects));
            CreateUpdateFile(updatedArchitects);
            AddArchitectsToSpecializedGroups(updatedArchitects);
        }

        private void UpdateSpecializationOfArchitect(DateTime currentDate, Architect architect, List<Architect> updatedArchitects)
        {
            var updatedArchitect = architect.Copy();
            updatedArchitect.InsertDate = currentDate;
            updatedArchitect.Specialization = updatedArchitect.Specialization switch
            {
                OBIEKT_BIUROWY => _rand.Next() % 2 == 0 ? OBIEKT_MIESZKALNY : OBIEKT_USLUGOWY,
                OBIEKT_MIESZKALNY => _rand.Next() % 2 == 0 ? OBIEKT_BIUROWY : OBIEKT_USLUGOWY,
                OBIEKT_USLUGOWY => _rand.Next() % 2 == 0 ? OBIEKT_MIESZKALNY : OBIEKT_BIUROWY,
                _ => throw new ArgumentOutOfRangeException()
            };

            updatedArchitect.Id = Architects.Count();
            Architects.Add(updatedArchitect);
            updatedArchitects.Add(updatedArchitect);
            architect.Active = false;
            architect.ExpirationDate = currentDate;
            _idMapper.Add(architect.Id, updatedArchitect.Id);
        }

        private static void DeactivateArchitect(DateTime currentDate, Architect architect)
        {
            architect.Active = false;
            architect.ExpirationDate = currentDate;
        }

        private List<Architect> GetArchitectsToUpdate(int amount)
        {
            var architectsToUpdate = new List<Architect>();
            for (var i = 0; i < amount; i++)
            {
                var architectureType = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                if (!AvailableArchitects[architectureType].Any()) continue;
                architectsToUpdate.Add(AvailableArchitects[architectureType].First(a => a.Active));
                AvailableArchitects[architectureType].RemoveAt(0);
            }
            return architectsToUpdate;
        }

        private void CreateUpdateFile(List<Architect> architects)
        {
            architects.ForEach(architect =>
            {
                var id = Architects.FindIndex(a => a.Active == false && a.Pesel == architect.Pesel);
                _updates.Add($"UPDATE Pracownicy SET Specjalizacja='{architect.Specialization.ToString()}' WHERE ID_PRACOWNIK={id}");
            });
            WriteUpdates();
        }

        private void AddArchitectsToSpecializedGroups(List<Architect> architects)
        {
            architects.ForEach(architect => AvailableArchitects[architect.Specialization].Add(architect));
        }
        private void WriteUpdates()
        {
            var text = new StringBuilder();
            _updates.ForEach(update => text.AppendLine(update));
            File.AppendAllText($"{Resources.Global_Data_Path}architects_update.sql", text.ToString(), Encoding.UTF8);
        }

    }
}