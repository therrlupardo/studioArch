using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio
{
    public static class Calculator
    {
        public static decimal CalculateProjectCost(DateTime start, DateTime end, ArchitectureTypeEnum type)
        {
            var time = (end - start).Days;
            return type switch
            {
                ArchitectureTypeEnum.OBIEKT_BIUROWY => (4500 * time),
                ArchitectureTypeEnum.OBIEKT_MIESZKALNY => (6000 * time),
                ArchitectureTypeEnum.OBIEKT_USLUGOWY => (10000 * time),
                _ => 0
            };
        }

        public static decimal CalculateOuterProjectCost(DateTime start, DateTime end, OuterProjectType type)
        {
            var time = (end - start).Days;
            return type switch
            {
                OuterProjectType.ALARMOWY => (50 * time),
                OuterProjectType.ELEKTRYCZNY => (75 * time),
                OuterProjectType.GRZEWCZY => (80 * time),
                OuterProjectType.OGRODNICZY => (25 * time),
                OuterProjectType.PRZECIWGROMOWY => (35 * time),
                OuterProjectType.WODOCIĄGOWY => (55 * time),
                _ => 0
            };
        }

        public static decimal CalculateSupervisionCost(DateTime start, DateTime end)
        {
            return (end - start).Days * 1500;
        }
    }
}
