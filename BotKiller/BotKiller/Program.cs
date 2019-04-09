using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

//       │ Author     : NYAN CAT
//       │ Name       : Bot Killer v0.1
//       │ Contact    : https://github.com/NYAN-x-CAT

//       This program Is distributed for educational purposes only.

namespace BotKiller
{
    class Program
    {
        static void Main()
        {
            RunBotKiller();
        }

        public static void RunBotKiller()
        {
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (Inspection(p.MainModule.FileName))
                        if (!IsWindowVisible(p.MainModule.FileName))
                        {
                            p.Kill();
                            RegistryDelete(p.MainModule.FileName);
                        }
                }
                catch { }
            }
        }

        private static bool Inspection(string payload)
        {
            if (payload.Equals(Process.GetCurrentProcess().MainModule.FileName, StringComparison.CurrentCultureIgnoreCase)) return false;
            if (payload.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))) return true;
            if (payload.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))) return true;
            if (payload.Contains(Path.GetTempPath())) return true;
            if (payload.EndsWith("vbs") || payload.EndsWith("js")) return true;
            if (payload.Contains(Path.GetTempPath())) return true;
            if (payload.Contains(RuntimeEnvironment.GetRuntimeDirectory())) return true;
            return false;
        }

        private static bool IsWindowVisible(string WinTitle)
        {
            IntPtr lHandle;
            lHandle = FindWindow(null, WinTitle);
            return IsWindowVisible(lHandle);
        }

        private static void RegistryDelete(string payload)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                foreach (string ValueOfName in key.GetValueNames())
                {
                    if (key.GetValue(ValueOfName).ToString().Equals(payload, StringComparison.CurrentCultureIgnoreCase))
                        key.DeleteValue(ValueOfName);
                }
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}