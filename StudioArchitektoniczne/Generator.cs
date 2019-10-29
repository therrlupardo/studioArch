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

        DateTime initDate = new DateTime(2010, 1, 1);
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
            var dateRange = (DateTime.Today - initDate).Days;
            GenerateT0InitData();
            GenerateProjectsAndOverwatches(dateRange);
            AssignArchitectsToProjects();

            // t1 -> mutacja starych danych, generowanie nowych, 
            MutateT0Data();
            GenerateT1InitData();
            Console.WriteLine();

            // --- start t1


            //while .....

            // client order date -> rand (t0, t1> 

            // project
            //  rand   ->   client id    architect(s) id         

            // projectoverwatch
            //  rand   ->   start date   ( > project end date )

            // outerprojects 
            // if(rand ...)
            //  rand         subject id  start date ( > project end date  &&  < overwatch start date)

            // if(rand ...)
            // create clients, architects, outer subjects ...

            // end while


            // --- end t1


        }

        private void AssignArchitectsToProjects()
        {
            Random rand = new Random();
            while (availableOBA.Any() || availableOMA.Any() || availableOUA.Any())
            {
                switch (RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>())
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        {
                            if (projectsOB.FindAll(p => p.status != ProjectStatusEnum.UKONCZONY).Count == 0) break;
                            if (!availableOBA.Any()) break;
                            var neededArchitects = rand.Next(1, Math.Min(4, availableOBA.Count));
                            Project project = projectsOB.First();
                            project.startDate = currentDate; // czy to nadpisze listę projects?
                            project.endDate = currentDate.AddDays(rand.Next(100, 300));
                            for (int i = 0; i < neededArchitects; i++)
                            {
                                ProjectDone pd = new ProjectDone();
                                pd.architectId = availableOBA.First().id;
                                availableOBA.Remove(availableOBA.First());
                                pd.projectId = project.id;
                                listOfProjectsDone.Add(pd);
                            }
                            var overwatch = listOfOverwatches.Find(o => o.projectId == project.id);
                            if (overwatch != null)
                            {
                                overwatch.startDate = project.endDate;
                                overwatch.endDate = overwatch.startDate.AddDays(rand.Next(50, 150));
                                var projectsDone = listOfProjectsDone.FindAll(pd => pd.projectId == project.id);
                                var availableOverwatchers = listOfArchitects.FindAll(a => projectsDone.FindIndex(pd => pd.architectId == a.id) != -1);
                                if (availableOverwatchers.Any() && availableOverwatchers.Find(o => o.canOverwatch) != null)
                                {
                                    var arch = availableOverwatchers.Find(o => o.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                                else
                                {
                                    var arch = availableOBA.Find(a => a.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                            }
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        {
                            if (projectsOB.FindAll(p => p.status != ProjectStatusEnum.UKONCZONY).Count == 0) break;
                            if (!availableOMA.Any()) break;
                            var neededArchitects = rand.Next(1, Math.Min(4, availableOMA.Count));
                            Project project = projectsOM.First();
                            project.startDate = currentDate; // czy to nadpisze listę projects?
                            project.endDate = currentDate.AddDays(rand.Next(100, 300));
                            for (int i = 0; i < neededArchitects; i++)
                            {
                                ProjectDone pd = new ProjectDone();
                                pd.architectId = availableOMA.First().id;
                                availableOMA.Remove(availableOMA.First());
                                pd.projectId = project.id;
                                listOfProjectsDone.Add(pd);
                            }
                            var overwatch = listOfOverwatches.Find(o => o.projectId == project.id);
                            if (overwatch != null)
                            {
                                overwatch.startDate = project.endDate;
                                overwatch.endDate = overwatch.startDate.AddDays(rand.Next(50, 150));
                                var projectsDone = listOfProjectsDone.FindAll(pd => pd.projectId == project.id);
                                var availableOverwatchers = listOfArchitects.FindAll(a => projectsDone.FindIndex(pd => pd.architectId == a.id) != -1);
                                if (availableOverwatchers.Any() && availableOverwatchers.Find(o => o.canOverwatch) != null)
                                {
                                    var arch = availableOverwatchers.Find(o => o.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                                else
                                {
                                    var arch = availableOBA.Find(o => o.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                            }
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        {
                            if (projectsOB.FindAll(p => p.status != ProjectStatusEnum.UKONCZONY).Count == 0) break;
                            if (!availableOUA.Any()) break;
                            var neededArchitects = rand.Next(1, Math.Min(4, availableOUA.Count));
                            Project project = projectsOU.First();
                            project.startDate = currentDate; // czy to nadpisze listę projects?
                            project.endDate = currentDate.AddDays(rand.Next(100, 300));
                            for (int i = 0; i < neededArchitects; i++)
                            {
                                ProjectDone pd = new ProjectDone();
                                pd.architectId = availableOUA.First().id;
                                availableOUA.Remove(availableOUA.First());
                                pd.projectId = project.id;
                                listOfProjectsDone.Add(pd);
                            }
                            var overwatch = listOfOverwatches.Find(o => o.projectId == project.id);
                            if (overwatch != null)
                            {
                                overwatch.startDate = project.endDate;
                                overwatch.endDate = overwatch.startDate.AddDays(rand.Next(50, 150));
                                var projectsDone = listOfProjectsDone.FindAll(pd => pd.projectId == project.id);
                                var availableOverwatchers = listOfArchitects.FindAll(a => projectsDone.FindIndex(pd => pd.architectId == a.id) != -1);
                                if (availableOverwatchers.Any() && availableOverwatchers.Find(o => o.canOverwatch) != null)
                                {
                                    var arch = availableOverwatchers.Find(o => o.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                                else
                                {
                                    var arch = availableOBA.Find(a => a.canOverwatch);
                                    if (arch != null) overwatch.architectId = arch.id;
                                }
                            }
                        }
                        break;
                }

                currentDate.AddDays(rand.Next(2));
                FreeArchitects();
            }
        }

        private void FreeArchitects()
        {
            List<Architect> archs = new List<Architect>();
            var endingProjects = listOfProjects.FindAll(p => p.endDate <= currentDate && p.status != ProjectStatusEnum.UKONCZONY);
            if (endingProjects.Any())
            {
                endingProjects.ForEach(p => p.status = ProjectStatusEnum.UKONCZONY);
                var pds = listOfProjectsDone.FindAll(pd => endingProjects.Find(p => p.id == pd.projectId) != null);
                archs = listOfArchitects.FindAll(a => pds.Find(pd => pd.architectId == a.id) != null);
                var overwatch = listOfOverwatches.Find(o => endingProjects.Find(p => p.id == o.projectId) != null);
                var overwatcher = archs.Find(a => a.id == overwatch.architectId);
                if (overwatcher != null)
                {
                    archs.Remove(overwatcher);
                }
                // else overwatcher was not designing project, so won't be removed like that
            }
            var endingOverwatches = listOfOverwatches.FindAll(o => o.endDate <= currentDate && o.endDate >= currentDate.AddDays(-2));
            if (endingOverwatches.Any())
            {
                archs = listOfArchitects.FindAll(a => endingOverwatches.Find(o => o.architectId == a.id) != null);
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

        private void GenerateProjectsAndOverwatches(int dateRange)
        {
            for (int i = 0; i < t0projects; i++)
            {
                DateTime clientOrderDate = this.initDate.AddDays(new Random().Next(dateRange));
                Client client = listOfClients[new Random().Next(listOfClients.Count)];
                Project project = new Project(clientOrderDate, client.id);
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

            listOfProjects = OrderListByStatusAndClientOrderDate(listOfProjects);
            projectsOB = OrderListByStatusAndClientOrderDate(projectsOB);
            projectsOM = OrderListByStatusAndClientOrderDate(projectsOM);
            projectsOU = OrderListByStatusAndClientOrderDate(projectsOU);

            for (int i = 0; i < t0overwatches; i++)
            {
                OuterSubject manager = listOfOuterSubjects[new Random().Next(listOfOuterSubjects.Count)];
                Project project = listOfProjects[new Random().Next(listOfProjects.Count)];
                while (listOfOverwatches.FindIndex(overwatch => overwatch.projectId == project.id) != -1)
                {
                    project = listOfProjects[new Random().Next(listOfProjects.Count)];
                }
                ProjectOverwatch overwatch = new ProjectOverwatch(manager.id, Guid.Empty, project.id); // będzie nadzór, ale nie ma jeszcze przypisanego architekta
                listOfOverwatches.Add(overwatch);
            }

            for (int i = 0; i < t0outerProjects; i++)
            {
                Project project = listOfProjects[new Random().Next(listOfProjects.Count)];
                OuterSubject os = listOfOuterSubjects[new Random().Next(listOfOuterSubjects.Count)];
                listOfOuterProjects.Add(new OuterProject(os.id, project.id));
            }
        }

        private List<Project> OrderListByStatusAndClientOrderDate(List<Project> list)
        {
            return list.OrderBy(project => project.status).ThenBy(project => project.clientOrderDate).ToList();
        }

        private void GenerateT0InitData()
        {
            for (int i = 0; i < t0clients; i++) listOfClients.Add(new Client());
            for (int i = 0; i < t0architects; i++) listOfArchitects.Add(new Architect());
            this.availableOBA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_BIUROWY);
            this.availableOMA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_MIESZKALNY);
            this.availableOUA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_USLUGOWY);
            for (int i = 0; i < t0outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject());
            Console.WriteLine();
        }

        private void MutateT0Data()
        {
            // todo: zmienić losowe dane na inne (tylko architekci mogą być zmienieni żeby to miało sens?)
        }

        private void GenerateT1InitData()
        {
            for (int i = 0; i < t1clients; i++) listOfClients.Add(new Client());
            for (int i = 0; i < t1architects; i++) listOfArchitects.Add(new Architect());
            for (int i = 0; i < t1outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject());
        }

    }
}