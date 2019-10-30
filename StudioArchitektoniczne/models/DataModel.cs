using System;
using System.Collections.Generic;
using System.Text;

namespace StudioArchitektoniczne.models
{
    abstract class DataModel
    {
        public virtual string ToCsvString()
        {
            string text = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                text = text + prop.GetValue(this).ToString() + ",";
            }
            return text;
        }

        public virtual string ToBulkString()
        {
            var properties = this.GetType().GetProperties();
            string[] text = new string[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                text[i] = properties[i].GetValue(this).ToString();
            }

            return String.Join('|', text);
        }
    }
}
