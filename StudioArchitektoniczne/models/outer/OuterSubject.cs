using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne
{
    class OuterSubject
    {
        public OuterSubject(uint id, NameEnum name, SurnameEnum surname, string phone)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.phone = phone;
        }

        public UInt32 id { get; set; }
        public NameEnum name { get; set; }
        public SurnameEnum surname { get; set; }
        public String phone { get; set; }
    }
}
