using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Architect : DataModel
    {
        public Architect() { }
        public Architect(int id, DateTime dataWstawienia, DateTime dataWygasniecia)
        {
            this.id = id;
            specialization = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            birthDate = RandomValueGenerator.GetRandomBirthDate();
            phone = RandomValueGenerator.GetPhoneNumber();
            contractId = new Random().Next();
            canOverwatch = new Random().Next() % 2 == 0;
            pesel = RandomValueGenerator.GetPesel();
            this.dataWstawienia = dataWstawienia;
            this.dataWygasniecia = dataWygasniecia;
            active = true;
            idPrzelozonego = -2; // -1 is top supervisor, -2 is none (initial value)
        }

        public int id { get; set; }
        public ArchitectureTypeEnum specialization { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public DateTime birthDate { get; set; }
        public String phone { get; set; }
        public int contractId { get; set; }
        public bool canOverwatch { get; set; }
        public string pesel { get; set; }
        public int idPrzelozonego { get; set; }
        public DateTime dataWstawienia { get; set; }
        public DateTime dataWygasniecia { get; set; }
        public Boolean active { get; set; }

        private String GetCanOverwatchString()
        {
            return canOverwatch ? "UPRAWNIONY" : "NIEUPRAWNIONY";
        }

        public override string ToCsvString()
        {
            return $"{id},{name},{surname},{DataModel.ConvertDateToDDMMYYYY(birthDate)},{phone},{contractId},{GetCanOverwatchString()},{pesel}";
        }

        public override string ToBulkString()
        {
            return $"{id}|{specialization}|{idPrzelozonego}";
        }

        public Architect Copy()
        {
//            Architect architect = new Architect(id, dataWstawienia, dataWygasniecia);
//            architect.id = id;
//            architect.active = active;
//            architect.dataWstawienia = dataWstawienia;
//            architect.dataWygasniecia = dataWygasniecia;
//            architect.canOverwatch = canOverwatch;
//            architect.idPrzelozonego = idPrzelozonego;
//            architect.name = name;
//            architect.pesel = pesel;
//            architect.specialization = specialization;
//            architect.surname = surname;
            Architect architect = new Architect()
            {
                id = this.id,
                active = this.active,
                dataWstawienia = this.dataWstawienia,
                dataWygasniecia = this.dataWygasniecia,
                canOverwatch = this.canOverwatch,
                idPrzelozonego = this.idPrzelozonego,
                name = this.name,
                pesel = this.pesel,
                specialization = this.specialization,
                surname = this.surname,
                contractId = this.contractId,
                birthDate = this.birthDate,
                phone = this.phone
            };
            return architect;
        }
    }
}
