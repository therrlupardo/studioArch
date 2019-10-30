using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Architect : DataModel
    {
        public Architect(int id)
        {
            this.id = id;
            specialization = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            birthDate = RandomValueGenerator.GetRandomBirthDate();
            phone = RandomValueGenerator.GetPhoneNumber();
            canOverwatch = new Random().Next() % 2 == 0;
            contractId =  new Random().Next();
        }

        public int id { get; }
        public ArchitectureTypeEnum specialization { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public DateTime birthDate { get; set; }
        public String phone { get; set; }
        public bool canOverwatch { get; set; }
        public int contractId { get; set; }

        private String GetCanOverwatchString()
        {
            return canOverwatch ? "TAK" : "NIE";
        }

        public override string ToCsvString()
        {
            return $"{id},{name},{surname},{birthDate},{phone},{contractId},{GetCanOverwatchString()},";
        }

        public override string ToBulkString()
        {
            return $"{id},{specialization}";
        }
    }
}
