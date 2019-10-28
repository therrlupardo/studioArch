using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Architect
    {
        public Architect()
        {
            this.id = Guid.NewGuid();
            this.specialization = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>().ToString();
            this.name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            this.surname = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            this.birthDate = RandomValueGenerator.GetRandomBirthDate();
            this.phone = RandomValueGenerator.GetPhoneNumber();
            this.canOverwatch = new Random().Next() % 2 == 0;
            this.contractId = (uint) new Random().Next();
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

        public override string ToString()
        {
            return $"{id};{specialization};{name};{surname};{birthDate};{phone};{GetCanOverwatchString()};{contractId}";
        }
    }
}
