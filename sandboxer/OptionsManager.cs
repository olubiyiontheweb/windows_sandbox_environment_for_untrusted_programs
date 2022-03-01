using System;
using System.IO;
using System.Linq;

// third party library for parsing command line arguments
using CommandLine; // https://github.com/commandlineparser/commandline

using sandboxer.Definitions;

namespace sandboxer
{
    partial class Program
    {
        public class OptionsManager
        {
            [Option('d', "debugmode", Required = false, HelpText = "Display debug information while running program")]
            public bool DebugMode { get; set; }

            [Option('i', "pass_arguments", Required = false, HelpText = "Pass arguments to the program")]
            public bool PassArguments { get; set; }
        }

        /// <summary>
        /// ask the user for the program name, arguments and sandbox mode
        /// </summary>
        static void AskUserInteractively(string question)
        {
            if(SandboxerGlobalSetting.State != States.RUNNING)
            {
                return;
            }

            switch (question)
            {
                case "program":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nPlease enter the file name of the program you want to run in the sandbox (remember to place the file in the current/working directory): ");
                    string program = Console.ReadLine();
                    if (File.Exists(program))
                    {
                        SandboxerGlobalSetting.ProgramToRun = program;
                    }
                    else
                    {
                        RuntimeException.Debug("\nThe program you entered does not exist. Want to try again? (y/n)");
                        string input = Console.ReadLine();
                        if (input == "y" || input == "yes")
                        {
                            AskUserInteractively("program");
                        }
                        else
                        {
                            SandboxerGlobalSetting.State = States.EXIT;
                        }                       
                    }
                    break;
                case "arguments":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nEnter the arguments for the program you want to run in the sandbox: ");
                    string arguments = Console.ReadLine();
                    if (arguments != string.Empty)
                    {
                        SandboxerGlobalSetting.ArgumentsForProgram = arguments.Split(' ');
                    }
                    break;
                case "workingdir":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nEnter the working directory for the program you want to run in the sandbox: ");
                    string workingdir = Console.ReadLine();
                    if (Directory.Exists(workingdir))
                    {
                        SandboxerGlobalSetting.WorkingDirectory = workingdir;
                    }
                    else
                    {
                        RuntimeException.Debug("\nThe working directory you entered does not exist. Want to try again? (y/n)");
                        string input = Console.ReadLine();
                        if (input == "y" || input == "yes")
                        {
                            AskUserInteractively("workingdir");
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case "permissionselections":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nEnter the supported permissions you want to grant to the program (N/E/F): ");
                    string permissions = Console.ReadLine();
                    PermissionDict permissionStruct = new PermissionDict();
                    if (permissions.Contains("N"))
                    {
                        permissionStruct.Networking = true;
                    }

                    if (permissions.Contains("E"))
                    {
                        permissionStruct.Execution = true;
                    }

                    if (permissions.Contains("F"))
                    {
                        permissionStruct.FileSystemAcess = true;
                    }

                    SandboxerGlobalSetting.PermissionSelections = permissionStruct;

                    break;
                case "custompermissions":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nEnter the custom permissions you want to grant to the program (refer to user manual): ");
                    string custompermissions = Console.ReadLine();
                    if (custompermissions != string.Empty)
                    {
                        SandboxerGlobalSetting.CustomPermissions = custompermissions.Split(' ').ToList();
                    }
                    break;
                case "mode":
                    SandboxerGlobalSetting.RedirectMessageDisplay("\nOne more thing, please enter the sandbox mode you want to run the program in: ");
                    SandboxerGlobalSetting.RedirectMessageDisplay("\n0. Console");
                    SandboxerGlobalSetting.RedirectMessageDisplay("1. PowershellVM");
                    SandboxerGlobalSetting.RedirectMessageDisplay("2. Interactive");
                    SandboxerGlobalSetting.RunningMode = (RunningModes)Enum.Parse(typeof(RunningModes), Console.ReadLine(), true);
                    break;                        
                default:
                    AskUserInteractively("program");
                    AskUserInteractively("arguments");
                    AskUserInteractively("workingdir");
                    AskUserInteractively("permissionselections");
                    AskUserInteractively("custompermissions");
                    AskUserInteractively("mode");                    
                    break;
            }            
        }
    }
}
