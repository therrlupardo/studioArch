using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StudioArchitektoniczne.models
{
    public abstract class DataModel
    {
        public virtual string ToCsvString()
        {
            string text = "", tmp = "";
            foreach (var prop in this.GetType().GetProperties())
            {
                tmp = prop.GetValue(this).ToString();
                CorrectIfDate(ref text);
                text = text + tmp + ",";
            }
            return text;
        }

        public virtual string ToBulkString()
        {
            var properties = this.GetType().GetProperties();
            string[] text = new string[properties.Length];
            string tmp = "";

            for (int i = 0; i < properties.Length; i++)
            {
                tmp = properties[i].GetValue(this).ToString();
                CorrectIfDate(ref tmp);
                text[i] = "" + tmp;
            }

            return String.Join('|', text);
        }

        private void CorrectIfDate(ref string text)
        {
            string[] format = { "dd/MM/yyyy hh:mm:ss", "MM/dd/yyyy hh:mm:ss", };
            string[] tmp;
            if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                text = text.Substring(0, text.Length - 9);
                tmp = text.Split("/");
                text = "";
                for (int i = tmp.Length - 1; i >= 0; i--)
                {
                    text += tmp[i];
                }
                text = text.Replace("/", "");
            }

            // Regex regex = new Regex(@"^\d{1,2}\\/\d{1,2}\\/\d{4} \d{2}:\d{2}:\d{2}$"); 
            // if(regex.IsMatch(text)) text = text.Substring(0, text.Length - 9);
        }

    }
}
