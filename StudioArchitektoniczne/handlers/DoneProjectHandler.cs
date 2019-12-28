using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio.handlers
{
    public class DoneProjectHandler: AbstractHandler, IGenerator, IWritable
    {
        public List<DoneProject> DoneProjects { get; set; }

        public DoneProjectHandler()
        {
            DoneProjects = new List<DoneProject>();
        }
        public void Generate(int amount, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string time)
        {
            var dataModels = DoneProjects.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}projects_done_{time}.bulk");
        }
    }
}