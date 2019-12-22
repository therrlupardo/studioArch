using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchitecturalStudio.handlers;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.models.outer;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio
{
    public class Generator
    {
        private GeneratorParameters _firstPeriod, _secondPeriod;
        private Random _rand;
        private DateTime _currentDate = new DateTime(2010, 1, 1);
        private int _supervisions = 0;
        private readonly List<Project> _listOfProjects = new List<Project>();
        private readonly List<ProjectSupervision> _listOfSupervisors = new List<ProjectSupervision>();
        private readonly List<OuterProject> _listOfOuterProjects = new List<OuterProject>();
        private readonly List<ProjectDone> _listOfProjectsDone = new List<ProjectDone>();

        private List<Project> _projectsOm = new List<Project>();
        private List<Project> _projectsOu = new List<Project>();
        private List<Project> _projectsOb = new List<Project>();
        
        private readonly Dictionary<int, int> _mutatedArchitectsIdMapper = new Dictionary<int, int>();
        private readonly List<string> _listOfUpdates = new List<string>();

        private readonly ArchitectHandler _architectHandler = new ArchitectHandler();
        private readonly ClientHandler _clientHandler = new ClientHandler();
        private readonly OuterSubjectHandler _outerSubjectHandler = new OuterSubjectHandler();
        public void InitGenerator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            _rand = new Random(2137);
            _firstPeriod = firstPeriod;
            _secondPeriod = secondPeriod;
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
            GenerateComplexData(dateRange, _firstPeriod.Projects, _firstPeriod.Supervisions, _firstPeriod.OuterProjects, true);
            AssignArchitectsToProjects();
            WriteToFiles("t1");

            _architectHandler.Update(_currentDate);

            GenerateSimpleDataT1();
            GenerateComplexData(dateRange, _secondPeriod.Projects, _secondPeriod.Supervisions, _secondPeriod.OuterProjects, false);
            AssignArchitectsToProjects();
            WriteToFiles("t2");
        }

        private void AssignArchitectsToProjects()
        {
            while (_listOfProjects.Find(p => p.Status != ProjectStatusEnum.UKONCZONY) != null)
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                switch (type)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        CreateConnection(_projectsOb, _architectHandler.AvailableArchitects[ArchitectureTypeEnum.OBIEKT_BIUROWY]);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        CreateConnection(_projectsOm, _architectHandler.AvailableArchitects[ArchitectureTypeEnum.OBIEKT_MIESZKALNY]);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        CreateConnection(_projectsOu, _architectHandler.AvailableArchitects[ArchitectureTypeEnum.OBIEKT_USLUGOWY]);
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
                    var architect = _architectHandler.Architects[projectDone.ArchitectId].Active
                        ? _architectHandler.Architects[projectDone.ArchitectId]
                        : _architectHandler.Architects[_mutatedArchitectsIdMapper.GetValueOrDefault(projectDone.ArchitectId)];
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
                        var architect = _architectHandler.Architects[pd.ArchitectId].Active
                            ? _architectHandler.Architects[pd.ArchitectId]
                            : _architectHandler.Architects[_mutatedArchitectsIdMapper.GetValueOrDefault(pd.ArchitectId)];
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
                    var architect = _architectHandler.Architects[o.ArchitectId].Active
                        ? _architectHandler.Architects[o.ArchitectId]
                        : _architectHandler.Architects[_mutatedArchitectsIdMapper.GetValueOrDefault(o.ArchitectId)];
                    supervisors.Add(architect);
                });
                architects = architects.Concat(supervisors).ToList();
            }
            architects.ForEach(a => _architectHandler.AvailableArchitects[a.Specialization].Add(a));
        }
        private void GenerateSimpleDataT0()
        {
            _clientHandler.Generate(_firstPeriod.Clients, _currentDate);
            _architectHandler.Generate(_firstPeriod.Architects, _currentDate);
            _outerSubjectHandler.Generate(_firstPeriod.Clients, _currentDate);
        }
        private void GenerateSimpleDataT1()
        {
            _clientHandler.Generate(_secondPeriod.Clients, _currentDate);
            _architectHandler.Generate(_secondPeriod.Architects, _currentDate);
            _outerSubjectHandler.Generate(_secondPeriod.Clients, _currentDate);
        }
        private void GenerateComplexData(int dateRange, int numberOfProjects, int numberOfSupervisions, int numberOfOuterProjects, bool isFirstGeneration)
        {
            GenerateProjects(isFirstGeneration, numberOfProjects, dateRange);
            GenerateSupervisions(isFirstGeneration, numberOfSupervisions, numberOfProjects);
            GenerateOuterProjects(isFirstGeneration, numberOfOuterProjects);
        }
        private void GenerateProjects(bool isFirstGeneration, int count, int dateRange)
        {
            var shift = isFirstGeneration ? 0 : _firstPeriod.Projects;
            for (var i = 0; i < count; i++)
            {
                var clientOrderDate = _currentDate.AddDays(_rand.Next(dateRange));
                var client = _clientHandler.Clients[_rand.Next(_clientHandler.Clients.Count)];
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
            var index = isFirstGeneration ? 0 : _firstPeriod.Projects;
            for (var i = isFirstGeneration ? 0 : _firstPeriod.Supervisions; i < count; i++)
            {
                var manager = _outerSubjectHandler.OuterSubjects[_rand.Next(_outerSubjectHandler.OuterSubjects.Count)];
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
            var shift = isFirstGeneration ? 0 : _firstPeriod.OuterProjects;
            for (var i = 0; i < count; i++)
            {
                var project = _listOfProjects[_rand.Next(_listOfProjects.Count)];
                var os = _outerSubjectHandler.OuterSubjects[_rand.Next(_outerSubjectHandler.OuterSubjects.Count)];
                _listOfOuterProjects.Add(new OuterProject(i, os.Id, project.Id));
            }
        }
        private static List<Project> OrderListByStatusAndClientOrderDate(IEnumerable<Project> list)
        {
            return list.OrderBy(project => project.Status).ThenBy(project => project.ClientOrderDate).ToList();
        }


        private void WriteToFiles(string time)
        {
            _architectHandler.Write(time);
            _clientHandler.Write(time);
            _outerSubjectHandler.Write(time);
            CreateCsvHeaders(Resources.Global_Data_Path, time);

            var tmp = new List<AbstractDataModel>();

            tmp = _listOfOuterProjects.Cast<AbstractDataModel>().ToList();
            WriteToCsv(tmp, Resources.Global_Data_Path + "outer_projects_" + time + ".csv");
            tmp.Clear();

            tmp = _listOfProjects.Cast<AbstractDataModel>().ToList();
            WriteToBulk(tmp, Resources.Global_Data_Path + "projects_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfProjectsDone.Cast<AbstractDataModel>().ToList();
            WriteToBulk(tmp, Resources.Global_Data_Path + "projects_done_" + time + ".bulk");
            tmp.Clear();

            tmp = _listOfSupervisors.Cast<AbstractDataModel>().ToList();
            WriteToBulk(tmp, Resources.Global_Data_Path + "project_supervisions_" + time + ".bulk");
            tmp.Clear();
        }
        private void WriteToBulk(params object[] parameters) { }
        private void WriteToCsv(params object[] parameters) { }

        private static string GetFileName(params object[] parameters) => "a.txt";

        private static void CreateCsvHeaders(string path, string time)
        {
            File.WriteAllText(GetFileName(path, "outer_projects", time, "csv"), Resources.Generator_OuterProjects_Csv_Header, Encoding.UTF8);
        }
    }
}