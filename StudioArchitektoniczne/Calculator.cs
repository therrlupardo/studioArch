using StudioArchitektoniczne.models.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    static class Calculator
    {
        public static Double CalculateProjectCost(DateTime start, DateTime end, ArchitectureTypeEnum type)
        {
            var time = (end - start).Days;
            switch (type)
            {

                case ArchitectureTypeEnum.OBIEKT_BIUROWY:
                    return 4500 * time;
                case ArchitectureTypeEnum.OBIEKT_MIESZKALNY:
                    return 6000 * time;
                case ArchitectureTypeEnum.OBIEKT_USLUGOWY:
                    return 10000 * time;
                default: return 0;
            }
        }

        public static Double CalculateOuterProjectCost(DateTime start, DateTime end, OuterProjectType type)
        {
            var time = (end - start).Days;
            switch (type)
            {
                case OuterProjectType.ALARMOWY:
                    return 50 * time;
                case OuterProjectType.ELEKTRYCZNY:
                    return 75 * time;
                case OuterProjectType.GRZEWCZY:
                    return 80 * time;
                case OuterProjectType.OGRODNICZY:
                    return 25 * time;
                case OuterProjectType.PRZECIWGROMOWY:
                    return 35 * time;
                case OuterProjectType.WODOCIĄGOWY:
                    return 55 * time;
                default: return 0;
            }
        }

        public static Double CalculateOverwatchCost(DateTime start, DateTime end)
        {
            return (end - start).Days * 1500;
        }
    }
}
