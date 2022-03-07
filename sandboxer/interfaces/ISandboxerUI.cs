using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sandboxer.Definitions;

namespace sandboxer.interactive
{
    public interface ISandboxerUI
    {
        /// <summary>
        /// Gets or sets the sandbox mode, which is either dotnet or windows sandbox
        /// </summary>
        RunningModes sandboxMode { get; set; }

        /// <summary>
        /// List of selected permissions
        /// </summary>
        PermissionDict availablePermissions { get; set; }

        /// <summary>
        /// The file name of the program to run in the sandbox
        /// </summary>
        string programName { get; set; }

        /// <summary>
        /// Arguments for the program
        /// </summary>
        string[] Arguments { get; set; }

        /// <summary>
        /// Sets the working directory for the sandboxed environment.
        /// </summary>
        string workingDirectory { get; set; }

        string networkAddress { get; set; }

        /// <summary>
        /// This method will be used to send error messages to the display.
        /// </summary>
        List<string> errorMessage { get; set; }
        List<string> customPermissions { get; set; }

        /// <summary>
        /// This method will be used to open the Sandboxer UI.
        /// </summary>
        void ShowUI();

        /// <summary>
        /// this method will get the current checked permissions at the time of button click
        /// </summary>
        void GetCurrentPermissions();

        void RefreshConsoleLog();
    }
}
