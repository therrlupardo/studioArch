using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio.models.outer
{
    public class OuterProject : DataModel
    {
        public OuterProject(int id, int subjectId, int projectId)
        {
            Id = id;
            ProjectId = projectId;
            SubjectId = subjectId;
            InitRandomValues();
        }

        private void InitRandomValues()
        {
            Name = $"{ProjectType}-{new Random().Next(10000)}";
            ProjectType = RandomValueGenerator.GetEnumRandomValue<OuterProjectType>();
            StartDate = new DateTime();
            EndDate = StartDate.AddDays(new Random().Next(2000));
            Prize = Calculator.CalculateOuterProjectCost(StartDate, EndDate, ProjectType);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public OuterProjectType ProjectType { get; set; }
        public decimal Prize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
    }
}
