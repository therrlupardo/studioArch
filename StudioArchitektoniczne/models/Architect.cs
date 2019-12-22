using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio.models
{
    public class Architect : AbstractDataModel
    {
        private const string Authorized = "UPRAWNIONY";
        private const string Unauthorized = "NIEUPRAWNIONY";

        public Architect() { }
        public Architect(int id, DateTime insertDate, DateTime expirationDate)
        {
            Id = id;
            InsertDate = insertDate;
            ExpirationDate = expirationDate;
            Active = true;
            PrincipalId = -2; // -1 is top supervisor, -2 is none (initial value)
            InitRandomValues();
        }

        private void InitRandomValues()
        {
            Specialization = RandomValueGenerator.GetEnumRandomValue<ArchitectureTypeEnum>();
            Name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            Surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            BirthDate = RandomValueGenerator.GetRandomBirthDate();
            Phone = RandomValueGenerator.GetPhoneNumber();
            ContractId = new Random().Next();
            CanSupervise = new Random().Next() % 2 == 0;
            Pesel = RandomValueGenerator.GetPesel();
        }

        public int Id { get; set; }
        public ArchitectureTypeEnum Specialization { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public int ContractId { get; set; }
        public bool CanSupervise { get; set; }
        public string Pesel { get; set; }
        public int PrincipalId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Active { get; set; }

        private string CanSuperviseToString()
        {
            return CanSupervise ? Authorized : Unauthorized;
        }

        private string PrincipalToString()
        {
            return PrincipalId == -1 ? "" : PrincipalId.ToString();
        }

        public override string ToCsv()
        {
            return $"{Id},{Name},{Surname},{ConvertDate(BirthDate)},{Phone},{ContractId},{CanSuperviseToString()},{Pesel}";
        }

        public override string ToBulk()
        {
            return $"{Id}|{Specialization}|{PrincipalToString()}";
        }

        public Architect Copy()
        {
            var architect = new Architect
            {
                Id = Id,
                Active = Active,
                InsertDate = InsertDate,
                ExpirationDate = ExpirationDate,
                CanSupervise = CanSupervise,
                PrincipalId = PrincipalId,
                Name = Name,
                Pesel = Pesel,
                Specialization = Specialization,
                Surname = Surname,
                ContractId = ContractId,
                BirthDate = BirthDate,
                Phone = Phone
            };
            return architect;
        }
    }
}
