using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class GenericFormatter : AbstractLineFormatter
    {
        public GenericFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            string? result = Tag.Tag + Line;
            return !string.IsNullOrWhiteSpace(result) ? result : "";
        }
    }
}
