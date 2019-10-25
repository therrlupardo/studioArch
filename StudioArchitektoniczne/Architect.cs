using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class Architect
    {
        public UInt32 id { get; set; }
        public String specialization { get; set; }
        public String name { get; set; }
        public String  surname { get; set; }
        public DateTime birthDate { get; set; }
        public String phone { get; set; }
        public Boolean canOverwatch { get; set; }
        public UInt32 contractId { get; set; }
    }
}
