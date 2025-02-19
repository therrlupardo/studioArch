﻿using System;
using System.ComponentModel;
using System.Linq;
using ArchitecturalStudio.models;
using ArchitecturalStudio.models.enums;
using ArchitecturalStudio.models.enums.company;

namespace ArchitecturalStudio
{
    public class RandomValueGenerator
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
            string email = client.CompanyName.Any() ? client.CompanyName : $"{client.Name}.{client.Surname}";
            email += $"@{GetEnumDescription(GetEnumRandomValue<DomainsEnum>())}";
            return email.ToLower();
        }

        public static string GetRandomCompanyName(Client client)
        {
            return $"{client.Name.Substring(0, 3).ToString()}{client.Surname.Substring(0, 3).ToString()}{GetEnumDescription(RandomValueGenerator.GetEnumRandomValue<CompanyMidNameEnum>())}";
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

        public static DateTime GetRandomBirthDate()
        {
            DateTime start = new DateTime(1955, 1, 1);
            DateTime end = new DateTime(1990, 12, 31);
            return start.AddDays(new Random().Next((end - start).Days));
        }

        public static String GetPesel()
        {
            DateTime start = new DateTime(1955, 1, 1);
            DateTime end = new DateTime(1990, 12, 31);
            DateTime birthDate = start.AddDays(new Random().Next((end - start).Days));
            String randomNumers = "";
            for (int i = 0; i < 5; i++)
            {
                randomNumers += new Random().Next(10).ToString();
            }
            return $"{birthDate.Year.ToString().Substring(2)}{birthDate.Month.ToString("00")}{birthDate.Day.ToString("00")}{randomNumers}";
        }

    }
}
