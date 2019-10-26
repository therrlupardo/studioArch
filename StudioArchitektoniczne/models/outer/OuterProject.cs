using System;

namespace StudioArchitektoniczne.models.outer
{
    class OuterProject
    {
        public OuterProject(uint id, string name, uint subjectId, string projectType, double prize, DateTime startDate, DateTime endDate, uint projectId)
        {
            this.id = id;
            this.name = name;
            this.subjectId = subjectId;
            this.projectType = projectType;
            this.prize = prize;
            this.startDate = startDate;
            this.endDate = endDate;
            this.projectId = projectId;
        }

        public UInt32 id { get; set; }
        public String name { get; set; }
        public UInt32 subjectId { get; set; }
        public String projectType { get; set; }
        public Double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public UInt32 projectId { get; set; }
    }
}
