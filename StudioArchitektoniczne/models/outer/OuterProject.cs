using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models.outer
{
    class OuterProject
    {
        public OuterProject(Guid subjectId, Guid projectId)
        {
            this.id = Guid.NewGuid();
            this.name = $"{projectType}-{new Random().Next(10000)}";
            this.subjectId = subjectId;
            this.projectType = RandomValueGenerator.GetEnumRandomValue<OuterProjectType>();
            this.startDate = new DateTime();
            this.endDate = startDate.AddDays(new Random().Next(2000));
            this.prize = Calculator.CalculateOuterProjectCost(startDate, endDate, projectType);
            this.projectId = projectId;
        }

        public override string ToString()
        {
            return $"{id};{name};{subjectId};{projectType};{startDate.ToShortDateString()};{endDate.ToShortDateString()};{prize};{projectId}";
        }

        public Guid id { get; set; }
        public String name { get; set; }
        public Guid subjectId { get; set; }
        public OuterProjectType projectType { get; set; }
        public Double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Guid projectId { get; set; }
    }
}
