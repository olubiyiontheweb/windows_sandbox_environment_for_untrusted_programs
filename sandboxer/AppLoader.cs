using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using sandboxer.permissions;

namespace sandboxer.AppLoader
{
    /// <summary>
    /// Load assembly files and run them in the created sandbox environment
    /// with the specified security or permission level
    /// </summary>
    internal class SandboxEnvironment
    {
        #region Private Fields

        private AppDomain sandbox_domain;
        
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public SandboxEnvironment() { }

        /// <summary>
        /// checks the given file if it's a valid assembly file and returns a boolean value
        /// this was copied from my code in component based architecture assignment on virtual machine
        /// (Oluwatosin, 2021)
        /// </summary>>
        
        internal static bool IsFileAnAssembly(string path)
        {
            if (!File.Exists(path))
            {
                string error_message = "File does not exist: " + path;
                RuntimeException.Debug(error_message);
            }

            try
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
            catch (Exception e)
            {
                RuntimeException.Debug(e.Message);
                return false;
            }
        }

        /// <summary>
        /// initialize app domain with permissions and security level
        /// </summary>
        public void InitalizeEnvironment()
        {
            string program_path = Path.Combine(SandboxerGlobals.WorkingDirectory, SandboxerGlobals.ProgramToRun);

            string message = "Running program " + program_path + " ....";
            SandboxerGlobals.RedirectMessageDisplay(message);

            AppDomainSetup setup = new AppDomainSetup();
            
            try
            {
                setup.ApplicationBase = SandboxerGlobals.WorkingDirectory;

                if (setup.ApplicationBase == null)
                {
                    string error_message = "The working directory path provided is not valid: ";
                    RuntimeException.Debug(error_message);
                    return;
                }
            }
            catch (Exception e)
            {
                RuntimeException.Debug(e.Message);
            }

            // getting user defined permissions
            PermissionSet allowedSet = PermissionManager.AddPermissionsToSet();

            try
            {
                // create the sandbox application domain for the received program
                sandbox_domain = AppDomain.CreateDomain("Sandboxer Domain", null, setup, allowedSet);          

                // load the assembly into the sandbox application domain
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Application domain could not be created", e.Message);
                return;
            }

            if (IsFileAnAssembly(program_path))
            {
                SandboxerGlobals.RedirectMessageDisplay("Loading assembly: " + SandboxerGlobals.ProgramToRun);
                try
                {
                    // load and execute the assembly file into the application domain
                    sandbox_domain.ExecuteAssembly(program_path, SandboxerGlobals.ArgumentsForProgram);
                    AppDomain.Unload(sandbox_domain);
                }
                catch (Exception e)
                {
                    string error_message = "Security Error: file " + SandboxerGlobals.ProgramToRun + " could not be executed";
                    RuntimeException.Debug(error_message, e.Message);
                    return;
                }
            }
            else
            {
                RuntimeException.Debug("Error: " + SandboxerGlobals.ProgramToRun + " is not a valid assembly file");
                return;
            }
        }
    }
}