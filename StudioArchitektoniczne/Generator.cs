using StudioArchitektoniczne.models;
using StudioArchitektoniczne.models.enums;
using StudioArchitektoniczne.models.outer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudioArchitektoniczne
{
    class Generator
    {
        int t0clients, t0architects, t0projects, t0overwatches, t0outerProjects, t0outerSubjects;
        int t1clients, t1architects, t1projects, t1overwatches, t1outerProjects, t1outerSubjects;
        Random rand;
        DateTime currentDate = new DateTime(2010, 1, 1);

        List<Architect> listOfArchitects = new List<Architect>();
        List<Client> listOfClients = new List<Client>();
        List<Project> listOfProjects = new List<Project>();
        List<ProjectOverwatch> listOfOverwatches = new List<ProjectOverwatch>();
        List<OuterProject> listOfOuterProjects = new List<OuterProject>();
        List<OuterSubject> listOfOuterSubjects = new List<OuterSubject>();
        List<ProjectDone> listOfProjectsDone = new List<ProjectDone>();

        List<Project> projectsOM = new List<Project>();
        List<Project> projectsOU = new List<Project>();
        List<Project> projectsOB = new List<Project>();
        
        List<Architect> availableOMA = new List<Architect>();
        List<Architect> availableOUA = new List<Architect>();
        List<Architect> availableOBA = new List<Architect>();

        public Generator(int t0clients, int t0architects, int t0projects, int t0overwatches, int t0outerProjects,
            int t0outerSubjects, int t1clients, int t1architects, int t1projects,
            int t1overwatches, int t1outerProjects, int t1outerSubjects)
        {
            this.rand = new Random(2137);
            this.t0clients = t0clients;
            this.t0architects = t0architects;
            this.t0projects = t0projects;
            this.t0overwatches = t0overwatches;
            this.t0outerProjects = t0outerProjects;
            this.t0outerSubjects = t0outerSubjects;
            this.t1clients = t1clients;
            this.t1architects = t1architects;
            this.t1projects = t1projects;
            this.t1overwatches = t1overwatches;
            this.t1outerProjects = t1outerProjects;
            this.t1outerSubjects = t1outerSubjects;
        }

        public void GenerateData()
        {
            var dateRange = (DateTime.Today - currentDate).Days/10;
            GenerateT0InitData();
            GenerateProjectsAndOverwatches(dateRange, t0projects, t0overwatches, t0outerProjects, true);
            AssignArchitectsToProjects();

            // t1 -> mutacja starych danych, generowanie nowych
            MutateT0Data();
            GenerateT1InitData();
            GenerateProjectsAndOverwatches(dateRange, t1projects, t1overwatches, t1outerProjects, false);
            AssignArchitectsToProjects();
        }

        private void AssignArchitectsToProjects()
        {
            while (listOfProjects.Find(p => p.status != ProjectStatusEnum.UKONCZONY) != null)
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                switch (type)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        GenerateConnection(projectsOB, availableOBA);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        GenerateConnection(projectsOM, availableOMA);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        GenerateConnection(projectsOU, availableOUA);
                        break;
                }
                if (rand.Next(30) < 1)
                {
                    currentDate = currentDate.AddDays(1);
                    FreeArchitects();
                }
            }
        }

        private void GenerateConnection(List<Project> projects, List<Architect> architects)
        {
            if (!architects.Any() || !projects.Any()) return;
            var neededArchitects = rand.Next(1, Math.Min(4, architects.Count));
            Project project = projects.First();
            if (project.clientOrderDate > currentDate) return;

            var id = projects.FindIndex(p => p.id == project.id);
            projects[id].startDate = currentDate;
            projects[id].endDate = currentDate.AddDays(rand.Next(10, 20));
            projects[id].updateSize();

            for (int i = 0; i < neededArchitects; i++)
            {
                ProjectDone pd = new ProjectDone();
                pd.architectId = architects.First().id;
                architects.Remove(architects.First());
                pd.projectId = project.id;
                listOfProjectsDone.Add(pd);
            }

            projects.RemoveAll(p => p.id == project.id);
            var overwatch = listOfOverwatches.Find(o => o.projectId == project.id);
            if (overwatch != null)
            {
                overwatch.startDate = project.endDate;
                overwatch.endDate = overwatch.startDate.AddDays(rand.Next(5, 10));
                var projectsDone = listOfProjectsDone.FindAll(pd => pd.projectId == project.id);
                List<Architect> availableOverwatchers = new List<Architect>();
                projectsDone.ForEach(pd => availableOverwatchers.Add(listOfArchitects[pd.architectId]));
                if (availableOverwatchers.Any() && availableOverwatchers.Find(o => o.canOverwatch) != null)
                {
                    var arch = availableOverwatchers.Find(o => o.canOverwatch);
                    if (arch != null) overwatch.architectId = arch.id;
                }
                else
                {
                    var arch = architects.Find(a => a.canOverwatch);
                    if (arch != null) overwatch.architectId = arch.id;
                }
            }
        }

        private void FreeArchitects()
        {
            List<Architect> archs = new List<Architect>();
            var endingProjects = listOfProjects.FindAll(p => p.startDate >= p.clientOrderDate &&
                p.endDate <= currentDate &&
                p.status != ProjectStatusEnum.UKONCZONY
                );
            if (endingProjects.Any())
            {
                endingProjects.ForEach(p =>
                {
                    p.status = ProjectStatusEnum.UKONCZONY;
                    switch(p.architectureType)
                    {
                        case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                            projectsOB.RemoveAll(project => project.id == p.id);
                            break;
                        case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                            projectsOM.RemoveAll(project => project.id == p.id);
                            break;
                        case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                            projectsOU.RemoveAll(project => project.id == p.id);
                            break;
                    }
                });
                List<int> ids = new List<int>();
                listOfProjectsDone.FindAll(pd => endingProjects.Find(p => p.id == pd.projectId) != null)
                    .ForEach(pd => archs.Add(listOfArchitects[pd.architectId]));
                var overwatch = listOfOverwatches.Find(o => endingProjects.Find(p => p.id == o.projectId) != null);
                if (overwatch != null)
                {
                    var overwatcher = archs.Find(a => a.id == overwatch.architectId);
                    if (overwatcher != null)
                    {
                        archs.Remove(overwatcher);
                    }
                }
            }
            var endingOverwatches = listOfOverwatches.FindAll(o => o.endDate <= currentDate && o.endDate >= currentDate.AddDays(-1));
            if (endingOverwatches.Any())
            {
                List<Architect> overwatchers = new List<Architect>();
                endingOverwatches.ForEach(o => overwatchers.Add(listOfArchitects[o.architectId]));
                archs.Concat(overwatchers);
            }
            archs.ForEach(a =>
            {
                switch (a.specialization)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        availableOBA.Add(a);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        availableOMA.Add(a);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        availableOUA.Add(a);
                        break;
                }
            });
        }

        private void GenerateProjectsAndOverwatches(int dateRange, int numberOfProjects, int numberOfOverwatches, int numberOfOuterProjects, bool isFirstGeneration)
        {
            var shift = isFirstGeneration ? 0 : t0projects;
            for (int i = 0; i < numberOfProjects; i++)
            {
                DateTime clientOrderDate = currentDate.AddDays(rand.Next(dateRange));
                Client client = listOfClients[rand.Next(listOfClients.Count)];
                Project project = new Project(
                    i + shift,
                    clientOrderDate, 
                    client.id);
                listOfProjects.Add(project);
                switch (project.architectureType)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        projectsOB.Add(project);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        projectsOM.Add(project);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        projectsOU.Add(project);
                        break;
                }
            }

            projectsOB = OrderListByStatusAndClientOrderDate(projectsOB);
            projectsOM = OrderListByStatusAndClientOrderDate(projectsOM);
            projectsOU = OrderListByStatusAndClientOrderDate(projectsOU);

            var delta = numberOfOverwatches != 0 ? numberOfProjects / numberOfOverwatches : numberOfProjects;
            int index = isFirstGeneration ? 0 : t0projects;
            for (int i = 0; i < numberOfOverwatches; i++)
            {
                OuterSubject manager = listOfOuterSubjects[rand.Next(listOfOuterSubjects.Count)];
                Project project = listOfProjects[index];
                index += delta;
                ProjectOverwatch overwatch = new ProjectOverwatch(i, manager.id, 0, project.id);
                listOfOverwatches.Add(overwatch);
            }

            shift = isFirstGeneration ? 0 : t0outerProjects;
            for (int i = 0; i < numberOfOuterProjects; i++)
            {
                Project project = listOfProjects[rand.Next(listOfProjects.Count)];
                OuterSubject os = listOfOuterSubjects[rand.Next(listOfOuterSubjects.Count)];
                listOfOuterProjects.Add(new OuterProject(i, os.id, project.id));
            }
        }

        private List<Project> OrderListByStatusAndClientOrderDate(List<Project> list)
        {
            return list.OrderBy(project => project.status).ThenBy(project => project.clientOrderDate).ToList();
        }

        private void GenerateT0InitData()
        {
            for (int i = 0; i < t0clients; i++) listOfClients.Add(new Client(i));
            for (int i = 0; i < t0architects; i++) listOfArchitects.Add(new Architect(i));
            this.availableOBA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_BIUROWY);
            this.availableOMA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_MIESZKALNY);
            this.availableOUA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_USLUGOWY);
            for (int i = 0; i < t0outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject(i));
            Console.WriteLine();
        }

        private void MutateT0Data()
        {
            // todo: zmienić losowe dane na inne (tylko architekci mogą być zmienieni żeby to miało sens?)
        }

        private void GenerateT1InitData()
        {
            for (int i = t0clients; i < t0clients + t1clients; i++) listOfClients.Add(new Client(i));
            for (int i = t0architects; i < t0architects + t1architects; i++) listOfArchitects.Add(new Architect(i));
            for (int i = t0outerSubjects; i < t0outerSubjects + t1outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject(i));
        }

        public int CountGeneratedRecords()
        {
            return listOfArchitects.Count + listOfClients.Count + listOfOuterProjects.Count + listOfOuterSubjects.Count +
                listOfOverwatches.Count + listOfProjects.Count + listOfProjectsDone.Count;
        }

    }
}