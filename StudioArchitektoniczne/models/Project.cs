using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio.models
{
    public class Project : DataModel
    {

        private const string Supervised = "NADZOROWANO";
        private const string NotSupervised = "NIENADZOROWANO";

        public Project(int id, DateTime clientOrderDate, int clientId)
        {
            Id = id;
            ClientOrderDate = clientOrderDate;
            ClientId = clientId;
            ArchitectureType = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            StartDate = new DateTime();
            EndDate = StartDate.AddDays(new Random().Next(300));
            Prize = Calculator.CalculateProjectCost(StartDate, EndDate, ArchitectureType);
            TotalPrize = Prize;
            Size = (EndDate-StartDate).Days;
            Status = ProjectStatusEnum.PRZYJETO_DO_REALIZACJI;
        }

        public int Id { get; set; }
        public ArchitectureTypeEnum ArchitectureType { get; set; }
        public decimal Prize { get; set; }
        public DateTime ClientOrderDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatusEnum Status { get; set; }
        public int Size { get; set; }
        public int ClientId { get; set; }
        public int StartDelay { get; set; }
        public int EndDelay { get; set; }
        public decimal TotalPrize { get; set; }
        public bool IsSupervised { get; set; }
        public LengthEnum Length { get; set; }
        public SupervisionAccumulation SupervisionAccumulation { get; set; }
        public int CurrentSupervisions { get; set; }

        public void Update(int supervisions)
        {
            Size = (EndDate - StartDate).Days * 8;
            StartDelay = (StartDate - ClientOrderDate).Days;
            EndDelay = (EndDate - ClientOrderDate).Days;
            CurrentSupervisions = supervisions;
            UpdateLength();
            UpdateSupervisionAccumulation();
        }

        private void UpdateLength()
        {
            if (Size <= 60) Length = LengthEnum.BARDZO_KROTKI;
            else if (Size <= 120) Length = LengthEnum.KROTKI;
            else if (Size <= 180) Length = LengthEnum.SREDNI;
            else if (Size <= 240) Length = LengthEnum.DLUGI;
            else Length = LengthEnum.BARDZO_DLUGI;
        }

        private void UpdateSupervisionAccumulation()
        {
            if (CurrentSupervisions < 10) SupervisionAccumulation = SupervisionAccumulation.MALE;
            else if (CurrentSupervisions < 20) SupervisionAccumulation = SupervisionAccumulation.SREDNIE;
            else SupervisionAccumulation = SupervisionAccumulation.DUZE;
        }

        private string IsSupervisedToString()
        {
            return IsSupervised ? Supervised : NotSupervised;
        }

        public override string ToBulk()
        {
            return $"{Id}|{Size}|{Prize}|{TotalPrize}|{ArchitectureType}|{IsSupervisedToString()}|{CurrentSupervisions}|{ConvertDate(ClientOrderDate)}|{ConvertDate(StartDate)}|{ConvertDate(EndDate)}|{ClientId}";
        }

    }
}
