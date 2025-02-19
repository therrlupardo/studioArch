﻿using System;
using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.outer;
using ArchitecturalStudio.Properties;
using static ArchitecturalStudio.models.enums.StatusEnum;

namespace ArchitecturalStudio.handlers
{
    public class SupervisionHandler : AbstractHandler, IGenerator, IWritable
    {
        public List<ProjectSupervision> Supervisions { get; set; }
        private readonly ProjectHandler _projectHandler;
        private readonly OuterSubjectHandler _outerSubjectHandler;

        public SupervisionHandler(ProjectHandler projectHandler, OuterSubjectHandler outerSubjectHandler)
        {
            Supervisions = new List<ProjectSupervision>();
            _projectHandler = projectHandler;
            _outerSubjectHandler = outerSubjectHandler;
        }
        public void Generate(int amount, params object[] parameters)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException();

            var index = Supervisions.Any() ? (_projectHandler.Projects.FindAll(p => p.Status != UKONCZONY).Count - 1) : 0;
            var delta = Supervisions.Count / amount;
            for (var i = 0; i < amount; i++)
            {
                var manager = _outerSubjectHandler.GetRandomOuterSubject();
                var project = _projectHandler.Projects[index];
                CreateSupervision(manager, project);
                index += delta;
            }
        }

        private void CreateSupervision(OuterSubject manager, Project project)
        {
            var supervision = new ProjectSupervision(Supervisions.Count + 1, manager.Id, 0, project.Id);
            project.TotalPrize = project.Prize + supervision.Prize;
            project.IsSupervised = true;
            supervision.RecalculateFields();
            Supervisions.Add(supervision);
        }

        public void Write(string time)
        {
            var dataModels = Supervisions.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}project_supervisions_{time}.bulk");
        }

        public ProjectSupervision GetOneById(int id)
        {
            return Supervisions.Find(a => a.Id == id);
        }

        public List<ProjectSupervision> GetEndingSupervisions(DateTime currentDate)
        {
            return Supervisions.FindAll(o => o.EndDate <= currentDate && o.EndDate >= currentDate.AddDays(-1));
        }

        public int CountActive()
        {
            return Supervisions.FindAll(s => s.Status == W_TRAKCIE_PRAC).Count;
        }

        public void EndSupervision(ProjectSupervision supervision)
        {
            supervision.Status = UKONCZONY;
        }
    }
}