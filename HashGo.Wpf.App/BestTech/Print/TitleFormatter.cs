using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class TitleFormatter : AbstractLineFormatter
    {
        public TitleFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            return PrintCenteredLabel(Line, true);
        }

        private string PrintCenteredLabel(string label, bool expandLabel, char fillChar = '░')
        {
            if (string.IsNullOrEmpty(label)) return "".PadLeft(MaxWidth, fillChar);
            if (expandLabel) label = ExpandLabel(label);
            int leftPad = ((MaxWidth) + label.Length);
            if (leftPad % 2 == 1) leftPad++;
            string? text = label.PadLeft(leftPad / 2, fillChar);
            int totalLength = MaxWidth - text.Length;

            if (totalLength > 0)
                return text + "".PadLeft(MaxWidth - text.Length, fillChar);
            return text + "".PadLeft(MaxWidth, fillChar);
        }
    }
}
