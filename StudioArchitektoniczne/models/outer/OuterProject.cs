using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models.outer
{
    class OuterProject
    {
        public OuterProject(int id, int subjectId, int projectId)
        {
            this.id = id;
            name = $"{projectType}-{new Random().Next(10000)}";
            this.subjectId = subjectId;
            projectType = RandomValueGenerator.GetEnumRandomValue<OuterProjectType>();
            startDate = new DateTime();
            endDate = startDate.AddDays(new Random().Next(2000));
            prize = Calculator.CalculateOuterProjectCost(startDate, endDate, projectType);
            this.projectId = projectId;
        }

        public override string ToString()
        {
            return $"{id};{name};{subjectId};{projectType};{startDate.ToShortDateString()};{endDate.ToShortDateString()};{prize};{projectId}";
        }

        public int id { get; set; }
        public string name { get; set; }
        public int subjectId { get; set; }
        public OuterProjectType projectType { get; set; }
        public double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int projectId { get; set; }
    }
}
