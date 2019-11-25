using System;
using System.ComponentModel.DataAnnotations;
using StudioArchitektoniczne.models.enums;

namespace StudioArchitektoniczne.models
{
    class Project : DataModel
    {
        public Project(int id, DateTime clientOrderDate, int clientId)
        {
            this.id = id;
//            address = new Address().ToString();
            architectureType = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            startDate = new DateTime();
            endDate = startDate.AddDays(new Random().Next(40000));
            prize = Calculator.CalculateProjectCost(startDate, endDate, architectureType);
            size = (endDate-startDate).Days;
            this.clientOrderDate = clientOrderDate;
            this.clientId = clientId;
        }

        public int id { get; }
//        public string address { get; set; }
        public ArchitectureTypeEnum architectureType { get; set; }
        public Decimal prize { get; set; }
        public DateTime clientOrderDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
//        public ProjectStatusEnum status { get; set; }
        public int size { get; set; }
        public int clientId { get; set; }
        public int startDelay { get; set; }
        public int endDelay { get; set; }
        public Decimal totalPrize { get; set; }
        public bool isOverwatched { get; set; }
        public LengthEnum length { get; set; }
        public OverwatchAccumulation overwatchAccumulation { get; set; }

        public void updateSize()
        {
            size = (endDate - startDate).Days * 8;
            startDelay = (startDate - clientOrderDate).Days;
            endDelay = (endDate - clientOrderDate).Days;
            if (size <= 60) length = LengthEnum.BARDZO_KROTKI;
            else if (size <= 120) length = LengthEnum.KROTKI;
            else if (size <= 180) length = LengthEnum.SREDNI;
            else if (size <= 240) length = LengthEnum.DLUGI;
            else length = LengthEnum.BARDZO_DLUGI;
        }
    }
}
