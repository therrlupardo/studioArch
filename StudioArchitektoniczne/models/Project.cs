using System;
using StudioArchitektoniczne.models.enums;

namespace StudioArchitektoniczne.models
{
    class Project
    {
        public Project(int id, DateTime clientOrderDate, int clientId)
        {
            this.id = id;
            address = new Address().ToString();
            architectureType = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            startDate = new DateTime();
            endDate = startDate.AddDays(new Random().Next(4000));
            prize = Calculator.CalculateProjectCost(startDate, endDate, architectureType);
            status = ProjectStatusEnum.PRZYJETO_DO_REALIZACJI;
            size = (int)(endDate-startDate).Days;
            this.clientOrderDate = clientOrderDate;
            this.clientId = clientId;
        }

        public override string ToString()
        {
            return $"{id};{address};{architectureType.ToString()};{startDate.ToShortDateString()};" +
                $"{endDate.ToShortDateString()};{prize};{status.ToString()};{size};{clientOrderDate.ToShortDateString()};{clientId};";
        }

        public int id { get; }
        public string address { get; set; }
        public ArchitectureTypeEnum architectureType { get; set; }
        public double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusEnum status { get; set; }
        public int size { get; set; }
        public DateTime clientOrderDate { get; set; }
        public int clientId { get; set; }

        public void updateSize()
        {
            size = (int)(endDate - startDate).Days;
        }
    }
}
