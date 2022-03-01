using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Collections.Generic;
using System.Security.Permissions;

using sandboxer;

namespace sandboxer.permissions
{
    /// <summary>
    /// This class will handle the creation of configuration files and
    /// and addition of permissions to the configuration file or application domain.
    static class PermissionManager
    {
        /// <summary>
        /// populate permission sets for the sandbox based on user selections
        /// https://docs.microsoft.com/en-us/dotnet/api/system.security.codeaccesspermission?view=netframework-4.7.2
        /// </summary>
        public static PermissionSet AddPermissionsToSet()
        {            
            PermissionSet permission_set = new PermissionSet(PermissionState.None);

            try
            {
                // add all supported permissions to the permission set
                if (SandboxerGlobalSetting.PermissionSelections.Execution)
                    permission_set.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

                if (SandboxerGlobalSetting.PermissionSelections.FileSystemAcess)
                    permission_set.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, SandboxerGlobalSetting.WorkingDirectory));

                // adding some required permissions to the sandbox
                permission_set.AddPermission (new UIPermission (PermissionState.Unrestricted));
                permission_set.AddPermission (new ReflectionPermission (ReflectionPermissionFlag.MemberAccess));
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Sandbox could not set permissions for the program", e.Message);
            }

            try
            {                
                // cast list of strings (custom permissions) provided by users to list of permissions using reflection
                List<IPermission> custom_permissions = new List<IPermission>();

                foreach (string permission in SandboxerGlobalSetting.CustomPermissions)
                {
                    IPermission instance = (IPermission)Activator.CreateInstance(Type.GetType(permission));
                    custom_permissions.Add(instance);
                }                

                foreach (IPermission permission in custom_permissions)
                {
                    permission_set.AddPermission(permission);
                    permission_set.Demand();
                }
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Sandbox could not set your custom permissions for the program", e.Message);
            }

            return permission_set;
        }

        /// <summary>
        /// create a .wsb configuration file for windows sandbox based on user inputs for global settings
        /// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-configure-using-wsb-file
        /// </summary>
        public static void CreateConfigurationFile()
        {
            string working_directory = SandboxerGlobalSetting.WorkingDirectory;
            // create a new file
            using (StreamWriter file = new StreamWriter(working_directory + "user_defined_sanbox_config" + ".wsb"))
            {
                // write xml content to the file
                file.WriteLine("<Configuration>");
                file.WriteLine("  <Networking>" + (SandboxerGlobalSetting.PermissionSelections.Networking ? "Enable" : "Disable") + "</Networking>");
                file.WriteLine("  <MappedFolders>");
                file.WriteLine("    <MappedFolder>");
                file.WriteLine("      <HostFolder>" + SandboxerGlobalSetting.WorkingDirectory +"</HostFolder>");
                file.WriteLine("      <SandboxFolder>" + @"G:\" +"</SandboxFolder>");
                file.WriteLine("      <ReadOnly>" + (SandboxerGlobalSetting.PermissionSelections.FileSystemAcess ? "True" : "False") + "</ReadOnly>");
                file.WriteLine("    </MappedFolder>");
                file.WriteLine("  </MappedFolders>");
                file.WriteLine("  <LogonCommand>");
                file.WriteLine("    <Command>" + SandboxerGlobalSetting.ProgramToRun + " " + string.Join(" ", SandboxerGlobalSetting.ArgumentsForProgram) + "</Command>");
                file.WriteLine("  </LogonCommand>");

                // writing custom permissions to file
                if (SandboxerGlobalSetting.CustomPermissions.Count > 0)
                {
                    foreach (string permission in SandboxerGlobalSetting.CustomPermissions)
                    {
                        file.WriteLine(permission);
                    }
                }
                file.WriteLine("</Configuration>");
            }
        }
    }
}
