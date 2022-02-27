using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sandboxer.interactive
{
    public interface ISandboxerUI
    {
        /// <summary>
        /// Gets or sets the sandbox mode, which is either dotnet or windows sandbox
        /// </summary>
        string sandboxMode { get; set; }

        /// <summary>
        /// List of selected permissions
        /// </summary>
        List<string> selectedPermissions { get; set; }

        /// <summary>
        /// The file name of the program to run in the sandbox
        /// </summary>
        string programName { get; set; }

        /// <summary>
        /// Arguments for the program
        /// </summary>
        string Arguments { get; set; }

        /// <summary>
        /// Sets the working directory for the sandboxed environment.
        /// </summary>
        string workingDirectory { get; set; }

        /// <summary>
        /// This method will be used to send error messages to the display.
        /// </summary>
        string errorMessage { get; set; }

        /// <summary>
        /// This method will be used to open the Sandboxer UI.
        /// </summary>
        void ShowUI();
    }
}
