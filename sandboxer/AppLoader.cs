using System;
using System.IO;
using sandboxer.Definitions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace sandboxer.AppLoader
{
    /// <summary>
    /// Load assembly files and run them in the created sandbox environment
    /// </summary>
    internal class SandboxEnvironment
    {
        #region Private Fields
        // The following variables are used to store the values of the command line arguments.
        private RunningModes runningMode = RunningModes.CONSOLE;
        private LogModes logMode = LogModes.CONSOLE;
        private LogLevels logLevel = LogLevels.INFO;
        private SecurityLevels securityLevel = SecurityLevels.DEFAULT;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public SandboxEnvironment()
        {

        }
        public SandboxEnvironment(RunningModes runningMode, LogModes logMode, LogLevels logLevel, SecurityLevels securityLevel)
        {
            // set the running mode
            this.runningMode = runningMode;
            // set the log mode
            this.logMode = logMode;
            // set the log level
            this.logLevel = logLevel;
            // set the security level
            this.securityLevel = securityLevel;
        }

        /// <summary>
        /// checks the given file if it's a valid assembly file and returns a boolean value
        /// this was copied from my code in component based architecture assignment on virtual machine
        /// (Oluwatosin, 2021)
        /// </summary>>
        internal static bool IsFileAnAssembly(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            // initalize the PE reader on the file stream
            PEReader peReader = new PEReader(fs);

            // check if library has CLI metadata
            return (!peReader.HasMetadata) ? false : (
                // Check that file has an assembly manifest.
                peReader.GetMetadataReader().IsAssembly
            );
        }

        /// <summary>
        /// load the assembly files and run them in the created sandbox environment
        /// </summary>
        public void LoadApplication(string programName)
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            System.Console.WriteLine("Loading application: " + programName);
            
            AppDomainSetup setup = new AppDomainSetup();

            // create application domain for the received program
            AppDomain programDomain = AppDomain.CreateDomain(programName, null, setup);

            if (IsFileAnAssembly(programName))
            {
                // load the assembly file into the application domain
                programDomain.Load(programName);
            }
            else
            {
                Console.WriteLine("The file is not a valid assembly file");
            }
        }
    }
}