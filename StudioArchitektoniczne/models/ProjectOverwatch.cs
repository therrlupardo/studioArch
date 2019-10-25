using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class ProjectOverwatch
    {
        public ProjectOverwatch(uint id, DateTime startDate, DateTime dateTime, double prize, uint constructionManagerId, uint architectId, uint projectId)
        {
            this.id = id;
            this.startDate = startDate;
            this.dateTime = dateTime;
            this.prize = prize;
            this.constructionManagerId = constructionManagerId;
            this.architectId = architectId;
            this.projectId = projectId;
        }

        public UInt32 id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime dateTime { get; set; }
        public Double prize { get; set; }
        public UInt32 constructionManagerId { get; set; }
        public UInt32 architectId { get; set; }
        public UInt32 projectId { get; set; }
    }
}
