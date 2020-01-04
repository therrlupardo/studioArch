using System;
using ArchitecturalStudio.models.enums;
using static ArchitecturalStudio.models.enums.StatusEnum;

namespace ArchitecturalStudio.models
{
    public class ProjectSupervision : AbstractDataModel
    {
        public ProjectSupervision(int id, int managerId, int architectId, int projectId)
        {
            Id = id;
            ConstructionManagerId = managerId;
            ArchitectId = architectId;
            ProjectId = projectId;
            StartDate = new DateTime();
            EndDate = StartDate.AddDays(new Random().Next(300));
            Prize = Calculator.CalculateSupervisionCost(StartDate, EndDate);
            Status = PRZYJETO_DO_REALIZACJI;
        }

        public int Id { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Prize { get; set; }
        public int ConstructionManagerId { get; set; }
        public int ArchitectId { get; set; }
        public int ProjectId { get; set; }
        public int EndDelay { get; set; }
        public int Size { get; set; }
        public LengthEnum Length { get; set; }
        public StatusEnum Status { get; set; }

        public void RecalculateFields()
        {
            EndDelay = (EndDate - StartDate).Days;
            Size = EndDelay * 8;
            RecalculateLength();
        }

        private void RecalculateLength()
        {
            if (Size <= 60) Length = LengthEnum.BARDZO_KROTKI;
            else if (Size <= 120) Length = LengthEnum.KROTKI;
            else if (Size <= 180) Length = LengthEnum.SREDNI;
            else if (Size <= 240) Length = LengthEnum.DLUGI;
            else Length = LengthEnum.BARDZO_DLUGI;
        }

        public override string ToBulk()
        {
            return $"{Id}|{Size}|{Prize}|{ConvertDate(StartDate)}|{ConvertDate(EndDate)}|{ArchitectId}|{ConstructionManagerId}|{ProjectId}";
        }
    }
}
