using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client
    {
        public Client(int id)
        {
            this.id = id;
            this.name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            this.surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            this.phone = RandomValueGenerator.GetPhoneNumber();
            this.companyName = new Random().NextDouble() < 0.6 ? RandomValueGenerator.GetRandomCompanyName(this) : "";
            this.email = RandomValueGenerator.GetRandomEmail(this);
        }

        public int id { get; }
        public string name { get; set; }
        public string surname { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
