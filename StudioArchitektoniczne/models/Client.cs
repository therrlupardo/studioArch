using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Client
    {
        public Client()
        {

        }

        public Client(uint id)
        {
            this.id = id;
        }

        public Client(uint id, String name, String surname, string email, string phone, string companyName)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.phone = phone;
            this.companyName = companyName;
        }

        public uint id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String companyName { get; set; }

        public static Client GenerateRandomClient(uint id)
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
