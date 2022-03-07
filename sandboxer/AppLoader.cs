using System;
using System.IO;
using System.Security;

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
        /// initialize app domain with permissions and security level
        /// </summary>
        public void InitalizeEnvironment()
        {
            string program_path = Path.Combine(SandboxerGlobals.WorkingDirectory, SandboxerGlobals.ProgramToRun);
            string[] program_args = SandboxerGlobals.ArgumentsForProgram;

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
            allowedSet.Demand();

            try
            {
                // create the sandbox application domain for the received program
                sandbox_domain = AppDomain.CreateDomain(
                    Path.GetFileNameWithoutExtension(SandboxerGlobals.ProgramToRun),
                    null, setup, allowedSet);          

                // load the assembly into the sandbox application domain
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Application domain could not be created", e.Message);
                return;
            }

            SandboxerGlobals.RedirectMessageDisplay("Loading assembly: " + SandboxerGlobals.ProgramToRun);
            
            try
            {
                // load and execute the assembly file into the application domain
                sandbox_domain.ExecuteAssembly(program_path, program_args);
                AppDomain.Unload(sandbox_domain);
            }
            catch (Exception e)
            {
                string error_message = "Security Error: file " + SandboxerGlobals.ProgramToRun + " could not be executed";
                RuntimeException.Debug(error_message, e.Message);
                return;
            }
        }
    }
}