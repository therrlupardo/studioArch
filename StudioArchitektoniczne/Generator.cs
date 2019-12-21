using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.models.outer;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio
{
    public class Generator
    {
        private GeneratorParameters FirstPeriod, SecondPeriod;
        Random _rand;
        DateTime _currentDate = new DateTime(2010, 1, 1);
        private int _supervisions = 0;
        List<Architect> _listOfArchitects = new List<Architect>();
        List<Client> _listOfClients = new List<Client>();
        List<Project> _listOfProjects = new List<Project>();
        List<ProjectSupervision> _listOfSupervisors = new List<ProjectSupervision>();
        List<OuterProject> _listOfOuterProjects = new List<OuterProject>();
        List<OuterSubject> _listOfOuterSubjects = new List<OuterSubject>();
        List<ProjectDone> _listOfProjectsDone = new List<ProjectDone>();

        List<Project> _projectsOm = new List<Project>();
        List<Project> _projectsOu = new List<Project>();
        List<Project> _projectsOb = new List<Project>();

        List<Architect> _availableOma = new List<Architect>();
        List<Architect> _availableOua = new List<Architect>();
        List<Architect> _availableOba = new List<Architect>();

        Dictionary<int, int> _mutatedArchitectsIdMapper = new Dictionary<int, int>();
        private int _lastArchitect;
        List<string> _listOfUpdates = new List<string>();

        public void InitGenerator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            _rand = new Random(2137);
            FirstPeriod = firstPeriod;
            SecondPeriod = secondPeriod;
        }

        public Generator(GeneratorSize size)
        {
            switch (size)
            {
                case GeneratorSize.SMALL:
                    InitSmallGenerator();
                    break;
                case GeneratorSize.HUNDRED_THOUSAND:
                    InitHundredThousandGenerator();
                    break;
                case GeneratorSize.QUARTER_MILLION:
                    InitQuarterMillionGenerator();
                    break;
                case GeneratorSize.HALF_MILLION:
                    InitHalfMillionGenerator();
                    break;
                case GeneratorSize.MILLION:
                    InitMillionGenerator();
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public Generator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitSmallGenerator()
        {
            var firstPeriod = new GeneratorParameters(10, 100, 200, 10, 10, 10);
            var secondPeriod = new GeneratorParameters(10, 100, 200, 10, 10, 10);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitHundredThousandGenerator()
        {
            var firstPeriod = new GeneratorParameters(200, 30000, 10000, 2000, 2000, 200);
            var secondPeriod = new GeneratorParameters(100, 15000, 10000, 1000, 1000, 100);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitQuarterMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(400, 60000, 20000, 4000, 4000, 400);
            var secondPeriod = new GeneratorParameters(200, 30000, 10000, 2000, 2000, 200);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitHalfMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(1000, 150000, 50000, 10000, 10000, 1000);
            var secondPeriod = new GeneratorParameters(500, 75000, 25000, 5000, 5000, 500);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(2000, 300000, 100000, 20000, 20000, 2000);
            var secondPeriod = new GeneratorParameters(1000, 150000, 100000, 10000, 10000, 1000);
            InitGenerator(firstPeriod, secondPeriod);
        }

        public void GenerateData()
        {

            var dateRange = (DateTime.Today - _currentDate).Days / 10;
            GenerateSimpleDataT0();
            GenerateComplexData(dateRange, FirstPeriod.Projects, FirstPeriod.Supervisions, FirstPeriod.OuterProjects, true);
            AssignArchitectsToProjects();
            WriteToFiles("t1");

            MutateData();
            WriteUpdatesToFiles("t2");

            GenerateSimpleDataT1();
            GenerateComplexData(dateRange, SecondPeriod.Projects, SecondPeriod.Supervisions, SecondPeriod.OuterProjects, false);
            AssignArchitectsToProjects();
            WriteToFiles("t2");
        }
        public int CountGeneratedRecords()
        {
            return _listOfArchitects.Count + _listOfClients.Count + _listOfOuterProjects.Count + _listOfOuterSubjects.Count +
                _listOfSupervisors.Count + _listOfProjects.Count + _listOfProjectsDone.Count;
        }

        private void AssignArchitectsToProjects()
        {
            while (_listOfProjects.Find(p => p.Status != ProjectStatusEnum.UKONCZONY) != null)
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                switch (type)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        CreateConnection(_projectsOb, _availableOba);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        CreateConnection(_projectsOm, _availableOma);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        CreateConnection(_projectsOu, _availableOua);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (_rand.Next(30) >= 1) continue;
                _currentDate = _currentDate.AddDays(1);
                FreeArchitects();
            }
        }
        private void CreateConnection(List<Project> projects, List<Architect> architects)
        {
            if (!architects.Any() || !projects.Any()) return;
            var neededArchitects = _rand.Next(1, Math.Min(4, architects.Count));
            var project = projects[0];
            if (project.ClientOrderDate > _currentDate) return;

            var id = projects.FindIndex(p => p.Id == project.Id);
            projects[id].StartDate = _currentDate;
            projects[id].EndDate = _currentDate.AddDays(_rand.Next(10, 20));
            projects[id].Update(_supervisions);

            var outerProjects = _listOfOuterProjects.FindAll(op => op.ProjectId == project.Id);

            foreach (var pr in outerProjects)
            {
                pr.StartDate = _currentDate.AddDays(_rand.Next(0, 10));
                pr.EndDate = pr.StartDate.AddDays(_rand.Next(0, 10));
            }

            var pd = new ProjectDone();
            for (var i = 0; i < neededArchitects; i++)
            {
                pd.ArchitectId = architects[0].Id;
                architects.Remove(architects[0]);
                pd.ProjectId = project.Id;
                _listOfProjectsDone.Add(pd);
            }

            projects.RemoveAll(p => p.Id == project.Id);
            var supervision = _listOfSupervisors.Find(o => o.ProjectId == project.Id);
            if (supervision != null)
            {
                _supervisions++;
                supervision.StartDate = project.EndDate;
                supervision.EndDate = supervision.StartDate.AddDays(_rand.Next(5, 10));
                var projectsDone = _listOfProjectsDone.FindAll(pd => pd.ProjectId == project.Id);
                var availableSupervisors = new List<Architect>();

                projectsDone.ForEach(projectDone =>
                {
                    var architect = _listOfArchitects[projectDone.ArchitectId].Active
                        ? _listOfArchitects[projectDone.ArchitectId]
                        : _listOfArchitects[_mutatedArchitectsIdMapper.GetValueOrDefault(projectDone.ArchitectId)];
                    availableSupervisors.Add(architect);
                });
                if (availableSupervisors.Any() && availableSupervisors.Find(o => o.CanSupervise) != null)
                {
                    var arch = availableSupervisors.Find(o => o.CanSupervise);
                    if (arch != null) supervision.ArchitectId = arch.Id;
                }
                else
                {
                    var arch = architects.Find(a => a.CanSupervise);
                    if (arch != null) supervision.ArchitectId = arch.Id;
                }
            }
        }
        private void FreeArchitects()
        {
            var architects = new List<Architect>();
            var endingProjects = _listOfProjects.FindAll(p => p.StartDate >= p.ClientOrderDate &&
                p.EndDate <= _currentDate &&
                p.Status != ProjectStatusEnum.UKONCZONY
                );
            if (endingProjects.Any())
            {
                endingProjects.ForEach(p =>
                {
                    p.Status = ProjectStatusEnum.UKONCZONY;
                    switch (p.ArchitectureType)
                    {
                        case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                            _projectsOb.RemoveAll(project => project.Id == p.Id);
                            break;
                        case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                            _projectsOm.RemoveAll(project => project.Id == p.Id);
                            break;
                        case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                            _projectsOu.RemoveAll(project => project.Id == p.Id);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
                _listOfProjectsDone.FindAll(pd => endingProjects.Find(p => p.Id == pd.ProjectId) != null)
                    .ForEach(pd =>
                    {
                        var architect = _listOfArchitects[pd.ArchitectId].Active
                            ? _listOfArchitects[pd.ArchitectId]
                            : _listOfArchitects[_mutatedArchitectsIdMapper.GetValueOrDefault(pd.ArchitectId)];
                        architects.Add(architect);
                    });
                var supervision = _listOfSupervisors.Find(o => endingProjects.Find(p => p.Id == o.ProjectId) != null);
                if (supervision != null)
                {
                    var supervisor = architects.Find(a => a.Id == supervision.ArchitectId);
                    if (supervisor != null)
                    {
                        architects.Remove(supervisor);
                        _supervisions--;
                    }
                }
            }
            var endingSupervisions = _listOfSupervisors.FindAll(o => o.EndDate <= _currentDate && o.EndDate >= _currentDate.AddDays(-1));
            if (endingSupervisions.Any())
            {
                var supervisors = new List<Architect>();
                endingSupervisions.ForEach(o =>
                {
                    var architect = _listOfArchitects[o.ArchitectId].Active
                        ? _listOfArchitects[o.ArchitectId]
                        : _listOfArchitects[_mutatedArchitectsIdMapper.GetValueOrDefault(o.ArchitectId)];
                    supervisors.Add(architect);
                });
                architects = architects.Concat(supervisors).ToList();
            }
            architects.ForEach(a =>
            {
                switch (a.Specialization)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        _availableOba.Add(a);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        _availableOma.Add(a);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        _availableOua.Add(a);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
        private void GenerateSimpleDataT0()
        {
            for (var i = 0; i < FirstPeriod.Clients; i++) _listOfClients.Add(new Client(i));
            for (var i = 0; i < FirstPeriod.Architects; i++) _listOfArchitects.Add(new Architect(i, _currentDate, new DateTime(2999, 12, 31)));
            for (var i = 0; i < FirstPeriod.OuterProjects; i++) _listOfOuterSubjects.Add(new OuterSubject(i));
            ShuffleArchitects();
            GenerateArchitectsInitialHierarchy();
        }
        private void GenerateSimpleDataT1()
        {
            var numberOfArchitects = _listOfArchitects.Count;
            for (var i = FirstPeriod.Clients; i < FirstPeriod.Clients + SecondPeriod.Clients; i++) _listOfClients.Add(new Client(i));
            for (var i = numberOfArchitects; i < numberOfArchitects + SecondPeriod.Architects; i++) _listOfArchitects.Add(new Architect(i, _currentDate, new DateTime(2999, 12, 31)));
            for (var i = FirstPeriod.OuterSubjects; i < FirstPeriod.OuterSubjects + SecondPeriod.OuterSubjects; i++) _listOfOuterSubjects.Add(new OuterSubject(i));
            ShuffleArchitects();
            GenerateArchitectsAfterHierarchy();
        }
        private void GenerateComplexData(int dateRange, int numberOfProjects, int numberOfSupervisions, int numberOfOuterProjects, bool isFirstGeneration)
        {
            GenerateProjects(isFirstGeneration, numberOfProjects, dateRange);
            GenerateSupervisions(isFirstGeneration, numberOfSupervisions, numberOfProjects);
            GenerateOuterProjects(isFirstGeneration, numberOfOuterProjects);
        }
        private void GenerateProjects(bool isFirstGeneration, int count, int dateRange)
        {
            var shift = isFirstGeneration ? 0 : FirstPeriod.Projects;
            for (var i = 0; i < count; i++)
            {
                var clientOrderDate = _currentDate.AddDays(_rand.Next(dateRange));
                var client = _listOfClients[_rand.Next(_listOfClients.Count)];
                var project = new Project(
                    i + shift,
                    clientOrderDate,
                    client.Id);
                _listOfProjects.Add(project);
                switch (project.ArchitectureType)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        _projectsOb.Add(project);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        _projectsOm.Add(project);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        _projectsOu.Add(project);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _projectsOb = OrderListByStatusAndClientOrderDate(_projectsOb);
            _projectsOm = OrderListByStatusAndClientOrderDate(_projectsOm);
            _projectsOu = OrderListByStatusAndClientOrderDate(_projectsOu);
        }
        private void GenerateSupervisions(bool isFirstGeneration, int count, int numberOfProjects)
        {
            var delta = count != 0 ? numberOfProjects / count : numberOfProjects;
            var index = isFirstGeneration ? 0 : FirstPeriod.Projects;
            for (var i = isFirstGeneration ? 0 : FirstPeriod.Supervisions; i < count; i++)
            {
                var manager = _listOfOuterSubjects[_rand.Next(_listOfOuterSubjects.Count)];
                var project = _listOfProjects[index];
                index += delta;
                var supervision = new ProjectSupervision(i, manager.Id, 0, project.Id);
                project.TotalPrize = project.Prize + supervision.Prize;
                project.IsSupervised = true;
                supervision.Update();
                _listOfSupervisors.Add(supervision);
            }
        }
        private void GenerateOuterProjects(bool isFirstGeneration, int count)
        {
            var shift = isFirstGeneration ? 0 : FirstPeriod.OuterProjects;
            for (var i = 0; i < count; i++)
            {
                var project = _listOfProjects[_rand.Next(_listOfProjects.Count)];
                var os = _listOfOuterSubjects[_rand.Next(_listOfOuterSubjects.Count)];
                _listOfOuterProjects.Add(new OuterProject(i, os.Id, project.Id));
            }
        }
        private static List<Project> OrderListByStatusAndClientOrderDate(IEnumerable<Project> list)
        {
            return list.OrderBy(project => project.Status).ThenBy(project => project.ClientOrderDate).ToList();
        }
        private void MutateData()
        {
            MutateArchitectsSpecializations();
            MutateArchitectsCanSupervise();
        }
        private void MutateArchitectsSpecializations()
        {
            var toMutate = _rand.Next(1, 4);
            var architectsToMutate = ChooseArchitectsToMutate(toMutate);
            var mutatedArchitects = new List<Architect>();
            architectsToMutate.ForEach(architect =>
            {
                var mutatedArchitect = architect.Copy();
                architect.Active = false;
                architect.ExpirationDate = _currentDate;
                mutatedArchitect.InsertDate = _currentDate;
                mutatedArchitect.Specialization = mutatedArchitect.Specialization switch
                {
                    ArchitectureTypeEnum.OBIEKT_BIUROWY => (_rand.Next(2) == 0
                        ? ArchitectureTypeEnum.OBIEKT_MIESZKALNY
                        : ArchitectureTypeEnum.OBIEKT_USLUGOWY),
                    ArchitectureTypeEnum.OBIEKT_MIESZKALNY => (_rand.Next(2) == 0
                        ? ArchitectureTypeEnum.OBIEKT_BIUROWY
                        : ArchitectureTypeEnum.OBIEKT_USLUGOWY),
                    ArchitectureTypeEnum.OBIEKT_USLUGOWY => (_rand.Next(2) == 0
                        ? ArchitectureTypeEnum.OBIEKT_MIESZKALNY
                        : ArchitectureTypeEnum.OBIEKT_BIUROWY),
                    _ => throw new ArgumentOutOfRangeException()
                };

                mutatedArchitect.Id = _listOfArchitects.Count();
                _listOfArchitects.Add(mutatedArchitect);
                mutatedArchitects.Add(mutatedArchitect);
                _mutatedArchitectsIdMapper.Add(architect.Id, mutatedArchitect.Id);
            });
            CreateSpecializationUpdates(mutatedArchitects);
            AddArchitectsToSpecializedGroups(mutatedArchitects);
        }
        private void MutateArchitectsCanSupervise()
        {
            var toMutate = _rand.Next(1, 4);
            var architectsToMutate = ChooseArchitectsToMutate(toMutate);
            architectsToMutate.ForEach(architect =>
            {
                var mutatedArchitect = architect.Copy();
                architect.Active = false;
                architect.ExpirationDate = _currentDate;
                mutatedArchitect.CanSupervise = !mutatedArchitect.CanSupervise;
                mutatedArchitect.InsertDate = _currentDate;
                mutatedArchitect.Id = _listOfArchitects.Count();
                _listOfArchitects.Add(mutatedArchitect);
                _mutatedArchitectsIdMapper.Add(architect.Id, mutatedArchitect.Id);
            });
        }
        private List<Architect> ChooseArchitectsToMutate(int toMutate)
        {
            var architectsToMutate = new List<Architect>();
            for (var i = 0; i < toMutate; i++)
            {
                switch (RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>())
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        if (_availableOba.Any())
                        {
                            architectsToMutate.Add(_availableOba[0]);
                            _availableOba.RemoveAt(0);
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        if (_availableOma.Any())
                        {
                            architectsToMutate.Add(_availableOma[0]);
                            _availableOma.RemoveAt(0);
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        if (_availableOua.Any())
                        {
                            architectsToMutate.Add(_availableOua[0]);
                            _availableOua.RemoveAt(0);
                        }
                        break;
                }
            }
            return architectsToMutate;
        }
        private void AddArchitectsToSpecializedGroups(List<Architect> architects)
        {
            architects.ForEach(architect =>
            {
                switch (architect.Specialization)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        _availableOba.Add(architect);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        _availableOma.Add(architect);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        _availableOua.Add(architect);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
        private void ShuffleArchitects()
        {
            _availableOba = _listOfArchitects.FindAll(a => a.Specialization == ArchitectureTypeEnum.OBIEKT_BIUROWY);
            _availableOma = _listOfArchitects.FindAll(a => a.Specialization == ArchitectureTypeEnum.OBIEKT_MIESZKALNY);
            _availableOua = _listOfArchitects.FindAll(a => a.Specialization == ArchitectureTypeEnum.OBIEKT_USLUGOWY);
            _availableOba = Shuffle(_availableOba);
            _availableOma = Shuffle(_availableOma);
            _availableOua = Shuffle(_availableOua);
        }

        private void GenerateArchitectsInitialHierarchy()
        {
            var ordered = _listOfArchitects.OrderByDescending(a => a.Pesel).ToList();
            ordered.ForEach(a => a.PrincipalId = (int)Math.Floor((double)(a.Id - 1) / 10));
            _lastArchitect = ordered.Last().Id;
        }

        private void GenerateArchitectsAfterHierarchy()
        {
            var ordered = _listOfArchitects.FindAll(a => a.PrincipalId == -2).OrderByDescending(a => a.Pesel).ToList();
            ordered.ForEach(a => a.PrincipalId = _lastArchitect + (int)Math.Floor((double)(a.Id - 1) / 10));
        }

        private void GenerateArchitectsAfterHierarchy(List<Architect> list, int groupSupervisor)
        {
            var ordered = list.FindAll(a => a.PrincipalId == -2).OrderByDescending(a => a.Pesel).ToList();
            ordered.ForEach(a => a.PrincipalId = groupSupervisor + (int)Math.Floor((double)(a.Id - 1) / 10));
        }
        private static List<T> Shuffle<T>(List<T> list)
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

        private void WriteToFiles(string time)
        {
            const string path = "../../../data/";

            CreateCsvHeaders(path, time);

            var tmp = new List<DataModel>();
            tmp = _listOfArchitects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "architects_" + time + ".csv");
            tmp.Clear();

            tmp = _listOfOuterProjects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "outer_projects_" + time + ".csv");
            tmp.Clear();

            tmp = _listOfOuterSubjects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "outer_subjects_" + time + ".csv");
            tmp.Clear();

            tmp = _listOfArchitects.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "architects_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfClients.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "clients_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfProjects.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "projects_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfProjectsDone.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "projects_done_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfSupervisors.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "project_supervisions_" + time + ".bulk");
            tmp.Clear();
        }

        private static void CreateCsvHeaders(string path, string time)
        {
            File.WriteAllText(GetFileName(path, "outer_subjects", time, "csv"), Resources.Generator_OuterSubjects_Csv_Header, Encoding.UTF8);
            File.WriteAllText(GetFileName(path, "outer_projects", time, "csv"), Resources.Generator_OuterProjects_Csv_Header, Encoding.UTF8);
            File.WriteAllText(GetFileName(path, "architects", time, "csv"), Resources.Generator_Architects_Csv_Header, Encoding.UTF8);
        }

        private static string GetFileName(string path, string fileName, string time, string extension)
        {
            return $"{path}{fileName}_{time}.{extension}";
        }


        private static void WriteToCsv(List<DataModel> list, string filename)
        {
            var text = new StringBuilder();
            list.ForEach(element => text.AppendLine(element.ToCsv()));
            File.AppendAllText(filename, text.ToString(), Encoding.UTF8);
        }

        private static void WriteToBulk(List<DataModel> list, string filename)
        {
            var text = new StringBuilder();
            list.ForEach(element => text.AppendLine(element.ToBulk()));
            File.AppendAllText(filename, text.ToString(), Encoding.UTF8);
        }

        private void CreateSpecializationUpdates(List<Architect> architects)
        {
            architects.ForEach(architect =>
            {
                var id = _listOfArchitects.FindIndex(a => a.Active == false && a.Pesel == architect.Pesel);
                _listOfUpdates.Add($"UPDATE Pracownicy SET Specjalizacja='{architect.Specialization.ToString()}' WHERE ID_PRACOWNIK={id}");
            });
        }

        private void WriteUpdatesToFiles(string time)
        {
            const string path = "../../../data/";

            var text = new StringBuilder();
            _listOfUpdates.ForEach(update => text.AppendLine(update));
            File.AppendAllText(GetFileName(path, "architects_update", time, "sql"), text.ToString(), Encoding.UTF8);
        }
    }
}