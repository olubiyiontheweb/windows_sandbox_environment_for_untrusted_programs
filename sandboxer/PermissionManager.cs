using System;
using System.Data.Common;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Collections.Generic;
using System.Security.Permissions;

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
                if (SandboxerGlobals.PermissionSelections.Execution == true)
                {
                    permission_set.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                    SandboxerGlobals.RedirectMessageDisplay("Added Execution permission");
                }

                if (SandboxerGlobals.PermissionSelections.FileSystemAcess == true)
                {
                    permission_set.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, SandboxerGlobals.WorkingDirectory));
                    SandboxerGlobals.RedirectMessageDisplay("Added FileIOPermission permission");
                }

                if (SandboxerGlobals.PermissionSelections.Networking == true)
                {
                    if(string.IsNullOrWhiteSpace(SandboxerGlobals.NetworkAddress))
                    {
                        RuntimeException.Debug("You need to provide a network address to give the sandbox access to the network");
                        return null;
                    }
                    
                    permission_set.AddPermission(new SocketPermission(
                        NetworkAccess.Connect,
                        TransportType.All,
                        SandboxerGlobals.NetworkAddress,
                        SocketPermission.AllPorts));
                    SandboxerGlobals.RedirectMessageDisplay("Added Networking permission");
                }

                // adding some required permissions to the sandbox
                if(SandboxerGlobals.PermissionSelections.Reflection == true)
                {
                    permission_set.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
                    SandboxerGlobals.RedirectMessageDisplay("Added Reflection permission");
                }

                if (SandboxerGlobals.PermissionSelections.UserInterface == true)
                {
                    permission_set.AddPermission(new UIPermission(PermissionState.Unrestricted));
                    SandboxerGlobals.RedirectMessageDisplay("Added UserInterface permission");
                }

                if (SandboxerGlobals.PermissionSelections.Security == true)
                {
                    permission_set.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
                    SandboxerGlobals.RedirectMessageDisplay("Added Security permission");
                }

                if (SandboxerGlobals.PermissionSelections.AudioAccess == true)
                {
                    permission_set.AddPermission(new MediaPermission(MediaPermissionAudio.AllAudio, MediaPermissionVideo.NoVideo, MediaPermissionImage.SafeImage));
                    SandboxerGlobals.RedirectMessageDisplay("Added AudioAccess permission");
                }

                if (SandboxerGlobals.PermissionSelections.SMTP == true)
                {
                    permission_set.AddPermission(new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort));
                    SandboxerGlobals.RedirectMessageDisplay("Added SMTP permission");
                }

                if (SandboxerGlobals.PermissionSelections.Printing == true)
                {
                    permission_set.AddPermission(new PrintingPermission(PrintingPermissionLevel.DefaultPrinting));
                    SandboxerGlobals.RedirectMessageDisplay("Added Printing permission");
                }

                if (SandboxerGlobals.PermissionSelections.Web == true)
                {
                    permission_set.AddPermission(new WebPermission(PermissionState.Unrestricted));
                    SandboxerGlobals.RedirectMessageDisplay("Added Web permission");
                }

                if (SandboxerGlobals.PermissionSelections.Registry == true)
                {
                    permission_set.AddPermission(new RegistryPermission(RegistryPermissionAccess.AllAccess, "*"));
                    SandboxerGlobals.RedirectMessageDisplay("Added Registry permission");
                }

                // let's make sure we have all the permissions we need
                permission_set.Demand();
            }
            catch (Exception e)
            {                
                RuntimeException.Debug("Error: Sandbox could not set permissions for the program", e.Message);
                return null;
            }

            return permission_set;
        }

        /// <summary>
        /// create a .wsb configuration file for windows sandbox based on user inputs for global settings
        /// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-configure-using-wsb-file
        /// </summary>
        public static void CreateConfigurationFile()
        {
            // create a new file
            using (StreamWriter file = new StreamWriter("user_defined_sanbox_config" + ".wsb"))
            {
                // write xml content to the file
                file.WriteLine("<Configuration>");
                file.WriteLine("  <Networking>" + ((SandboxerGlobals.PermissionSelections.Networking == true) ? "Enable" : "Disable") + "</Networking>");
                file.WriteLine("  <MappedFolders>");
                file.WriteLine("    <MappedFolder>");
                file.WriteLine("      <HostFolder>" + SandboxerGlobals.WorkingDirectory +"</HostFolder>");
                file.WriteLine("      <SandboxFolder>" + @"c:\MountedFolder\" +"</SandboxFolder>");
                file.WriteLine("      <ReadOnly>" + (SandboxerGlobals.PermissionSelections.FileSystemAcess ? "False" : "True") + "</ReadOnly>");
                file.WriteLine("    </MappedFolder>");
                file.WriteLine("  </MappedFolders>");
                file.WriteLine("  <LogonCommand>");
                file.WriteLine("    <Command>" + Path.Combine(@"c:\MountedFolder\", SandboxerGlobals.ProgramToRun) + " " + string.Join(" ", SandboxerGlobals.ArgumentsForProgram) + "</Command>");
                file.WriteLine("  </LogonCommand>");

                // writing custom permissions to file
                try
                {
                    if (SandboxerGlobals.CustomPermissions.Count > 0)
                    {
                        foreach (string permission in SandboxerGlobals.CustomPermissions)
                        {
                            file.WriteLine(permission);
                        }
                    }
                }
                catch(Exception)
                { }
                
                file.WriteLine("</Configuration>");
            }
        }
    }
}
