using StudioArchitektoniczne.models;
using StudioArchitektoniczne.models.outer;
using System;
using System.Collections.Generic;

namespace StudioArchitektoniczne
{
    class Program
    {

        static void Main(string[] args)
        {
            Generator generator = Generator200k();
            DateTime begin = DateTime.Now;
            generator.GenerateData();
            var time = (DateTime.Now - begin);
            Console.WriteLine($"Zakończono generowanie! Czas: {time}");
            Console.WriteLine($"Wygenerowano {generator.CountGeneratedRecords()} rekordów");
            Console.WriteLine();
        }
    
        private static Generator CustomGenerator()
        {
            Console.WriteLine("Witaj w generatorze danych do studia architektonicznego!");
            Console.WriteLine("Zacznijmy do okresu t0-t1:");
            Console.WriteLine("Ilu klientów ma zostać wygenerowanych?");
            int t0clients = int.Parse(Console.ReadLine());
            Console.WriteLine("Ilu architektów ma zostać wygenerowanych?");
            int t0architects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile projektów ma zostać wygenerowanych?");
            int t0projects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile nadzorów ma zostać wygenerowanych?");
            int t0overwatches = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile zewnętrznych podmiotów ma zostać wygenerowanych?");
            int t0outerSubjects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile zewnętrznych projektów ma zostać wygenerowanych?");
            int t0outerProjects = int.Parse(Console.ReadLine());
            Console.WriteLine("=====================================");
            Console.WriteLine("Przejdźmy do okresu t1-t2:");
            Console.WriteLine("Ilu klientów ma zostać wygenerowanych?");
            int t1clients = int.Parse(Console.ReadLine());
            Console.WriteLine("Ilu architektów ma zostać wygenerowanych?");
            int t1architects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile projektów ma zostać wygenerowanych?");
            int t1projects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile nadzorów ma zostać wygenerowanych?");
            int t1overwatches = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile zewnętrznych podmiotów ma zostać wygenerowanych?");
            int t1outerSubjects = int.Parse(Console.ReadLine());
            Console.WriteLine("Ile zewnętrznych projektów ma zostać wygenerowanych?");
            int t1outerProjects = int.Parse(Console.ReadLine());
            Console.WriteLine("====================================");
            Console.WriteLine("Teraz zostaną wygenerowane dane. Proszę czekać");
            return new Generator(t0clients, t0architects, t0projects, t0overwatches, t0outerProjects, t0outerSubjects,
                                 t1clients, t1architects, t1projects, t1overwatches, t1outerProjects, t1outerSubjects);
        }

        private static Generator Generator100k()
        {
            return new Generator(200, 30000, 10000, 2000, 2000, 200, 100, 15000, 5000, 1000, 1000, 100);
        }

        private static Generator Generator200k()
        {
            return new Generator(400, 60000, 20000, 4000, 4000, 400, 200, 30000, 10000, 2000, 2000, 200);
        }
    }

}
