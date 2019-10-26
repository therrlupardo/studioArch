using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client
    {
        public Client()
        {
            this.id = Guid.NewGuid();
            this.name = RandomValues.RandomEnumValue<NameEnum>();
            this.surname = RandomValues.RandomEnumValue<SurnameEnum>();
            this.email = RandomValues.RandomEmail();
            this.phone = RandomValues.RandomPhoneNumber();
            this.companyName = RandomValues.RandomCompanyName();
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

        public static Client GenerateRandomClient(Guid id)
        {
            Client client = new Client(id);
            Boolean isCompany = (new Random().Next(2)).Equals(1);
            if (isCompany)
            {
                client.companyName = RandomValueGenerator.GetEnumRandomValue<CompanyNameEnum>().ToString();
                client.email = $"starch@{client.companyName}.com";
            }
            else
            {
                client.name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
                client.surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
                client.email = $"{client.name}.{client.surname}@starch.pl";
                client.companyName = "";

            }
            client.phone = RandomValueGenerator.GetPhoneNumber();
            return client;
        }

        public override string ToString()
        {
            return $"CLIENT: {id}, {name}, {surname}, {companyName}, {email}, {phone}";
        }
    }
}
