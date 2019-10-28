using System;
using StudioArchitektoniczne.models.enums;

namespace StudioArchitektoniczne.models
{
    class Project
    {
        public Project(DateTime clientOrderDate, Guid clientId)
        {
            this.id = Guid.NewGuid();
            this.address = new Address().ToString();
            this.architectureType = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            this.startDate = new DateTime();
            this.endDate = startDate.AddDays(new Random().Next(2000));
            this.prize = Calculator.CalculateProjectCost(startDate, endDate, architectureType);
            this.status = ProjectStatusEnum.PRZYJETO_DO_REALIZACJI;
            this.size = (uint)(endDate-startDate).Days;
            this.clientOrderDate = clientOrderDate;
            this.clientId = clientId;
        }

        public override string ToString()
        {
            return $"{id};{address};{architectureType.ToString()};{startDate.ToShortDateString()};" +
                $"{endDate.ToShortDateString()};{prize};{status.ToString()};{size};{clientOrderDate.ToShortDateString()};{clientId};";
        }

        public Guid id { get; }
        public String address { get; set; }
        public ArchitectureTypeEnum architectureType { get; set; }
        public Double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusEnum status { get; set; }
        public UInt32 size { get; set; }
        public DateTime clientOrderDate { get; set; }
        public Guid clientId { get; set; }

    }
}
