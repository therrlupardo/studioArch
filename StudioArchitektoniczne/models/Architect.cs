using System;

namespace StudioArchitektoniczne.models
{
    class Architect
    {
        public Architect(string specialization, string name, string surname, DateTime birthDate, string phone, bool canOverwatch, uint contractId)
        {
            this.id = Guid.NewGuid();
            this.specialization = specialization;
            this.name = name;
            this.surname = surname;
            this.birthDate = birthDate;
            this.phone = phone;
            this.canOverwatch = canOverwatch;
            this.contractId = contractId;
        }

        public Guid id { get; }
        public String specialization { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public DateTime birthDate { get; set; }
        public String phone { get; set; }
        public Boolean canOverwatch { get; set; }
        public UInt32 contractId { get; set; }

        private String GetCanOverwatchString()
        {
            return canOverwatch ? "TAK" : "NIE";
        }
    }
}
