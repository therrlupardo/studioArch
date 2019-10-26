using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client
    {
        public Client()
        {
            this.id = Guid.NewGuid();
            this.name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            this.surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            this.phone = RandomValueGenerator.GetPhoneNumber();
            this.companyName = new Random().NextDouble() < 0.6 ? RandomValueGenerator.GetRandomCompanyName(this) : "";
            this.email = RandomValueGenerator.GetRandomEmail(this);
        }

        public Client(Guid id) {
            this.id = id;
        }

        public Guid id { get; }
        public String name { get; set; }
        public String surname { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String companyName { get; set; }

        public override string ToString()
        {
            return $"CLIENT: {id}, {name}, {surname}, {companyName}, {email}, {phone}";
        }
    }
}
