﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    internal class BoxFormatter : AbstractLineFormatter
    {
        public BoxFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            return PrintWindow(Line, true);
        }

        private string PrintWindow(string line, bool expandLabel)
        {
            const string tl = "┌";
            const string tr = "┐";
            const string bl = "└";
            const string br = "┘";
            const string vl = "│";
            const string hl = "─";
            const string s = "░";
            if (expandLabel) line = ExpandLabel(line);
            StringBuilder? sb = new StringBuilder();
            sb.AppendLine(tl + hl.PadLeft(MaxWidth - 2, hl[0]) + tr);
            string? text = vl + line.PadLeft((((MaxWidth - 2) + line.Length) / 2), s[0]);
            sb.AppendLine(text + vl.PadLeft(MaxWidth - text.Length, s[0]));
            sb.Append(bl + hl.PadLeft(MaxWidth - 2, hl[0]) + br);
            return sb.ToString();
        }
    }
}
