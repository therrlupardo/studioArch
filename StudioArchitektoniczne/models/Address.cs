using StudioArchitektoniczne.models.enums;
using System;

namespace StudioArchitektoniczne.models
{
    class Address
    {
        public String city { get; set; }
        public String street { get; set; }
        public String postcode { get; set; }
        public String number { get; set; }

        public Address()
        {
            this.city = RandomValueGenerator.GetEnumRandomValue<CityEnum>().ToString();
            this.street = RandomValueGenerator.GetEnumRandomValue<StreetEnum>().ToString();
            this.postcode = $"{new Random().Next(100)}-{new Random().Next(1000)}";
            this.number = new Random().Next(100).ToString();
        }

        public override string ToString()
        {
            return $"{city}, {street} {number}, {postcode}";
        }
    }
}
