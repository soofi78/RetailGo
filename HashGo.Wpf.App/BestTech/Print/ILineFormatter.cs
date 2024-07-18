using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public interface ILineFormatter
    {
        int FontWidth { get; set; }
        int FontHeight { get; set; }
        FormatTag Tag { get; set; }
        string GetFormattedLine();
        string GetFormattedLineWithoutTags();
    }
}
