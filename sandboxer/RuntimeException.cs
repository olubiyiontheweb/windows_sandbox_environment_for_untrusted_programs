using System;
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
                Console.WriteLine(custom_message);
            }
            else if (SandboxerGlobalSetting.LogMode == LogModes.FILE)
            {
                using (StreamWriter writer = new StreamWriter(errorLogFilePath, true))
                {
                    writer.WriteLine(custom_message);
                }
            }
        }

        public static void Debug (string custom_message, string exception_message)
        {   
            if (SandboxerGlobalSetting.LogMode == LogModes.CONSOLE)
            {                
                Console.WriteLine(custom_message + "\n");

                if(SandboxerGlobalSetting.Verbose > 0)
                {
                    Console.WriteLine("Exception_message: " + exception_message);
                }
            }
            else if (SandboxerGlobalSetting.LogMode == LogModes.FILE)
            {
                using (StreamWriter writer = new StreamWriter(errorLogFilePath, true))
                {
                    writer.WriteLine(custom_message + "\n");

                    if(SandboxerGlobalSetting.Verbose > 0)
                    {
                        writer.WriteLine("Exception_message: " + exception_message);
                    }
                }
            }
        }
    }
}
