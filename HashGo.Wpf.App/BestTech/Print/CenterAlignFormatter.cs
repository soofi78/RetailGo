﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class CenterAlignFormatter : AbstractLineFormatter
    {
        public CenterAlignFormatter(string documentLine, int maxWidth)
            : base(documentLine, maxWidth)
        {
        }

        public override string GetFormattedLine()
        {
            return Line.PadLeft(((MaxWidth + Line.Length) / 2), ' ').PadRight(MaxWidth);
        }
    }
}
