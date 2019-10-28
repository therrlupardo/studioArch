using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models.outer
{
    class OuterSubject
    {
        public OuterSubject()
        {
            this.id = Guid.NewGuid();
            this.name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            this.surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            this.phone = RandomValueGenerator.GetPhoneNumber();
        }

        public Guid id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String phone { get; set; }
    }
}
