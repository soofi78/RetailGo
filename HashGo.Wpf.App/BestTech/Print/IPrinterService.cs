using HashGo.Core.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public interface IPrinterService
    {
         Task PrintBytes(PrinterSetting printer, List<byte[]> lines);
        Task Print(PrinterSetting printer, string[] lines);
    }
}
