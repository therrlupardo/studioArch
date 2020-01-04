using System;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio
{
    public class Program
    {

        public static void Main()
        { 
            var generator = new Generator(GeneratorSize.SMALL);
            generator.Run();
        }
    
        private static Generator CustomGenerator()
        { 
            Console.WriteLine(Resources.Program_CustomGenerator_Witaj_w_generatorze_danych_do_studia_architektonicznego_);

            Console.WriteLine(Resources.Program_CustomGenerator_Zacznijmy_do_okresu_t0_t1_);
            var firstPeriod = CreateGeneratorParameters();
            
            Console.WriteLine(Resources.Program_CustomGenerator_Przejdźmy_do_okresu_t1_t2_);
            var secondPeriod = CreateGeneratorParameters();

            Console.WriteLine(Resources.Program_CustomGenerator_Teraz_zostaną_wygenerowane_dane__Proszę_czekać);
            return new Generator(firstPeriod, secondPeriod);
        }

        private static GeneratorParameters CreateGeneratorParameters()
        {
            var parameters = new GeneratorParameters
            {
                Clients = ReadInput(Resources.Program_CustomGenerator_Ilu_klientów_ma_zostać_wygenerowanych_),
                Architects = ReadInput(Resources.Program_CustomGenerator_Ilu_architektów_ma_zostać_wygenerowanych_),
                Projects = ReadInput(Resources.Program_CustomGenerator_Ile_projektów_ma_zostać_wygenerowanych_),
                Supervisions = ReadInput(Resources.Program_CustomGenerator_Ile_nadzorów_ma_zostać_wygenerowanych_),
                OuterProjects = ReadInput(Resources.Program_CustomGenerator_Ile_zewnętrznych_podmiotów_ma_zostać_wygenerowanych_),
                OuterSubjects = ReadInput(Resources.Program_CustomGenerator_Ile_zewnętrznych_projektów_ma_zostać_wygenerowanych_)
            };
            Console.WriteLine(Resources.Program_CustomGenerator_BreakLine);
            return parameters;
        }

        private static int ReadInput(string message)
        {
            Console.WriteLine(message);
            return int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        }
    }

}
