using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Helpers
{
    public static class TabTipHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        static string programFiles = @"C:\Program Files\Common Files\Microsoft shared\ink";

        public static void ShowTabTip()
        {
            try
            {
                var uiHostNoLaunch = new UIHostNoLaunch();
                var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                tipInvocation.Toggle(GetDesktopWindow());
                Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch (Exception ex)
            {
                startOSKProcess();
            }
        }

        static void startOSKProcess()
        {
            string onScreenkeyboardPath = System.IO.Path.Combine(programFiles, "TabTip.exe");

            ProcessStartInfo processStartInfo = new ProcessStartInfo(onScreenkeyboardPath);
            processStartInfo.UseShellExecute = true;
            Process oskProcess = Process.Start(processStartInfo);
        }

        public static void HideTabTip()
        {
            try
            {
                var uiHostNoLaunch = new UIHostNoLaunch();
                var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                tipInvocation.Toggle(IntPtr.Zero); // Pass IntPtr.Zero to close the keyboard
                Marshal.ReleaseComObject(uiHostNoLaunch);
            }
            catch (Exception ex)
            {

            }

            finally
            {
                IntPtr hwnd = GetForegroundWindow();
                foreach (Process p in Process.GetProcessesByName("TabTip"))
                {
                    if (p.MainWindowHandle != hwnd)
                    {
                        p.Kill();
                    }
                }
            }
           
        }

        #region Touch keyboard

        [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
        class UIHostNoLaunch
        {
        }

        [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ITipInvocation
        {
            void Toggle(IntPtr hwnd);
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        #endregion
    }
}
