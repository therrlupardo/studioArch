using System;
using StudioArchitektoniczne.models.enums;

namespace StudioArchitektoniczne.models
{
    class Project
    {
        public Project(uint id, string address, ArchitectureTypeEnum architectureType, double prize, DateTime startDate, DateTime endDate, ProjectStatusEnum status, uint size, DateTime clientOrderDate, uint clientId)
        {
            this.id = id;
            this.address = address;
            this.architectureType = architectureType;
            this.prize = prize;
            this.startDate = startDate;
            this.endDate = endDate;
            this.status = status;
            this.size = size;
            this.clientOrderDate = clientOrderDate;
            this.clientId = clientId;
        }

        public UInt32 id { get; set; }
        public String address { get; set; }
        public ArchitectureTypeEnum architectureType { get; set; }
        public Double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusEnum status { get; set; }
        public UInt32 size { get; set; }
        public DateTime clientOrderDate { get; set; }
        public UInt32 clientId { get; set; }

    }
}
