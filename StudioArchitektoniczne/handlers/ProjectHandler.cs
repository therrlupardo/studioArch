using System;
using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;
using static ArchitecturalStudio.models.enums.ArchitectureTypeEnum;

namespace ArchitecturalStudio.handlers
{
    public class ProjectHandler : AbstractHandler, IGenerator, IWritable
    {
        public List<Project> Projects { get; set; }
        public Dictionary<ArchitectureTypeEnum, List<Project>> ScheduledProjects { get; set; }
        private readonly ClientHandler _clientHandler;
        private readonly Random _random;

        public ProjectHandler(ClientHandler clientHandler)
        {
            Projects = new List<Project>();
            _random = new Random(int.Parse(Resources.Global_Random_Seed));
            ScheduledProjects = new Dictionary<ArchitectureTypeEnum, List<Project>>()
            {
                { OBIEKT_MIESZKALNY, new List<Project>() },
                { OBIEKT_BIUROWY, new List<Project>() },
                { OBIEKT_USLUGOWY, new List<Project>() },
            };
            _clientHandler = clientHandler;
        }
        public void Generate(int amount, params object[] parameters)
        {
            if (!DateTime.TryParse(parameters[0].ToString(), out var currentDate)) throw new ArgumentOutOfRangeException();
            for (var i = 0; i < amount; i++)
            {
                var project = CreateProject(currentDate); 
                Projects.Add(project);
                ScheduledProjects[project.ArchitectureType].Add(project);
            }

            ScheduledProjects[OBIEKT_MIESZKALNY] =
                OrderListByStatusAndClientOrderDate(ScheduledProjects[OBIEKT_MIESZKALNY]);
            ScheduledProjects[OBIEKT_BIUROWY] =
                OrderListByStatusAndClientOrderDate(ScheduledProjects[OBIEKT_BIUROWY]);
            ScheduledProjects[OBIEKT_USLUGOWY] =
                OrderListByStatusAndClientOrderDate(ScheduledProjects[OBIEKT_USLUGOWY]);
        }

        private Project CreateProject(DateTime currentDate)
        {
            var clientOrderDate = CreateClientOrderDate(currentDate);
            var client = _clientHandler.GetRandomClient();
            return new Project(Projects.Count + 1, clientOrderDate, client.Id);
        }

        private DateTime CreateClientOrderDate(DateTime currentDate)
        {
            return currentDate.AddDays(_random.Next(3650));
        }

        public void Write(string time)
        {
            var dataModels = Projects.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}projects_{time}.bulk");
        }

        public Project GetRandomProject()
        {
            return Projects[_random.Next(Projects.Count)];
        }
        private static List<Project> OrderListByStatusAndClientOrderDate(IEnumerable<Project> list)
        {
            return list.OrderBy(project => project.Status).ThenBy(project => project.ClientOrderDate).ToList();
        }

        public List<Project> GetProjectsByEndDate(DateTime date)
        {
            return Projects.FindAll(IsEnding(date));
        }

        private static Predicate<Project> IsEnding(DateTime date)
        {
            return project => project.StartDate >= project.ClientOrderDate &&
                        project.EndDate <= date &&
                        project.Status != StatusEnum.UKONCZONY;
        }

        public void EndProject(Project project)
        {
            project.Status = StatusEnum.UKONCZONY;
            ScheduledProjects[project.ArchitectureType].RemoveAll(p => p.Id == project.Id);
        }
    }
}