using HashGo.Core.Contracts.Model;
using HashGo.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class PrintService : IPrinterService
    {
        public async Task Print(PrinterSetting printer, string[] lines)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(printer.ShareName)) return;
                try
                {
                    LinePrinter? myPrinter = new LinePrinter(printer.ShareName, printer.CharsPerLine,
                        printer.CodePage);
                    myPrinter.StartDocument();
                    var formatters =
                        new FormattedDocument(lines, printer.CharsPerLine).GetFormatters().ToList();
                    foreach (var formatter in formatters)
                    {
                        SendToPrinter(myPrinter, formatter);
                    }
                    if (formatters.Count() > 1)
                        myPrinter.Cut();
                    myPrinter.EndDocument();
                }
                catch (Exception ex)
                {
                    //NLogger.Error(ex);
                }
            });
        }

        private void SendToPrinter(LinePrinter printer, ILineFormatter line)
        {
            string? data = line.GetFormattedLine();

            if (!data.StartsWith("<"))
                printer.WriteLine(data, line.FontHeight, line.FontWidth, LineAlignment.Left);
            else if (line.Tag.TagName == "eb")
                printer.EnableBold();
            else if (line.Tag.TagName == "db")
                printer.DisableBold();
            else if (line.Tag.TagName == "ec")
                printer.EnableCenter();
            else if (line.Tag.TagName == "el")
                printer.EnableLeft();
            else if (line.Tag.TagName == "er")
                printer.EnableRight();
            else if (line.Tag.TagName == "bmp")
                printer.PrintBitmap(RemoveTag(data));
            else if (line.Tag.TagName == "invoice")
                printer.PrintInvoice(RemoveTag(data));
            else if (line.Tag.TagName == "cut")
                printer.Cut();
            else if (line.Tag.TagName == "beep")
                printer.Beep();
            else if (line.Tag.TagName == "drawer")
                printer.OpenCashDrawer();
            else if (line.Tag.TagName == "b")
                printer.Beep((char)line.FontHeight, (char)line.FontWidth);
            else if (line.Tag.TagName == ("xct"))
                printer.ExecCommand(RemoveTag(data));
        }

        private static string RemoveTag(string line)
        {
            return line.Contains(">") ? line.Substring(line.IndexOf(">", StringComparison.Ordinal) + 1) : line;
        }

        public async Task PrintBytes(PrinterSetting printer, List<byte[]> lines)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(printer.ShareName)) return;
                try
                {
                    LinePrinter? myPrinter = new LinePrinter(printer.ShareName, printer.CharsPerLine,
                        printer.CodePage);
                    myPrinter.StartDocument();
                    foreach (byte[]? formatter in lines)
                    {
                        myPrinter.WriteData(formatter);
                    }
                    if (lines.Any())
                        myPrinter.Cut();
                    myPrinter.EndDocument();
                }
                catch (Exception ex)
                {
                }
            });
        }
    }
}
