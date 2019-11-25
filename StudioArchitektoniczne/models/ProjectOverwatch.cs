using System;
using StudioArchitektoniczne.models.enums;

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
        public int endDelay { get; set; }
        public int size { get; set; }
        public LengthEnum length { get; set; }

        public void updateLength()
        {
            endDelay = (endDate - startDate).Days;
            size = endDelay * 8;
            if (size <= 60) length = LengthEnum.BARDZO_KROTKI;
            else if (size <= 120) length = LengthEnum.KROTKI;
            else if (size <= 180) length = LengthEnum.SREDNI;
            else if (size <= 240) length = LengthEnum.DLUGI;
            else length = LengthEnum.BARDZO_DLUGI;
        }
    }
}
