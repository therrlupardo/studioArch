using System;
using System.Collections.Generic;
using System.Linq;

namespace StudioArchitektoniczne
{
    static class RandomValues
    {
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        public static string RandomEmail()
        {
            Random rand = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = rand.Next(1, 20);
            string email = new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());

            List<string> domains = new List<string> { "@gmail.com", "@outlook.com", "@op.pl", "@wp.pl", "@onet.pl", "@o2.pl", "@yahoo.com", "@interia.pl", "@hotmail.com", "@live.com" };
            email += domains.ElementAt(rand.Next(domains.Count));

            return email;
        }

        public static string RandomPhoneNumber()
        {
            Random rand = new Random();
            string phoneNumber = "";

            for (int i = 0; i < 9; i++)
            {
                phoneNumber += rand.Next(1, 10).ToString();
            }

            return phoneNumber;
        }

        public static string RandomCompanyName()
        {
            Random rand = new Random();
            string companyName;

            if (rand.Next(0, 1) < 0.6)
            {
                //Client is an organization
                int length = rand.Next(3, 31);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                companyName = new string(Enumerable.Repeat(chars, length).Select(s => s[rand.Next(s.Length)]).ToArray());
            }
            else
            {
                companyName = "";
            }

            return companyName;
        }
    }
}
