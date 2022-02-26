using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Reflection;
using System.Runtime.Remoting;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using sandboxer;

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
        public void InitalizeEnvironment(string programName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Running program {0} ....", baseDirectory + @"\"+ programName);

            Byte[] fileBytes = File.ReadAllBytes(programName);

            AppDomainSetup setup = new AppDomainSetup();
            
            try
            {
                setup.ApplicationBase = baseDirectory;

                if (setup.ApplicationBase == null)
                {
                    string error_message = "The program path is not valid: " + programName;
                    RuntimeException.Debug(error_message);
                }
            }
            catch (Exception e)
            {
                RuntimeException.Debug(e.Message);
            }

            // selecting few security permissions here. 
            // user can specify more in the during use.
            // https://docs.microsoft.com/en-us/dotnet/api/system.security.codeaccesspermission?view=netframework-4.7.2
            PermissionSet allowedSet = new PermissionSet(PermissionState.None);
            
            try
            {
                allowedSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                allowedSet.AddPermission(new UIPermission(UIPermissionWindow.SafeTopLevelWindows));

                // only allow the current directory of the file to be accessed
                allowedSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, setup.ApplicationBase));
                allowedSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
                allowedSet.Demand();
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Sandbox could not set permissions for the program", e.Message);
            }

            try
            {
                // create the sandbox application domain for the received program
                sandbox_domain = AppDomain.CreateDomain(programName, null, setup, allowedSet);                
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Application domain could not be created", e.Message);
            }

            if (IsFileAnAssembly(programName))
            {
                Console.WriteLine("Loading assembly: " + programName);
                try
                {
                    // load and execute the assembly file into the application domain
                    var args = new String[] {  }; // args.Append("hello");
                    object[] parameters = new[] { args };
                    // sandbox_domain.ExecuteAssembly(programName, args);
                    Assembly assembly = sandbox_domain.Load(fileBytes);
                    MethodInfo method = assembly.EntryPoint;
                    method.Invoke(null, parameters);
                }
                catch (Exception e)
                {
                    string error_message = "Error: file " + programName + " could not be executed";
                    RuntimeException.Debug(error_message, e.Message);
                }
            }
            else
            {
                RuntimeException.Debug("Error: " + programName + " is not a valid assembly file");
            }
        }
    }
}