using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.outer;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio.handlers
{
    public class OuterProjectHandler: AbstractHandler, IGenerator, IWritable
    {
        public List<OuterProject> OuterProjects { get; set; }
        private readonly OuterSubjectHandler _outerSubjectHandler;
        private readonly ProjectHandler _projectHandler;
        private Random _rand;
        public OuterProjectHandler(ProjectHandler projectHandler, OuterSubjectHandler outerSubjectHandler)
        {
            OuterProjects = new List<OuterProject>();
            _rand = new Random(int.Parse(Resources.Global_Random_Seed));
            _projectHandler = projectHandler;
            _outerSubjectHandler = outerSubjectHandler;
        }
        public void Generate(int amount, params object[] parameters)
        {
            for (var i = 0; i < amount; i++)
            {
                var projectId = _projectHandler.GetRandomProject().Id;
                var outerSubjectId = _outerSubjectHandler.GetRandomOuterSubject().Id;
                OuterProjects.Add(new OuterProject(OuterProjects.Count + 1, outerSubjectId, projectId));
            }
        }
        
        public void Write(string time)
        {
            File.AppendAllLines(
                $"{Resources.Global_Data_Path}outer_projects_{time}.csv",
                new List<string>(){Resources.Generator_OuterProjects_Csv_Header},
                Encoding.UTF8);
            var dataModels = OuterProjects.Cast<AbstractDataModel>().ToList();
            WriteToCsv(dataModels, $"{Resources.Global_Data_Path}outer_projects_{time}.csv");
        }

        public List<OuterProject> GetAllById(int id)
        {
            return OuterProjects.FindAll(a => a.Id == id).ToList();
        }
    }
}