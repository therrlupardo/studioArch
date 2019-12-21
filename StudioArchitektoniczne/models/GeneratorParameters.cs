namespace ArchitecturalStudio.models
{
    public class GeneratorParameters
    {
        public int Clients { get; set; }
        public int Architects { get; set; }
        public int Projects { get; set; }
        public int Supervisions { get; set; }
        public int OuterProjects { get; set; }
        public int OuterSubjects { get; set; }

        public GeneratorParameters()
        {
            
        }
        public GeneratorParameters(int clients, int architects, int projects, int supervisions, int outerProjects, int outerSubjects)
        {
            Clients = clients;
            Architects = architects;
            Projects = projects;
            Supervisions = supervisions;
            OuterProjects = outerProjects;
            OuterSubjects = outerSubjects;
        }
    }
}