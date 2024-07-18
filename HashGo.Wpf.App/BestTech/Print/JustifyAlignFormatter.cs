using HashGo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class JustifyAlignFormatter : AbstractLineFormatter
    {
        private readonly bool _canBreak;
        private readonly int[] _columnWidths;
        private readonly double _ratio;

        public JustifyAlignFormatter(string documentLine, int maxWidth, bool canBreak, double ratio,
            int[] columnWidths = null) :
                base(documentLine, maxWidth)
        {
            _canBreak = canBreak;
            _ratio = ratio;
            _columnWidths = CalculateColumnWidths(documentLine, columnWidths);
        }

        private static int[] CalculateColumnWidths(string documnentLine, int[] columnWidths)
        {
            string[]? parts = documnentLine.Split('|');
            if (columnWidths == null || columnWidths.Count() != parts.Length)
                columnWidths = new int[parts.Count()];
            for (int i = 0; i < parts.Count(); i++)
            {
                if (columnWidths[i] < GetLength(parts[i]))
                    columnWidths[i] = GetLength(parts[i]);
            }
            return columnWidths;
        }

        public override string GetFormattedLine()
        {
            return JustifyText(MaxWidth, Line, _canBreak, _columnWidths);
        }

        private string JustifyText(int maxWidth, string line, bool canBreak, IList<int> columnWidths)
        {
            string[]? parts = Split(line);
            if (parts.Length == 1) return line;

            string? text = "";
            for (int i = parts.Length - 1; i > 0; i--)
            {
                int l = columnWidths[i]; //columnWidths != null ? columnWidths[i] : parts[i].Length;
                parts[i] = ExpandStrLeft(parts[i], l);
                text = parts[i] + text;
            }

            if (GetLength(parts[0]) > maxWidth)
                parts[0] = parts[0].Substring(0, maxWidth);

            if (canBreak && parts[0].Length + text.Length > maxWidth)
            {
                return parts[0] + "\r" + text.PadLeft(maxWidth);
            }

            return Merge(maxWidth, FixStr(parts[0], maxWidth - GetLength(text)), text);
        }

        private static string FixStr(string str, int lenght)
        {
            return SubStr(ExpandStrRight(str, lenght), lenght);
        }

        protected virtual string[] Split(string line)
        {
            line = line.Replace("<t>", '\t'.ToString());
            return line.Split('|');
        }

        protected virtual string Merge(int maxWidth, params string[] parts)
        {
            if (_ratio != 1)
            {
                while (HaveSuitablePart(parts) && ActualLength(string.Join("", parts)) > maxWidth)
                {
                    int index = GetSuitablePartIndex(parts);
                    parts[index] = TrimPart(parts[index]);
                }
            }
            return string.Join("", parts);
        }

        private string TrimPart(string part)
        {
            int index = part.IndexOf("  ", StringComparison.Ordinal);
            return part.Remove(index, 1);
        }

        private bool HaveSuitablePart(IEnumerable<string> parts)
        {
            return parts.Any(x => x.EndsWith("  "));
        }

        private int GetSuitablePartIndex(string[] parts)
        {
            for (int i = 0; i < parts.Count(); i++)
            {
                if (parts[i].EndsWith("  ")) return i;
            }
            return -1;
        }

        public double ActualLength(string str)
        {
            double lenTotal = 0;
            int n = GetLength(str);
            for (int i = 0; i < n; i++)
            {
                string? strWord = GetStrAt(str, i);
                if (strWord.Length > 1)
                {
                    lenTotal++;
                    continue;
                }
                int asc = Convert.ToChar(strWord);
                if (asc == 9)
                {
                    lenTotal = Math.Truncate(lenTotal);
                    lenTotal = lenTotal + GetTabLength(lenTotal);
                }
                else if (asc > 0 && asc < 256)
                    lenTotal++;
                else
                    lenTotal = lenTotal + GetDifference(strWord);
            }
            return lenTotal;
        }

        public double GetDifference(string c)
        {
            if (_ratio > 0) return _ratio;
            FormattedText? nsize = GetSize("X");
            FormattedText? ssize = GetSize(c);
            return ssize.Width / nsize.Width;
        }

        public int GetTabLength(double lenTotal)
        {
            int diff = Convert.ToInt32(lenTotal);
            if (lenTotal >= 8)
                diff = diff % 8;
            return 8 - diff;
        }

        public FormattedText GetSize(string text)
        {
            FormattedText? v = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(AppSettings.PrintFontFamily), 12, Brushes.Black);
            return v;
        }

        public int[] GetColumnWidths()
        {
            return _columnWidths;
        }
    }
}
