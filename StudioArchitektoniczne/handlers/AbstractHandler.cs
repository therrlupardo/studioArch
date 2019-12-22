using System.Collections.Generic;
using System.IO;
using System.Text;
using ArchitecturalStudio.models;

namespace ArchitecturalStudio.handlers
{
    public abstract class AbstractHandler
    {
        protected void WriteToCsv(List<AbstractDataModel> list, string filename)
        {
            var text = new StringBuilder();
            list.ForEach(element => text.AppendLine(element.ToCsv()));
            File.AppendAllText(filename, text.ToString(), Encoding.UTF8);
        }

        protected void WriteToBulk(List<AbstractDataModel> list, string filename)
        {
            var text = new StringBuilder();
            list.ForEach(element => text.AppendLine(element.ToBulk()));
            File.AppendAllText(filename, text.ToString(), Encoding.UTF8);
        }
    }
}