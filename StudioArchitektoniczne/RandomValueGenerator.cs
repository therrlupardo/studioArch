using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
