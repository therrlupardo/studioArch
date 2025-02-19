﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio.handlers
{
    public class DoneProjectHandler : AbstractHandler, IGenerator, IWritable
    {
        public List<DoneProject> DoneProjects { get; set; }
        private readonly ProjectHandler _projectHandler;
        private readonly ArchitectHandler _architectHandler;
        private readonly SupervisionHandler _supervisionHandler;
        private readonly OuterProjectHandler _outerProjectHandler;
        private readonly Random _random;
        public DateTime CurrentDate { get; set; }
        public DoneProjectHandler(ProjectHandler projectHandler, ArchitectHandler architectHandler,
            SupervisionHandler supervisionHandler, OuterProjectHandler outerProjectHandler)
        {
            DoneProjects = new List<DoneProject>();
            _projectHandler = projectHandler;
            _architectHandler = architectHandler;
            _supervisionHandler = supervisionHandler;
            _outerProjectHandler = outerProjectHandler;
            _random = new Random(int.Parse(Resources.Global_Random_Seed));
            CurrentDate = new DateTime(2010, 1, 1);
    }
        public void Generate(int amount, params object[] parameters)
        {
            while (IsAnyProjectRunning())
            {
                var type = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
                var connectionCreated = CreateConnection(type);
                if (connectionCreated && IsSameDay()) continue;
                CurrentDate = CurrentDate.AddDays(1);
                FreeArchitects();
            }
            Console.WriteLine(CurrentDate);
        }

        private bool IsAnyProjectRunning()
        {
            return _projectHandler.Projects.Find(p => p.Status != StatusEnum.UKONCZONY) != null;
        }

        private bool IsSameDay()
        {
            return _random.Next(30) >= 1;
        }

        public void Write(string time)
        {
            var dataModels = DoneProjects.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}projects_done_{time}.bulk");
        }

        public void FreeArchitects()
        {
            var architects = FreeFromProjects();
            architects = FreeFromSupervisions(architects);
            architects.ForEach(a => _architectHandler.AvailableArchitects[a.Specialization].Add(a));
        }

        private List<Architect> FreeFromSupervisions(List<Architect> architects)
        {
            var endingSupervisions = _supervisionHandler.GetEndingSupervisions(CurrentDate);
            if (!endingSupervisions.Any()) return architects;
            endingSupervisions.ForEach(_supervisionHandler.EndSupervision);
            endingSupervisions.ForEach(o => architects.Add(_architectHandler.GetOneById(o.ArchitectId)));
            return architects;
        }

        private List<Architect> FreeFromProjects()
        {
            var architectsToFree = new List<Architect>();
            var endingProjects = _projectHandler.GetProjectsByEndDate(CurrentDate);
            if (!endingProjects.Any()) return architectsToFree;

            endingProjects.ForEach(_projectHandler.EndProject);
            DoneProjects.FindAll(project => IsProjectEnding(endingProjects, project.ProjectId))
                .ForEach(project => architectsToFree.Add(_architectHandler.GetOneById(project.ArchitectId)));

            RemoveSupervisor(endingProjects, architectsToFree);

            return architectsToFree;
        }

        private void RemoveSupervisor(List<Project> endingProjects, List<Architect> architects)
        {
            var supervision = _supervisionHandler.Supervisions.Find(s => IsProjectEnding(endingProjects, s.Id));
            if (supervision == null) return;
            var supervisor = architects.Find(a => a.Id == supervision.ArchitectId);
            if (supervisor == null) return;
            architects.Remove(supervisor);
            supervision.Status = StatusEnum.W_TRAKCIE_PRAC;
        }

        private static bool IsProjectEnding(List<Project> endingProjects, int id)
        {
            return endingProjects.Find(p => p.Id == id) != null;
        }

        private bool CreateConnection(ArchitectureTypeEnum type)
        {
            var projects = _projectHandler.ScheduledProjects[type];
            var architects = _architectHandler.AvailableArchitects[type];
            if (IsAnyArchitectAndProjectAvailable(projects, architects)) return false;
            var project = projects[0];
            if (IsProjectOrdered(project)) return false;
            CreateDoneProject(architects, project);
            UpdateProject(projects, project.Id);
            UpdateOuterProjects(project.Id);
            UpdateSupervision(project, architects);
            projects.RemoveAll(p => p.Id == project.Id);
            return true;
        }

        private void UpdateSupervision(Project project, List<Architect> architects)
        {
            var supervision = _supervisionHandler.GetOneById(project.Id);
            if (supervision == null) return;
            UpdateSupervisionDates(supervision, project.EndDate);
            AssignSupervisor(project, supervision, architects);
        }

        private void UpdateSupervisionDates(ProjectSupervision supervision, DateTime endDate)
        {
            supervision.StartDate = endDate;
            supervision.EndDate = supervision.StartDate.AddDays(_random.Next(5, 10));
        }

        private void AssignSupervisor(Project project, ProjectSupervision supervision, List<Architect> architects)
        {
            var projectsDone = GetAllByProjectId(project.Id);
            var availableSupervisors = new List<Architect>();

            projectsDone.ForEach(projectDone => availableSupervisors.Add(_architectHandler.GetOneById(projectDone.ArchitectId)));
            var architect = IsAnySupervisorAvailable(availableSupervisors)
                ? availableSupervisors.Find(o => o.CanSupervise)
                : architects.Find(a => a.CanSupervise);
            if (architect != null) supervision.ArchitectId = architect.Id;
        }

        private static bool IsAnySupervisorAvailable(IReadOnlyCollection<Architect> availableSupervisors)
        {
            return availableSupervisors.Any() && availableSupervisors.FirstOrDefault(o => o.CanSupervise) != null;
        }

        private void CreateDoneProject(List<Architect> architects, Project project)
        {
            var architectsOnProject = GetArchitectsOnProject(architects);
            for (var i = 0; i < architectsOnProject; i++)
            {
                DoneProjects.Add(new DoneProject
                {
                    ArchitectId = architects[0].Id,
                    ProjectId = project.Id
                });
                architects.Remove(architects[0]);
            }
        }

        private void UpdateOuterProjects(int id)
        {
            foreach (var pr in _outerProjectHandler.GetAllById(id))
            {
                pr.StartDate = CurrentDate.AddDays(_random.Next(0, 10));
                pr.EndDate = pr.StartDate.AddDays(_random.Next(0, 10));
            }
        }

        private int GetArchitectsOnProject(ICollection architects)
        {
            return _random.Next(1, Math.Min(4, architects.Count+1));
        }

        private void UpdateProject(List<Project> projects, int id)
        {
            var index = projects.FindIndex(p => p.Id == id);
            projects[index].StartDate = CurrentDate;
            projects[index].EndDate = projects[index].StartDate.AddDays(_random.Next(10, 20));
            projects[index].Update(_supervisionHandler.CountActive());
        }

        private bool IsProjectOrdered(Project project)
        {
            return project.ClientOrderDate > CurrentDate;
        }

        private static bool IsAnyArchitectAndProjectAvailable(IEnumerable<Project> projects, IEnumerable<Architect> architects)
        {
            return !architects.Any() || !projects.Any();
        }

        public List<DoneProject> GetAllByProjectId(int id)
        {
            return DoneProjects.FindAll(a => a.ProjectId == id).ToList();
        }
    }
}