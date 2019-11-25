using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Architect : DataModel
    {
        public Architect(int id, DateTime dataWstawienia, DateTime dataWygasniecia)
        {
            this.id = id;
            specialization = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            canOverwatch = new Random().Next() % 2 == 0;
            pesel = RandomValueGenerator.GetPesel();
            this.dataWstawienia = dataWstawienia;
            this.dataWygasniecia = dataWygasniecia;
        }

        public int id { get; }
        public ArchitectureTypeEnum specialization { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public bool canOverwatch { get; set; }
        public string pesel { get; set; }
        public int idPrzelozonego { get; set; }
        public DateTime dataWstawienia { get; set; }
        public DateTime dataWygasniecia { get; set; }

        private String GetCanOverwatchString()
        {
            return canOverwatch ? "UPRAWNIONY" : "NIEUPRAWNIONY";
        }

        public override string ToCsvString()
        {
            return $"{id},{name},{surname},{pesel}, {idPrzelozonego}, {GetCanOverwatchString()},{dataWstawienia},{dataWygasniecia}";
        }

        public override string ToBulkString()
        {
            return $"{id}|{specialization}";
        }
    }
}
