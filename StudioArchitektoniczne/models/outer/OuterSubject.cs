using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models.outer
{
    class OuterSubject : DataModel
    {
        public OuterSubject(int id)
        {
            this.id = id;
            name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            phone = RandomValueGenerator.GetPhoneNumber();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }

    }
}
