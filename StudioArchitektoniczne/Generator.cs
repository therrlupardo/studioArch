using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchitecturalStudio.handlers;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio
{
    public class Generator
    {
        private GeneratorParameters _firstPeriod, _secondPeriod;
        private Random _rand;

        // fixme: maybe some list or dictionary of it?
        private ArchitectHandler _architectHandler;
        private ClientHandler _clientHandler;
        private OuterSubjectHandler _outerSubjectHandler;
        private OuterProjectHandler _outerProjectHandler;
        private ProjectHandler _projectHandler;
        private SupervisionHandler _supervisionHandler;
        private DoneProjectHandler _doneProjectHandler;
        public void InitGenerator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            _rand = new Random(int.Parse(Resources.Global_Random_Seed));
            _firstPeriod = firstPeriod;
            _secondPeriod = secondPeriod;
            _architectHandler = new ArchitectHandler();
            _clientHandler = new ClientHandler();
            _outerSubjectHandler = new OuterSubjectHandler();
            _projectHandler = new ProjectHandler(_clientHandler);
            _outerProjectHandler = new OuterProjectHandler(_projectHandler, _outerSubjectHandler);
            _supervisionHandler = new SupervisionHandler(_projectHandler, _outerSubjectHandler);
            _doneProjectHandler = new DoneProjectHandler(_projectHandler, _architectHandler, _supervisionHandler, _outerProjectHandler);
        }

        public Generator(GeneratorSize size)
        {
            switch (size)
            {
                case GeneratorSize.SMALL:
                    InitSmallGenerator();
                    break;
                case GeneratorSize.HUNDRED_THOUSAND:
                    InitHundredThousandGenerator();
                    break;
                case GeneratorSize.QUARTER_MILLION:
                    InitQuarterMillionGenerator();
                    break;
                case GeneratorSize.HALF_MILLION:
                    InitHalfMillionGenerator();
                    break;
                case GeneratorSize.MILLION:
                    InitMillionGenerator();
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public Generator(GeneratorParameters firstPeriod, GeneratorParameters secondPeriod)
        {
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitSmallGenerator()
        {
            var firstPeriod = new GeneratorParameters(10, 100, 200, 10, 10, 10);
            var secondPeriod = new GeneratorParameters(10, 100, 200, 10, 10, 10);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitHundredThousandGenerator()
        {
            var firstPeriod = new GeneratorParameters(200, 30000, 10000, 2000, 2000, 200);
            var secondPeriod = new GeneratorParameters(100, 15000, 10000, 1000, 1000, 100);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitQuarterMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(400, 60000, 20000, 4000, 4000, 400);
            var secondPeriod = new GeneratorParameters(200, 30000, 10000, 2000, 2000, 200);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitHalfMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(1000, 150000, 50000, 10000, 10000, 1000);
            var secondPeriod = new GeneratorParameters(500, 75000, 25000, 5000, 5000, 500);
            InitGenerator(firstPeriod, secondPeriod);
        }

        private void InitMillionGenerator()
        {
            var firstPeriod = new GeneratorParameters(2000, 300000, 100000, 20000, 20000, 2000);
            var secondPeriod = new GeneratorParameters(1000, 150000, 100000, 10000, 10000, 1000);
            InitGenerator(firstPeriod, secondPeriod);
        }

        public void Run()
        {
            RemoveOldData();
            GenerateData(_firstPeriod);
            WriteToFiles("t1");
            _architectHandler.Update(_doneProjectHandler.CurrentDate);
            GenerateData(_secondPeriod);
            WriteToFiles("t2");
        }

        private static void RemoveOldData()
        {
            var directoryInfo = new DirectoryInfo(Resources.Global_Data_Path);
            foreach (var file in directoryInfo.GetFiles()) file.Delete();
            foreach (var dir in directoryInfo.GetDirectories()) dir.Delete(true);
        }

        private void GenerateData(GeneratorParameters period)
        {
            _clientHandler.Generate(period.Clients);
            _architectHandler.Generate(period.Architects, _doneProjectHandler.CurrentDate);
            _outerSubjectHandler.Generate(period.Clients);
            _projectHandler.Generate(period.Projects, _doneProjectHandler.CurrentDate);
            _supervisionHandler.Generate(period.Supervisions);
            _outerProjectHandler.Generate(period.OuterProjects);
            _doneProjectHandler.Generate(0); // amount needed by interface, but here will be calculated on handler's side
        }

        private void WriteToFiles(string time)
        {
            _architectHandler.Write(time);
            _clientHandler.Write(time);
            _outerSubjectHandler.Write(time);
            _outerProjectHandler.Write(time);
            _projectHandler.Write(time);
            _supervisionHandler.Write(time);
            _doneProjectHandler.Write(time);
        }

        private void WriteToBulk(params object[] parameters) { }
    }
}