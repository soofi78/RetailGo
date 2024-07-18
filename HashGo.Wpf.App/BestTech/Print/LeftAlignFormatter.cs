using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class LeftAlignFormatter : AbstractLineFormatter
    {
        public LeftAlignFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            return Line.PadRight(MaxWidth, ' ');
        }
    }
}
