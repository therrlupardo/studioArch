using StudioArchitektoniczne.models;
using StudioArchitektoniczne.models.enums;
using StudioArchitektoniczne.models.enums.company;
using System;
using System.ComponentModel;
using System.Linq;

namespace StudioArchitektoniczne
{
    class RandomValueGenerator
    {
        public static T GetEnumRandomValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        public static String GetPhoneNumber()
        {
            String phone = "";
            for (int i = 0; i < 9; i++)
            {
                phone += (new Random().Next(10)).ToString();
                if (phone.Equals("0"))
                {
                    phone = (new Random().Next(1, 10)).ToString();
                }
            }
            return phone;
        }

        public static string GetRandomEmail(Client client)
        {
            string email = client.companyName.Any() ? client.companyName : $"{client.name}.{client.surname}";
            email += $"@{GetEnumDescription(GetEnumRandomValue<DomainsEnum>())}";
            return email.ToLower();
        }

        public static string GetRandomCompanyName(Client client)
        {
            return $"{client.name.Substring(0, 3).ToString()}{client.surname.Substring(0, 3).ToString()}{GetEnumDescription(RandomValueGenerator.GetEnumRandomValue<CompanyMidNameEnum>())}";
        }

        private static string GetEnumDescription(Enum value)
        {
            var info = value.GetType().GetField(value.ToString());
            var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }
            return value.ToString();
        }

    }
}
