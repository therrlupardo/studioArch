using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client
    {
        public Client(int id)
        {
            this.id = id;
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            phone = RandomValueGenerator.GetPhoneNumber();
            companyName = new Random().NextDouble() < 0.6 ? RandomValueGenerator.GetRandomCompanyName(this) : "";
            email = RandomValueGenerator.GetRandomEmail(this);
        }

        public int id { get; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string companyName { get; set; }

        public override string ToString()
        {
            return $"CLIENT: {id}, {name}, {surname}, {companyName}, {email}, {phone}";
        }
    }
}
