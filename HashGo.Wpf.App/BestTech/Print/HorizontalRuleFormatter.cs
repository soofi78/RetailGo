using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class HorizontalRuleFormatter : AbstractLineFormatter
    {
        public HorizontalRuleFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            string? result = Line.Trim();
            if (result.Length > 0)
                return "".PadLeft(MaxWidth, result[0]);
            return result;
        }
    }
}
