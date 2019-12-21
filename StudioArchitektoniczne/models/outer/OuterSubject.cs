using System;
using ArchitecturalStudio.models.enums;

namespace ArchitecturalStudio.models.outer
{
    public class OuterSubject : DataModel
    {
        public OuterSubject(int id)
        {
            Id = id;
            InitRandomValues();
        }

        private void InitRandomValues()
        {
            Name = RandomValueGenerator.GetEnumRandomValue<NameEnum>().ToString();
            Surname = RandomValueGenerator.GetEnumRandomValue<SurnameEnum>().ToString();
            Phone = RandomValueGenerator.GetPhoneNumber();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

    }
}
