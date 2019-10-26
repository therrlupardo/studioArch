using System;

namespace StudioArchitektoniczne.models
{
    class Address
    {
        public String city { get; set; }
        public String street { get; set; }
        public String postcode { get; set; }
        public String number { get; set; }

        public Address(String city, String street, String postcode, String number)
        {
            this.city = city;
            this.street = street;
            this.postcode = postcode;
            this.number = number;
        }

        public override string ToString()
        {
            return $"{city} {street} {number}, {postcode}";
        }
    }
}
