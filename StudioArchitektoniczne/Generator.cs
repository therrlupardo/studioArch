using System;
using System.Collections.Generic;

namespace StudioArchitektoniczne
{
    class Generator
    {
        DateTime t0;
        DateTime t1;
        DateTime t2;
        int t1RecordsNumber;
        int t2RecordsNumber;

        List<Architect> listOfArchitects = new List<Architect>();
        List<Client> listOfClients = new List<Client>();
        List<Project> listOfProjects = new List<Project>();
        List<ProjectOverwatch> listOfOverwatches = new List<ProjectOverwatch>();
        List<OuterProject> listOfOuterProjects = new List<OuterProject>();
        List<OuterSubject> listOfOuterSubjects = new List<OuterSubject>();

        public Generator(DateTime t0, DateTime t1, DateTime t2, int t1RecordsNumber, int t2RecordsNumber)
        {
            this.t0 = t0;
            this.t1 = t1;
            this.t2 = t2;
            this.t1RecordsNumber = t1RecordsNumber;
            this.t2RecordsNumber = t2RecordsNumber;
        }

        public void GenerateData()
        {
            //InitGenerate();


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

        private void InitGenerate()
        {
            // create Clients and Architects
            Random rand = new Random();

            for (int i = 0; i < t1RecordsNumber / 1000; i++)
            { 
                if (rand.Next(0, 1) < 0.65)
                {
                    //generate Client
                }

                if (rand.Next(0, 1) < 0.3)
                {
                    //generate Architect
                }

            }

            //create OuterSubjects

        }

    }
}