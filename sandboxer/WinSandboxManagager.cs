﻿using System;
using System.IO;
using System.Diagnostics;
using System.Management.Automation;
using System.Collections.ObjectModel;

using sandboxer;
using sandboxer.permissions;

namespace sandboxer.winsand
{
    public static class WinSandboxManagager
    {
        static string error_message = "You need to run this program as administrator to install Windows Sandbox feature.";

        public static bool CheckWindowsValidity()
        {
            OperatingSystem os = Environment.OSVersion;
            Version os_version = os.Version;
            if ((os.Platform == PlatformID.Win32NT) && os_version.Major >= 10)
            {
                SandboxerGlobalSetting.RedirectMessageDisplay("Windows 10 or higher detected.");
                return true;
            }
            else
            {
                SandboxerGlobalSetting.RedirectMessageDisplay("Windows 10 or higher is required to run in Windows Sandbox Mode.");
                return false;
            }
        }

        public static bool IsWindowsSandboxEnabled()
        {
            try
            {
                SandboxerGlobalSetting.RedirectMessageDisplay("Please hold-on let's check if Windows Sandbox feature is enabled on this system...");
                PowerShell ps = PowerShell.Create();
                ps.AddScript("(Get-WindowsOptionalFeature -Online -FeatureName \"Containers-DisposableClientVM\").state");
                Collection<PSObject> psOutput = ps.Invoke();
                if (psOutput.Count > 0)
                {
                    if (psOutput[0].ToString() == "Enabled")
                    {
                        SandboxerGlobalSetting.RedirectMessageDisplay("Windows Sandbox feature is enabled on this system.");
                        return true;
                    }
                    else
                    {
                        SandboxerGlobalSetting.RedirectMessageDisplay("Windows Sandbox feature is not enabled on this system.");
                        return false;
                    }
                }
                else
                {
                    SandboxerGlobalSetting.RedirectMessageDisplay("Windows Sandbox feature is not enabled on this system.");
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
                SandboxerGlobalSetting.RedirectMessageDisplay("Installing Windows Sandbox...");

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

        private static bool DoesFileExist(string file_path)
        {
            if (!File.Exists(file_path))
            {
                return false;
            }
            return true;
        }

        public static void RunWindowsSandbox()
        {
            try
            {
                SandboxerGlobalSetting.RedirectMessageDisplay("Starting Windows Sandbox...");

                string windir = Environment.GetEnvironmentVariable("windir");

                // create custom configuration file for windows sandbox
                PermissionManager.CreateConfigurationFile();

                // check if user defined config file exists or we use default one
                string config_filename = DoesFileExist(SandboxerGlobalSetting.WorkingDirectory + @"\user_defined_sanbox_config.wsb") ?
                    SandboxerGlobalSetting.WorkingDirectory + @"\user_defined_sanbox_config.wsb" :
                    SandboxerGlobalSetting.WorkingDirectory + @"\windows_sanbox_config.wsb";

                // Now that we've confirmed Windows sandbox is enabled, we can run it with the new configuration
                Process process = Process.Start(windir + @"\Sysnative\WindowsSandbox.exe", config_filename);
                process.WaitForExit();
            }
            catch (RuntimeException e)
            {
                RuntimeException.Debug(e.Message);
            }
        }
    }
}
