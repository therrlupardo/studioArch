using StudioArchitektoniczne.models;
using StudioArchitektoniczne.models.enums;
using StudioArchitektoniczne.models.outer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StudioArchitektoniczne
{
    class Generator
    {
        int t0clients, t0architects, t0projects, t0overwatches, t0outerProjects, t0outerSubjects;
        int t1clients, t1architects, t1projects, t1overwatches, t1outerProjects, t1outerSubjects;
        Random rand;
        DateTime currentDate = new DateTime(2010, 1, 1);
        private int overwatches = 0;
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

        Dictionary<int, int> mutatedArchitectsIdMapper = new Dictionary<int, int>();
        private int lastArchitect;
        List<string> listOfUpdates = new List<string>();

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

            var dateRange = (DateTime.Today - currentDate).Days / 10;
            GenerateSimpleDataT0();
            GenerateComplexData(dateRange, t0projects, t0overwatches, t0outerProjects, true);
            AssignArchitectsToProjects();
            WriteToFiles("t1");

            MutateData();
            WriteUpdatesToFiles("t2");

            GenerateSimpleDataT1();
            GenerateComplexData(dateRange, t1projects, t1overwatches, t1outerProjects, false);
            AssignArchitectsToProjects();
            WriteToFiles("t2");
        }
        public int CountGeneratedRecords()
        {
            return listOfArchitects.Count + listOfClients.Count + listOfOuterProjects.Count + listOfOuterSubjects.Count +
                listOfOverwatches.Count + listOfProjects.Count + listOfProjectsDone.Count;
        }

        private void AssignArchitectsToProjects()
        {
            while (listOfProjects.Find(p => p.status != ProjectStatusEnum.UKONCZONY) != null)
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                switch (type)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        CreateConnection(projectsOB, availableOBA);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        CreateConnection(projectsOM, availableOMA);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        CreateConnection(projectsOU, availableOUA);
                        break;
                }
                if (rand.Next(30) < 1)
                {
                    currentDate = currentDate.AddDays(1);
                    FreeArchitects();
                }
            }
        }
        private void CreateConnection(List<Project> projects, List<Architect> architects)
        {
            if (!architects.Any() || !projects.Any()) return;
            var neededArchitects = rand.Next(1, Math.Min(4, architects.Count));
            Project project = projects[0];
            if (project.clientOrderDate > currentDate) return;

            var id = projects.FindIndex(p => p.id == project.id);
            projects[id].startDate = currentDate;
            projects[id].endDate = currentDate.AddDays(rand.Next(10, 20));
            projects[id].updateSize(overwatches);

            var outerProjects = listOfOuterProjects.FindAll(op => op.projectId == project.id);

            foreach (var pr in outerProjects)
            {
                pr.startDate = currentDate.AddDays(rand.Next(0, 10));
                pr.endDate = pr.startDate.AddDays(rand.Next(0, 10));
            }

            for (int i = 0; i < neededArchitects; i++)
            {
                ProjectDone pd = new ProjectDone();
                pd.architectId = architects[0].id;
                architects.Remove(architects[0]);
                pd.projectId = project.id;
                listOfProjectsDone.Add(pd);
            }

            projects.RemoveAll(p => p.id == project.id);
            var overwatch = listOfOverwatches.Find(o => o.projectId == project.id);
            if (overwatch != null)
            {
                overwatches++;
                overwatch.startDate = project.endDate;
                overwatch.endDate = overwatch.startDate.AddDays(rand.Next(5, 10));
                var projectsDone = listOfProjectsDone.FindAll(pd => pd.projectId == project.id);
                List<Architect> availableOverwatchers = new List<Architect>();

                projectsDone.ForEach(pd =>
                {
                    var architect = listOfArchitects[pd.architectId].active
                        ? listOfArchitects[pd.architectId]
                        : listOfArchitects[mutatedArchitectsIdMapper.GetValueOrDefault(pd.architectId)];
                    availableOverwatchers.Add(architect);
                });
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
                    switch (p.architectureType)
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
                    .ForEach(pd =>
                    {
                        var architect = listOfArchitects[pd.architectId].active
                            ? listOfArchitects[pd.architectId]
                            : listOfArchitects[mutatedArchitectsIdMapper.GetValueOrDefault(pd.architectId)];
                        archs.Add(architect);
                    });
                var overwatch = listOfOverwatches.Find(o => endingProjects.Find(p => p.id == o.projectId) != null);
                if (overwatch != null)
                {
                    var overwatcher = archs.Find(a => a.id == overwatch.architectId);
                    if (overwatcher != null)
                    {
                        archs.Remove(overwatcher);
                        overwatches--;
                    }
                }
            }
            var endingOverwatches = listOfOverwatches.FindAll(o => o.endDate <= currentDate && o.endDate >= currentDate.AddDays(-1));
            if (endingOverwatches.Any())
            {
                List<Architect> overwatchers = new List<Architect>();
                endingOverwatches.ForEach(o =>
                {
                    var architect = listOfArchitects[o.architectId].active
                        ? listOfArchitects[o.architectId]
                        : listOfArchitects[mutatedArchitectsIdMapper.GetValueOrDefault(o.architectId)];
                    overwatchers.Add(architect);
                });
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
        private void GenerateSimpleDataT0()
        {
            for (int i = 0; i < t0clients; i++) listOfClients.Add(new Client(i));
            for (int i = 0; i < t0architects; i++) listOfArchitects.Add(new Architect(i, currentDate, new DateTime(2999, 12, 31)));
            for (int i = 0; i < t0outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject(i));
            ShuffleArchitects();
            GenerateArchitectsInitialHierarchy();
        }
        private void GenerateSimpleDataT1()
        {
            int numberOfarchitects = listOfArchitects.Count;
            for (int i = t0clients; i < t0clients + t1clients; i++) listOfClients.Add(new Client(i));
            for (int i = numberOfarchitects; i < t0architects + t1architects; i++) listOfArchitects.Add(new Architect(i, currentDate, new DateTime(2999, 12, 31)));
            for (int i = t0outerSubjects; i < t0outerSubjects + t1outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject(i));
            ShuffleArchitects();
            GenerateArchitectsAfterHierarchy();
        }
        private void GenerateComplexData(int dateRange, int numberOfProjects, int numberOfOverwatches, int numberOfOuterProjects, bool isFirstGeneration)
        {
            GenerateProjects(isFirstGeneration, numberOfProjects, dateRange);
            GenerateOverwatches(isFirstGeneration, numberOfOverwatches, numberOfProjects);
            GenerateOuterProjects(isFirstGeneration, numberOfOuterProjects);
        }
        private void GenerateProjects(bool isFirstGeneration, int count, int dateRange)
        {
            var shift = isFirstGeneration ? 0 : t0projects;
            for (int i = 0; i < count; i++)
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
        }
        private void GenerateOverwatches(bool isFirstGeneration, int count, int numberOfProjects)
        {
            var delta = count != 0 ? numberOfProjects / count : numberOfProjects;
            int index = isFirstGeneration ? 0 : t0projects;
            for (int i = isFirstGeneration ? 0 : t0overwatches; i < count; i++)
            {
                OuterSubject manager = listOfOuterSubjects[rand.Next(listOfOuterSubjects.Count)];
                Project project = listOfProjects[index];
                index += delta;
                ProjectOverwatch overwatch = new ProjectOverwatch(i, manager.id, 0, project.id);
                project.totalPrize = project.prize + overwatch.prize;
                project.isOverwatched = true;
                overwatch.updateLength();
                listOfOverwatches.Add(overwatch);
            }
        }
        private void GenerateOuterProjects(bool isFirstGeneration, int count)
        {
            var shift = isFirstGeneration ? 0 : t0outerProjects;
            for (int i = 0; i < count; i++)
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
        private void MutateData()
        {
            MutateArchitectsSpecializations();
            MutateArchitectsCanOverwatch();
        }
        private void MutateArchitectsSpecializations()
        {
            var toMutate = rand.Next(1, 4);
            List<Architect> architectsToMutate = ChooseArchitectsToMutate(toMutate);
            List<Architect> mutatedArchitects = new List<Architect>();
            // zmień specjalizacje wybranym architektom
            architectsToMutate.ForEach(architect =>
            {
                Architect mutatedArchitect = architect.Copy();
                architect.active = false;
                architect.dataWygasniecia = currentDate;
                mutatedArchitect.dataWstawienia = currentDate;
                switch (mutatedArchitect.specialization)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        mutatedArchitect.specialization = rand.Next(2) == 0 ? ArchitectureTypeEnum.OBIEKT_MIESZKALNY : ArchitectureTypeEnum.OBIEKT_USLUGOWY;
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        mutatedArchitect.specialization = rand.Next(2) == 0 ? ArchitectureTypeEnum.OBIEKT_BIUROWY : ArchitectureTypeEnum.OBIEKT_USLUGOWY;
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        mutatedArchitect.specialization = rand.Next(2) == 0 ? ArchitectureTypeEnum.OBIEKT_MIESZKALNY : ArchitectureTypeEnum.OBIEKT_BIUROWY;
                        break;
                }

                mutatedArchitect.id = listOfArchitects.Count();
                listOfArchitects.Add(mutatedArchitect);
                mutatedArchitects.Add(mutatedArchitect);
                mutatedArchitectsIdMapper.Add(architect.id, mutatedArchitect.id);
            });
            CreateSpecializatonUpdates(mutatedArchitects);
            AddArchitectsToSpecializedGroups(mutatedArchitects);
        }
        private void MutateArchitectsCanOverwatch()
        {
            var toMutate = rand.Next(1,4);
            var architectsToMutate = ChooseArchitectsToMutate(toMutate);
            var mutatedArchitects = new List<Architect>();
            architectsToMutate.ForEach(architect =>
            {
                var mutatedArchitect = architect.Copy();
                architect.active = false;
                architect.dataWygasniecia = currentDate;
                mutatedArchitect.canOverwatch = !mutatedArchitect.canOverwatch;
                mutatedArchitect.dataWstawienia = currentDate;
                mutatedArchitect.id = listOfArchitects.Count();
                mutatedArchitects.Add(mutatedArchitect);
                listOfArchitects.Add(mutatedArchitect);
                mutatedArchitectsIdMapper.Add(architect.id, mutatedArchitect.id);
            });
            CreateOverwatchUpdates(mutatedArchitects);
        }
        private List<Architect> ChooseArchitectsToMutate(int toMutate)
        {
            var architectsToMutate = new List<Architect>();
            for (int i = 0; i < toMutate; i++)
            {
                switch (RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>())
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        if (availableOBA.Any())
                        {
                            architectsToMutate.Add(availableOBA[0]);
                            availableOBA.RemoveAt(0);
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        if (availableOMA.Any())
                        {
                            architectsToMutate.Add(availableOMA[0]);
                            availableOMA.RemoveAt(0);
                        }
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        if (availableOUA.Any())
                        {
                            architectsToMutate.Add(availableOUA[0]);
                            availableOUA.RemoveAt(0);
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
                switch (architect.specialization)
                {
                    case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                        availableOBA.Add(architect);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                        availableOMA.Add(architect);
                        break;
                    case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                        availableOUA.Add(architect);
                        break;
                }
            });
        }
        private void ShuffleArchitects()
        {
            availableOBA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_BIUROWY);
            availableOMA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_MIESZKALNY);
            availableOUA = listOfArchitects.FindAll(a => a.specialization == ArchitectureTypeEnum.OBIEKT_USLUGOWY);
            availableOBA = Shuffle(availableOBA);
            availableOMA = Shuffle(availableOMA);
            availableOUA = Shuffle(availableOUA);
        }

        private void GenerateArchitectsInitialHierarchy()
        {
            var ordered = listOfArchitects.OrderByDescending(a => a.pesel).ToList();
            ordered.ForEach(a => a.idPrzelozonego = (int) Math.Floor((double) (a.id - 1) / 10));
            lastArchitect = ordered.Last().id;
        }

        private void GenerateArchitectsAfterHierarchy()
        {
            var ordered = listOfArchitects.FindAll(a => a.idPrzelozonego == -2).OrderByDescending(a => a.pesel).ToList();
            ordered.ForEach(a => a.idPrzelozonego = lastArchitect + (int)Math.Floor((double)(a.id - 1) / 10));
        }

        private void GenerateArchitectsAfterHierarchy(List<Architect> list, int groupSupervisor)
        {
            var ordered = list.FindAll(a => a.idPrzelozonego == -2).OrderByDescending(a => a.pesel).ToList();
            ordered.ForEach(a => a.idPrzelozonego = groupSupervisor + (int)Math.Floor((double)(a.id - 1) / 10));
        }
        private static List<T> Shuffle<T>(List<T> list)
        {
            Random rand = new Random(2137);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        private void WriteToFiles(string time)
        {
            string path = "../../../data/";

            CreateCsvHeaders(path, time);

            List<DataModel> tmp;
            tmp = listOfArchitects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "architects_" + time + ".csv");
            tmp.Clear();

            tmp = listOfOuterProjects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "outer_projects_" + time + ".csv");
            tmp.Clear();

            tmp = listOfOuterSubjects.Cast<DataModel>().ToList();
            WriteToCsv(tmp, path + "outer_subjects_" + time + ".csv");
            tmp.Clear();

            tmp = listOfArchitects.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "architects_" + time + ".bulk");
            tmp.Clear();

            tmp = listOfClients.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "clients_" + time + ".bulk");
            tmp.Clear();

            tmp = listOfProjects.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "projects_" + time + ".bulk");
            tmp.Clear();

            tmp = listOfProjectsDone.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "projects_done_" + time + ".bulk");
            tmp.Clear();

            tmp = listOfOverwatches.Cast<DataModel>().ToList();
            WriteToBulk(tmp, path + "project_overwatches_" + time + ".bulk");
            tmp.Clear();
        }

        private void CreateCsvHeaders(string path, string time)
        {
            File.WriteAllText(path + "outer_subjects_" + time + ".csv", "Identyfikator podmiotu,Imię,Nazwisko,Numer telefonu,\n", Encoding.UTF8);
            File.WriteAllText(path + "outer_projects_" + time + ".csv", "Identyfikator projektu,Nazwa projektu,Identyfikator podmiotu,Rodzaj projektu,Koszt,Data rozpoczęcia,Data zakończenia,Identyfikator projektu architektonicznego,\n", Encoding.UTF8);
            File.WriteAllText(path + "architects_" + time + ".csv", "Identyfikator pracownika,Imię,Nazwisko,Data urodzenia,Telefon,Identyfikator kontraktu,Uprawnienia do nadzoru,Pesel\n", Encoding.UTF8);
        }


        private void WriteToCsv(List<DataModel> list, String filename)
        {
            var text = new StringBuilder();
            for (int i = 0; i < list.Count; i++) text.AppendLine(((DataModel)list[i]).ToCsvString());
            File.AppendAllText(filename, text.ToString(), Encoding.UTF8);
        }

        private void WriteToBulk(List<DataModel> list, String filename)
        {
            var text = new StringBuilder();
            for (int i = 0; i < list.Count; i++) text.AppendLine(((DataModel)list[i]).ToBulkString());
            File.AppendAllText(filename, text.ToString(), Encoding.GetEncoding("UTF-16"));
        }

        private void CreateOverwatchUpdates(List<Architect> architects)
        {

        }

        private void CreateSpecializatonUpdates(List<Architect> architects)
        {
            foreach (var architect in architects)
            {
                var id = listOfArchitects.FindIndex(a => a.active == false && a.pesel == architect.pesel);
                listOfUpdates.Add(String.Format("UPDATE Pracownicy SET Specjalizacja='{0}' WHERE ID_PRACOWNIK={1}", architect.specialization.ToString(), id));
            }
        }

        private void WriteUpdatesToFiles(string time)
        {
            string path = "../../../data/";

            var text = new StringBuilder();
            foreach (var update in listOfUpdates) text.AppendLine(update.ToString());
            File.AppendAllText(path + "architects_update_" + time + ".sql", text.ToString(), Encoding.GetEncoding("UTF-16"));
        }
    }
}