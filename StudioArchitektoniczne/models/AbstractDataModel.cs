using System;
using System.Globalization;
using ArchitecturalStudio.interfaces;
using static System.DateTime;

namespace ArchitecturalStudio.models
{
    public abstract class  AbstractDataModel: IBulkConverter, ICsvConverter
    {
        public virtual string ToCsv()
        {
            var text = "";
            foreach (var prop in GetType().GetProperties())
            {
                var tmp = prop.GetValue(this).ToString();
                if (prop.PropertyType == typeof(DateTime))
                {
                    tmp = tmp.Substring(0, tmp.Length - 9);
                }
                text = text + tmp + ",";
            }
            return text;
        }

        public virtual string ToBulk()
        {
            var properties = GetType().GetProperties();
            var text = new string[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                var tmp = properties[i].GetValue(this).ToString();
                CorrectIfDate(ref tmp);
                text[i] = "" + tmp;
            }

            return string.Join('|', text);
        }

        public static void CorrectIfDate(ref string text)
        {
            string[] format = { "dd/MM/yyyy", "MM/dd/yyyy" };
            if (!TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) return;
            var tmp = text.Split("/");
            text = "";
            for (var i = tmp.Length - 1; i >= 0; i--)
            {
                text += tmp[i];
            }
        }

        public static string ConvertDate(DateTime date)
        {
            var txt = date.ToShortDateString();
            if (txt.Contains('.'))
            {
                txt = txt.Replace('.', '/');
            }
            CorrectIfDate(ref txt);
            return txt;
        }
    }
}
