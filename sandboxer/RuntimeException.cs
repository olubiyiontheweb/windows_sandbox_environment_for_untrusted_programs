﻿using System;
using System.IO;
using System.Runtime.Serialization;

using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.winsand;

namespace sandboxer
{
    /// <summary>
    /// Handles all the exceptions thrown by the application
    [Serializable]
    public class RuntimeException : Exception
    {
        private const string msg = "Oops!!! An error has occurred while executing the Sandbox program.\n" +
                                   "Please check the log (file) for more details.";

        public RuntimeException() : base(msg) { }
        public RuntimeException(string message) : base(msg + message) { }
        public RuntimeException(string message, Exception innerException) : base(msg + message, innerException) { }

        /// <summary>
        /// Show debug information if verbose mode is enabled
        /// </summary>

        static string errorLogFilePath = @"ErrorLog.txt";

        public static void Debug (string custom_message)
        {   
            if (SandboxerGlobalSetting.LogMode == LogModes.CONSOLE)
            {
                if(SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE)
                {
                    try
                    {
                        // append to the console box in windows forms
                        SandboxerGlobalSetting.SandboxerUIInstance.errorMessage = custom_message;
                    }
                    catch (Exception)
                    {                        
                        // do nothing
                    }
                }
                else
                {
                    Console.WriteLine(custom_message);
                }
            }
            else if (SandboxerGlobalSetting.LogMode == LogModes.FILE)
            {
                using (StreamWriter writer = new StreamWriter(errorLogFilePath, true))
                {
                    writer.WriteLine(custom_message);
                }
            }
        }

        /// <summary>
        /// Overloading the debug method
        public static void Debug (string custom_message, string exception_message)
        {   
            if (SandboxerGlobalSetting.LogMode == LogModes.CONSOLE)
            {                
                if(SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE)
                {
                    try
                    {
                        // append to the console box in windows forms
                        SandboxerGlobalSetting.SandboxerUIInstance.errorMessage = custom_message;
                    }
                    catch (Exception)
                    {                        
                        // do nothing
                    }
                }
                else
                {
                    Console.WriteLine(custom_message);
                }

                if(SandboxerGlobalSetting.DebugMode == true)
                {
                    if(SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE)
                    {
                        try
                        {
                            // append to the console box in windows forms
                            SandboxerGlobalSetting.SandboxerUIInstance.errorMessage = custom_message;
                        }
                        catch (System.Exception)
                        {                        
                            // do nothing
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Exception_message: " + exception_message + "\n");
                }
            }
            else if (SandboxerGlobalSetting.LogMode == LogModes.FILE)
            {
                using (StreamWriter writer = new StreamWriter(errorLogFilePath, true))
                {
                    writer.WriteLine(custom_message + "\n");

                    if(SandboxerGlobalSetting.DebugMode == true)
                    {
                        writer.WriteLine("Exception_message: " + exception_message);
                    }
                }
            }
        }
    }
}
