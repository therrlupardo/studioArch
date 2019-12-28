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
    public class OuterSubjectHandler: AbstractHandler, IGenerator, IWritable
    {
        public List<OuterSubject> OuterSubjects { get; set; }
        private readonly Random _random;

        public OuterSubjectHandler()
        {
            _random = new Random(int.Parse(Resources.Global_Random_Seed));
        }
        public void Generate(int amount, params object[] parameters)
        {
            for (var i = 0; i < amount; i++) OuterSubjects.Add(new OuterSubject(OuterSubjects.Count + 1));
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

        public OuterSubject GetRandomOuterSubject()
        {
            return OuterSubjects[_random.Next(OuterSubjects.Count)];
        }
    }
}