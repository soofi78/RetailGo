﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HashGo.Wpf.App.Behavior
{
    public class EnableTextBoxKeyboardBehaviour : Behavior<TextBox>
    {
        string programFiles = @"C:\Program Files\Common Files\Microsoft shared\ink";

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            this.AssociatedObject.LostFocus += AssociatedObject_LostFocus;
        }

        private void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
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

        void startOSKProcess()
        {
            string onScreenkeyboardPath = System.IO.Path.Combine(programFiles, "TabTip.exe");

            ProcessStartInfo processStartInfo = new ProcessStartInfo(onScreenkeyboardPath);
            processStartInfo.UseShellExecute = true;
            Process oskProcess = Process.Start(processStartInfo);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            this.AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
        }

        private void AssociatedObject_LostFocus(object sender, System.Windows.RoutedEventArgs e)
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
                closeOSKProcess();
            }
            //Process[] oskProcesses = Process.GetProcessesByName("TabTip");

            //if (oskProcesses?.Length > 0)
            //{
            //    foreach (Process process in oskProcesses)
            //    {
            //        //process.Close();
            //        process.Kill();
            //    }
            //}
        }

        void closeOSKProcess()
        {
            Process[] oskProcesses = Process.GetProcessesByName("TabTip");

            if (oskProcesses?.Length > 0)
            {
                foreach (Process process in oskProcesses)
                {
                    //process.Close();
                    process.Kill();
                }
            }
        }

        private void AssociatedObject_TextInput(object sender, TextCompositionEventArgs e)
        {
            //try
            //{
            //    var uiHostNoLaunch = new UIHostNoLaunch();
            //    var tipInvocation = (ITipInvocation)uiHostNoLaunch;
            //    tipInvocation.Toggle(GetDesktopWindow());
            //    Marshal.ReleaseComObject(uiHostNoLaunch);
            //}
            //catch (Exception ex)
            //{
            //    string onScreenkeyboardPath = System.IO.Path.Combine(programFiles, "TabTip.exe");

            //    ProcessStartInfo processStartInfo = new ProcessStartInfo(onScreenkeyboardPath);
            //    processStartInfo.UseShellExecute = true;
            //    Process oskProcess = Process.Start(processStartInfo);
            //}

            
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