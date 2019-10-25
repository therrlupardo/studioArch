using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class OuterProjects
    {
        public UInt32 id { get; set; }
        public String name { get; set; }
        public UInt32 subjectId { get; set; }
        public String projectType { get; set; }
        public Double prize { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public UInt32 projectId { get; set; }
    }
}
