using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class Project
    {
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
