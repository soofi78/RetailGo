using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public abstract class AbstractLineFormatter : ILineFormatter
    {
        private int _maxWidth;

        protected AbstractLineFormatter(string documentLine, int maxWidth)
        {
            Tag = new FormatTag(documentLine);
            MaxWidth = maxWidth;
            FontWidth = Tag.Width;
            FontHeight = Tag.Height;
            Line = RemoveTag(documentLine);
        }

        protected string Line { get; set; }

        protected int MaxWidth
        {
            get => _maxWidth / (FontWidth + 1);
            set => _maxWidth = value;
        }

        public int FontWidth { get; set; }
        public int FontHeight { get; set; }
        public FormatTag Tag { get; set; }
        public abstract string GetFormattedLine();

        public string GetFormattedLineWithoutTags()
        {
            string? result = GetFormattedLine();
            if (!string.IsNullOrEmpty(Tag.TagName))
                result = result.Replace(Tag.Tag, "");
            return result;
        }

        private static string RemoveTag(string line)
        {
            return line.Substring(line.IndexOf(">", StringComparison.Ordinal) + 1);
        }

        protected static string ExpandLabel(string label)
        {
            string? result = "";
            for (int i = 0; i < label.Length - 1; i++)
            {
                result += label[i] + " ";
            }
            result += label[label.Length - 1];
            return " " + result.Trim() + " ";
        }

        protected static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            return new StringInfo(str).LengthInTextElements;
        }

        protected static string SubStr(string str, int length)
        {
            if (length > 0)
            {
                return new StringInfo(str).SubstringByTextElements(0, length);
            }
            else
            {
                int totalLen = Math.Abs(length);
                if (str.Length >= totalLen)
                    return new StringInfo(str).SubstringByTextElements(0, Math.Abs(length));
                return new StringInfo(str).SubstringByTextElements(0, str.Length);
            }
        }

        protected static string ExpandStrRight(string str, int lenght)
        {
            str = str.TrimEnd();
            while (GetLength(str) < lenght)
                str = str + " ";
            return str;
        }

        protected static string ExpandStrLeft(string str, int lenght)
        {
            str = str.TrimStart();
            while (GetLength(str) < lenght)
                str = " " + str;
            return str;
        }

        protected string GetStrAt(string str, int index)
        {
            return new StringInfo(str).SubstringByTextElements(index, 1);
        }
    }
}
