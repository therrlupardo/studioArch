using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
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

        public Guid id { get; }
        public NameEnum name { get; set; }
        public SurnameEnum surname { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String companyName { get; set; }

    }
}
