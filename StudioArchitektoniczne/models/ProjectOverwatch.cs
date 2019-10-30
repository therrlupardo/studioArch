using System;

namespace StudioArchitektoniczne.models
{
    class ProjectOverwatch : DataModel
    {
        public ProjectOverwatch(int id, int managerId, int architectId, int projectId)
        {
            this.id = id;
            startDate = new DateTime();
            endDate = startDate.AddDays(new Random().Next(4000));
            prize = Calculator.CalculateOverwatchCost(startDate, endDate);
            constructionManagerId = managerId;
            this.architectId = architectId;
            this.projectId = projectId;
        }

        public int id { get; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Decimal prize { get; set; }
        public int constructionManagerId { get; set; }
        public int architectId { get; set; }
        public int projectId { get; set; }

    }
}
