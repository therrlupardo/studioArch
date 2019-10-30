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
            endDate = startDate.AddDays(new Random().Next(40000));
            prize = Calculator.CalculateProjectCost(startDate, endDate, architectureType);
            status = ProjectStatusEnum.PRZYJETO_DO_REALIZACJI;
            size = (int)(endDate-startDate).Days;
            this.clientOrderDate = clientOrderDate;
            this.clientId = clientId;
        }

        public int id { get; }
        public string address { get; set; }
        public ArchitectureTypeEnum architectureType { get; set; }
        public Decimal prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusEnum status { get; set; }
        public int size { get; set; }
        public DateTime clientOrderDate { get; set; }
        public int clientId { get; set; }

        public void updateSize()
        {
            size = (endDate - startDate).Days;
        }
    }
}
