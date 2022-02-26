using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Collections.ObjectModel;

using sandboxer;

namespace sandboxer.winsand
{
    static class WinSandboxManagager
    {
        static string basedir = Environment.CurrentDirectory;
        static string error_message = "You need to run this program as administrator to install Windows Sandbox feature.";

        public static bool CheckWindowsValidity()
        {
            OperatingSystem os = Environment.OSVersion;
            Version os_version = os.Version;
            if ((os.Platform == PlatformID.Win32NT) && os_version.Major >= 10)
            {
                Console.WriteLine("Windows 10 or higher detected.");
                return true;
            }
            else
            {
                Console.WriteLine("Windows 10 or higher is required to run in Windows Sandbox Mode.");
                return false;
            }
        }

        public static bool IsWindowsSandboxEnabled()
        {
            try
            {
                Console.WriteLine("Please hold-on let's check if Windows Sandbox feature is enabled on this system...");
                PowerShell ps = PowerShell.Create();
                ps.AddScript("(Get-WindowsOptionalFeature -Online -FeatureName \"Containers-DisposableClientVM\").state");
                Collection<PSObject> psOutput = ps.Invoke();
                if (psOutput.Count > 0)
                {
                    if (psOutput[0].ToString() == "Enabled")
                    {
                        Console.WriteLine("Windows Sandbox feature is enabled on this system.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Windows Sandbox feature is not enabled on this system.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Windows Sandbox feature is not enabled on this system.");
                    return false;
                }
            }
            catch (RuntimeException e)
            {
                RuntimeException.Debug("Error: " + error_message + "\n", e.Message);
                return false;
            }
        }

        public static void InstallWindowsSandbox()
        {
            try 
            {
                Console.WriteLine("Installing Windows Sandbox...");

                // we need to enable Windows sandbox feature in windows if 
                // we must run Windows Sandbox in windows 10
                PowerShell ps = PowerShell.Create();
                ps.AddCommand("Install-WindowsOptionalFeature");
                ps.AddParameter("-FeatureName", "Containers-DisposableClientVM");
                ps.AddParameter("-All");
                ps.AddParameter("-Online");
                ps.Invoke();
            }
            catch (RuntimeException e)
            {
                RuntimeException.Debug("Error: " + error_message + "\n", e.Message);
            }        
        }

        public static void RunWindowsSandbox()
        {
            try
            {
                Console.WriteLine("Starting Windows Sandbox...");

                string windir = Environment.GetEnvironmentVariable("windir");

                // Now that we've confirmed Windows sandbox is enabled, we can run it with the new configuration
                Process process = Process.Start(windir + @"\Sysnative\WindowsSandbox.exe", basedir + @"\windows_sanbox_config.wsb");
                process.WaitForExit();
            }
            catch (RuntimeException e)
            {
                RuntimeException.Debug(e.Message);
            }
        }
    }
}
