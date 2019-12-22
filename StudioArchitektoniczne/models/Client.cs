using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio.models
{
    public class Client : AbstractDataModel
    {
        public Client(int id)
        {
            Id = id;
            Name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            Surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            CompanyName = new Random().NextDouble() < 0.6 ? RandomValueGenerator.GetRandomCompanyName(this) : "";
            ClientType = CompanyName.Length == 0 ? ClientTypeEnum.INDYWIDUALNY : ClientTypeEnum.ORGANIZACJA;
        }

        public int Id { get; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public ClientTypeEnum ClientType { get; set; }

        public override string ToBulk()
        {
            return $"{Id}|{CompanyName}|{Name}|{Surname}|{ClientType}";
        }
    }
}
