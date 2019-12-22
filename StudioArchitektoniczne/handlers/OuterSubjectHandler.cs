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
    public class OuterSubjectHandler: AbstractHandler, IGenerator, IWritable
    {
        public List<OuterSubject> OuterSubjects { get; set; }
        public void Generate(int amount, params object[] parameters)
        {
            var index = OuterSubjects.Any() ? OuterSubjects.Count : 0;
            for (var i = 0; i < amount; i++) OuterSubjects.Add(new OuterSubject(index + i));
        }

        public void Write(string time)
        {
            File.AppendAllLines(
                $"{Resources.Global_Data_Path}outer_subjects_{time}.csv", 
                new List<string>(){Resources.Generator_OuterSubjects_Csv_Header},
                Encoding.UTF8);
            var dataModels = OuterSubjects.Cast<AbstractDataModel>().ToList();
            WriteToCsv(dataModels, $"{Resources.Global_Data_Path}outer_subjects_{time}.csv");
        }
    }
}