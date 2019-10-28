using System;

namespace StudioArchitektoniczne.models
{
    class ProjectOverwatch
    {
        public ProjectOverwatch(Guid managerId, Guid architectId, Guid projectId)
        {
            this.id = Guid.NewGuid();
            this.startDate = new DateTime();
            this.endDate = startDate.AddDays(new Random().Next(2000));
            this.prize = Calculator.CalculateOverwatchCost(startDate, endDate);
            this.constructionManagerId = managerId;
            this.architectId = architectId;
            this.projectId = projectId;
        }

        public Guid id { get; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Double prize { get; set; }
        public Guid constructionManagerId { get; set; }
        public Guid architectId { get; set; }
        public Guid projectId { get; set; }
    }
}
