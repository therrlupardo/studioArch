using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class Client
    {
        public Client(uint id, NameEnum name, SurnameEnum surname, string email, string phone, string companyName)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.phone = phone;
            this.companyName = companyName;
        }

        public UInt32 id { get; set; }
        public NameEnum name { get; set; }
        public SurnameEnum surname { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String companyName { get; set; }

    }
}
