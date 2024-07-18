using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.BestTech.Print
{
    public class PrinterHelper
    {
        public static IntPtr GetPrinter(string szPrinterName)
        {
            DOCINFOA? di = new DOCINFOA { pDocName = "DinePlan Document", pDataType = "RAW" };
            IntPtr hPrinter;
            if (!OpenPrinter(szPrinterName, out hPrinter, IntPtr.Zero)) BombWin32();
            if (!StartDocPrinter(hPrinter, 1, di)) BombWin32();
            if (!StartPagePrinter(hPrinter)) BombWin32();
            return hPrinter;
        }

        public static void EndPrinter(IntPtr hPrinter)
        {
            EndPagePrinter(hPrinter);
            EndDocPrinter(hPrinter);
            ClosePrinter(hPrinter);
        }

        public static void SendBytesToPrinter(string szPrinterName, byte[] pBytes)
        {
            IntPtr hPrinter = GetPrinter(szPrinterName);
            int dwWritten;
            if (!WritePrinter(hPrinter, pBytes, pBytes.Length, out dwWritten)) BombWin32();
            EndPrinter(hPrinter);
        }

        public static void SendFileToPrinter(string szPrinterName, string szFileName)
        {
            FileStream? fs = new FileStream(szFileName, FileMode.Open);
            int len = (int)fs.Length;
            byte[]? bytes = new byte[len];
            fs.Read(bytes, 0, len);
            SendBytesToPrinter(szPrinterName, bytes);
        }

        private static void BombWin32()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, DOCINFOA di);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, byte[] pBytes, int dwCount, out int dwWritten);

        [StructLayout(LayoutKind.Sequential)]
        public class DOCINFOA
        {
            public string pDocName;
            public string pOutputFile;
            public string pDataType;
        }
    }
}
