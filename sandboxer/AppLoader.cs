using System;
using System.IO;
using System.Security;
using System.Security.Policy;
using sandboxer.permissions;

namespace sandboxer.AppLoader
{
    /// <summary>
    /// Load assembly files and run them in the created sandbox environment
    /// with the specified security or permission level
    /// </summary>
    class SandboxEnvironment
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

            string message = "Running program " + program_path + "....";
            SandboxerGlobals.RedirectMessageDisplay(message);

            AppDomainSetup setup = new AppDomainSetup();
            
            try
            {
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.PrivateBinPath = SandboxerGlobals.WorkingDirectory;

                if (setup.ApplicationBase == null)
                {
                    string error_message = "The working directory path provided is not valid: ";
                    RuntimeException.Debug(error_message);
                    return;
                }
            }
            catch (Exception ex)
            {
                RuntimeException.Debug(ex.Message);
            }

            // getting user defined permissions
            // PermissionSet allowedSet = new PermissionSet(PermissionState.Unrestricted);
            PermissionSet allowedSet = PermissionManager.AddPermissionsToSet();
            allowedSet.Demand();
            
            StrongName[] trustedList = new StrongName[1];
            trustedList[0] = typeof(Program).Assembly.Evidence.GetHostEvidence<StrongName>();

            try
            {
                // create the sandbox application domain for the received program
                sandbox_domain = AppDomain.CreateDomain(
                    Path.GetFileNameWithoutExtension(SandboxerGlobals.ProgramToRun),
                    null, setup, allowedSet, trustedList);          

                // load the assembly into the sandbox application domain
            }
            catch (Exception ex)
            {                
                RuntimeException.Debug("Error: Application domain could not be created", ex.Message);
                return;
            }

            SandboxerGlobals.RedirectMessageDisplay("Loading assembly: " + SandboxerGlobals.ProgramToRun);
            
            try
            {
                // load and execute the assembly file into the application domain
                sandbox_domain.ExecuteAssembly(program_path, program_args);
            }
            catch (Exception ex)
            {
                string error_message = "Security Error: file " + SandboxerGlobals.ProgramToRun + " could not be executed";
                RuntimeException.Debug(error_message, ex.Message);
                return;
            }
        }
    }
}