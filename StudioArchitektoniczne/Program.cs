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
            List<Architect> listOfArchitects = new List<Architect>();
            List<Client> listOfClients = new List<Client>();
            List<Project> listOfProjects = new List<Project>();
            List<ProjectOverwatch> listOfOverwatches = new List<ProjectOverwatch>();
            List<OuterProject> listOfOuterProjects = new List<OuterProject>();
            List<OuterSubject> listOfOuterSubjects = new List<OuterSubject>();

            int number;
            Console.WriteLine("Ilu klientów potrzeba?");
            number = Convert.ToInt32(Console.ReadLine());
            for (uint i = 0; i < number; i++)
            {
                listOfClients.Add(Client.GenerateRandomClient(i));
            }

            listOfClients.ForEach(client => Console.WriteLine(client));

        }
    }
}
