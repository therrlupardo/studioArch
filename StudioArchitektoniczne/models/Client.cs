using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client : DataModel
    {
        public Client(int id)
        {
            this.id = id;
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
//            this.phone = RandomValueGenerator.GetPhoneNumber();
            companyName = new Random().NextDouble() < 0.6 ? RandomValueGenerator.GetRandomCompanyName(this) : "";
//            this.email = RandomValueGenerator.GetRandomEmail(this);
            clientType = companyName.Length == 0 ? ClientTypeEnum.INDYWIDUALNY : ClientTypeEnum.ORGANIZACJA;
        }

        public int id { get; }
        public string name { get; set; }
        public string surname { get; set; }
        public string companyName { get; set; }
        public ClientTypeEnum clientType { get; set; }
//        public string phone { get; set; }
//        public string email { get; set; }
    }
}
