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
            foreach (var prop in GetType().GetProperties())
            {
                tmp = prop.GetValue(this).ToString();
                if (prop.PropertyType == typeof(DateTime))
                {
                    tmp = tmp.Substring(0, tmp.Length - 9);
                }
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

        public static void CorrectIfDate(ref string text)
        {
            string[] format = { "dd/MM/yyyy", "MM/dd/yyyy", };
            string[] tmp;
            if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                tmp = text.Split("/");
                text = "";
                for (int i = tmp.Length - 1; i >= 0; i--)
                {
                    text += tmp[i];
                }
            }
        }

        public static string ConvertDateToDDMMYYYY(DateTime date)
        {
            var txt = date.ToShortDateString();
            CorrectIfDate(ref txt);
            return txt;
        }

    }
}
