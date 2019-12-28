using System;
using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.handlers;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio
{
    public class Generator
    {
        private GeneratorParameters _firstPeriod, _secondPeriod;
        private Random _rand;
        private DateTime _currentDate = new DateTime(2010, 1, 1);
        private int _supervisions = 0;
        private readonly List<DoneProject> _listOfProjectsDone = new List<DoneProject>();

        //        private List<Project> _projectsOm = new List<Project>();
        //        private List<Project> _projectsOu = new List<Project>();
        //        private List<Project> _projectsOb = new List<Project>();

        // fixme: maybe some list or dictionary of it?
        private ArchitectHandler _architectHandler;
        private ClientHandler _clientHandler;
        private OuterSubjectHandler _outerSubjectHandler;
        private OuterProjectHandler _outerProjectHandler;
        private ProjectHandler _projectHandler;
        private SupervisionHandler _supervisionHandler;
        private DoneProjectHandler _doneProjectHandler;
        public void InitGenerator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            _rand = new Random(int.Parse(Resources.Global_Random_Seed));
            _firstPeriod = firstPeriod;
            _secondPeriod = secondPeriod;
            _architectHandler = new ArchitectHandler();
            _clientHandler = new ClientHandler();
            _outerSubjectHandler = new OuterSubjectHandler();
            _projectHandler = new ProjectHandler(_clientHandler);
            _outerProjectHandler = new OuterProjectHandler(_projectHandler, _outerSubjectHandler);
            _supervisionHandler = new SupervisionHandler(_projectHandler, _outerSubjectHandler);
            _doneProjectHandler = new DoneProjectHandler();
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

        public void Run()
        {
            var dateRange = (DateTime.Today - _currentDate).Days / 10;
            GenerateData(_firstPeriod);
            AssignArchitectsToProjects();
            WriteToFiles("t1");

            _architectHandler.Update(_currentDate);

            GenerateData(_secondPeriod);
            AssignArchitectsToProjects();
            WriteToFiles("t2");
        }

        private void AssignArchitectsToProjects()
        {
            while (_projectHandler.Projects.Find(p => p.Status != ProjectStatusEnum.UKONCZONY) != null)
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                CreateConnection(_projectHandler.ScheduledProjects[type], _architectHandler.AvailableArchitects[type]);
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

            var outerProjects = _outerProjectHandler.OuterProjects.FindAll(op => op.ProjectId == project.Id);

            foreach (var pr in outerProjects)
            {
                pr.StartDate = _currentDate.AddDays(_rand.Next(0, 10));
                pr.EndDate = pr.StartDate.AddDays(_rand.Next(0, 10));
            }

            var pd = new DoneProject();
            for (var i = 0; i < neededArchitects; i++)
            {
                pd.ArchitectId = architects[0].Id;
                architects.Remove(architects[0]);
                pd.ProjectId = project.Id;
                _listOfProjectsDone.Add(pd);
            }

            projects.RemoveAll(p => p.Id == project.Id);
            var supervision = _supervisionHandler.Supervisions.Find(o => o.ProjectId == project.Id);
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
                        : _architectHandler.Architects[_architectHandler.IdMapper.GetValueOrDefault(projectDone.ArchitectId)];
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

        // fixme: do przeniesienia do ArchitectHandler
        private void FreeArchitects()
        {
            var architects = new List<Architect>();
            var endingProjects = _projectHandler.Projects.FindAll(p => p.StartDate >= p.ClientOrderDate &&
                p.EndDate <= _currentDate &&
                p.Status != ProjectStatusEnum.UKONCZONY
                );
            if (endingProjects.Any())
            {
                endingProjects.ForEach(project =>
                {
                    project.Status = ProjectStatusEnum.UKONCZONY;
                    _projectHandler.ScheduledProjects[project.ArchitectureType].RemoveAll(p => p.Id == project.Id);
                });
                _listOfProjectsDone.FindAll(pd => endingProjects.Find(p => p.Id == pd.ProjectId) != null)
                    .ForEach(pd =>
                    {
                        var architect = _architectHandler.Architects[pd.ArchitectId].Active
                            ? _architectHandler.Architects[pd.ArchitectId]
                            : _architectHandler.Architects[_architectHandler.IdMapper.GetValueOrDefault(pd.ArchitectId)];
                        architects.Add(architect);
                    });
                var supervision = _supervisionHandler.Supervisions.Find(o => endingProjects.Find(p => p.Id == o.ProjectId) != null);
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
            var endingSupervisions = _supervisionHandler.Supervisions.FindAll(o => o.EndDate <= _currentDate && o.EndDate >= _currentDate.AddDays(-1));
            if (endingSupervisions.Any())
            {
                var supervisors = new List<Architect>();
                endingSupervisions.ForEach(o =>
                {
                    var architect = _architectHandler.Architects[o.ArchitectId].Active
                        ? _architectHandler.Architects[o.ArchitectId]
                        : _architectHandler.Architects[_architectHandler.IdMapper.GetValueOrDefault(o.ArchitectId)];
                    supervisors.Add(architect);
                });
                architects = architects.Concat(supervisors).ToList();
            }
            architects.ForEach(a => _architectHandler.AvailableArchitects[a.Specialization].Add(a));
        }
        private void GenerateData(GeneratorParameters period)
        {
            _clientHandler.Generate(period.Clients);
            _architectHandler.Generate(period.Architects, _currentDate);
            _outerSubjectHandler.Generate(period.Clients);
            _projectHandler.Generate(period.Projects);
            _supervisionHandler.Generate(period.Supervisions);
            _outerProjectHandler.Generate(period.OuterProjects);
            _doneProjectHandler.Generate(0); // amount needed by interface, but here will be calculated on handler's side
        }

        private void WriteToFiles(string time)
        {
            _architectHandler.Write(time);
            _clientHandler.Write(time);
            _outerSubjectHandler.Write(time);
            _outerProjectHandler.Write(time);
            _projectHandler.Write(time);
            _supervisionHandler.Write(time);
            _doneProjectHandler.Write(time);
        }

        private void WriteToBulk(params object[] parameters) { }
    }
}