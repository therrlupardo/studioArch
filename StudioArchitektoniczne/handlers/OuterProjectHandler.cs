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
        private Random _rand;
        public OuterProjectHandler()
        {
            OuterProjects = new List<OuterProject>();
            _rand = new Random(int.Parse(Resources.Global_Random_Seed));
        }
        public void Generate(int amount, params object[] parameters)
        {
            var index = OuterProjects.Any() ? OuterProjects.Count : 0;
            var projects = (List<Project>) parameters[0];
            var outerSubjects = (List<OuterSubject>) parameters[1];
            for (var i = 0; i < amount; i++)
            {
                var projectId = projects[_rand.Next(projects.Count)].Id;
                var outerSubjectId = outerSubjects[_rand.Next(outerSubjects.Count)].Id;
                OuterProjects.Add(new OuterProject(index + i, outerSubjectId, projectId));
            }
        }
        
        public void Write(string time)
        {
            File.AppendAllLines(
                $"{Resources.Global_Data_Path}outer_projects{time}.csv",
                new List<string>(){Resources.Generator_OuterProjects_Csv_Header},
                Encoding.UTF8);
            var dataModels = OuterProjects.Cast<AbstractDataModel>().ToList();
            WriteToCsv(dataModels, $"{Resources.Global_Data_Path}outer_projects_{time}.csv");
        }
    }
}