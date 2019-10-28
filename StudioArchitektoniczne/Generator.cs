using StudioArchitektoniczne.models;
using StudioArchitektoniczne.models.outer;
using System;
using System.Collections.Generic;

namespace StudioArchitektoniczne
{
    class Generator
    {
        int t0clients, t0architects, t0projects, t0overwatches, t0outerProjects, t0outerSubjects;
        int t1clients, t1architects, t1projects, t1overwatches, t1outerProjects, t1outerSubjects;

        List<Architect> listOfArchitects = new List<Architect>();
        List<Client> listOfClients = new List<Client>();
        List<Project> listOfProjects = new List<Project>();
        List<ProjectOverwatch> listOfOverwatches = new List<ProjectOverwatch>();
        List<OuterProject> listOfOuterProjects = new List<OuterProject>();
        List<OuterSubject> listOfOuterSubjects = new List<OuterSubject>();

        public Generator(int t0clients, int t0architects, int t0projects, int t0overwatches, int t0outerProjects,
            int t0outerSubjects, int t1clients, int t1architects, int t1projects,
            int t1overwatches, int t1outerProjects, int t1outerSubjects)
        {
            this.t0clients = t0clients;
            this.t0architects = t0architects;
            this.t0projects = t0projects;
            this.t0overwatches = t0overwatches;
            this.t0outerProjects = t0outerProjects;
            this.t0outerSubjects = t0outerSubjects;
            this.t1clients = t1clients;
            this.t1architects = t1architects;
            this.t1projects = t1projects;
            this.t1overwatches = t1overwatches;
            this.t1outerProjects = t1outerProjects;
            this.t1outerSubjects = t1outerSubjects;
        }

        public void GenerateData()
        {
            GenerateT0InitData();
            MutateT0Data();
            GenerateT1InitData();
            Console.WriteLine();

            // --- start t1


            //while .....

            // client order date -> rand (t0, t1> 

            // project
            //  rand   ->   client id    architect(s) id         

            // projectoverwatch
            //  rand   ->   start date   ( > project end date )

            // outerprojects 
            // if(rand ...)
            //  rand         subject id  start date ( > project end date  &&  < overwatch start date)

            // if(rand ...)
            // create clients, architects, outer subjects ...

            // end while


            // --- end t1


        }

        private void GenerateT0InitData()
        {
            for (int i = 0; i < t0clients; i++) listOfClients.Add(new Client());
            for (int i = 0; i < t0architects; i++) listOfArchitects.Add(new Architect());
            for (int i = 0; i < t0outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject());
        }

        private void MutateT0Data()
        {
            // todo: zmienić losowe dane na inne (tylko architekci mogą być zmienieni żeby to miało sens?)
        }

        private void GenerateT1InitData()
        {
            for (int i = 0; i < t1clients; i++) listOfClients.Add(new Client());
            for (int i = 0; i < t1architects; i++) listOfArchitects.Add(new Architect());
            for (int i = 0; i < t1outerSubjects; i++) listOfOuterSubjects.Add(new OuterSubject());
        }

    }
}