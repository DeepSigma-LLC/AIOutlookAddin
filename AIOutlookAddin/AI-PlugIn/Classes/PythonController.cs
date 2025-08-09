using AI_PlugIn.DataStructures;
using AI_PlugIn.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_PlugIn.Classes
{
    internal class PythonController
    {
        private string virtual_environment_name {  get; set; }
        private string environment_directroy {  get; set; }

        /// <summary>
        /// Initializes controller. Optional parameter should be the root directory containing all your python environments.
        /// </summary>
        /// <param name="Envirornment_Directory"></param>
        public PythonController(string Envirornment_Directory = "")
        {
            SetEnvironmentDirectory(Envirornment_Directory);
        }

        public void SetEnvironmentDirectory(string EnvironmentDirectory)
        {
            this.environment_directroy = EnvironmentDirectory;
        }

        /// <summary>
        /// Sets the virtual environment of the controller.
        /// </summary>
        /// <param name="selected_virtual_environment_name"></param>
        /// <returns></returns>
        public Exception SetVirutalEnvironmentName(string selected_virtual_environment_name)
        {
            string path = Path.Combine(environment_directroy, selected_virtual_environment_name, "Scripts", "python.exe");
            if (File.Exists(path))
            {
                this.virtual_environment_name = selected_virtual_environment_name;
                return null;
            }
            return new ArgumentException("Invaild Python environment selected.");
        }


        /// <summary>
        /// Gets the virtual environment of the controller.
        /// </summary>
        /// <returns></returns>
        public string GetVirutalEnvironmentName()
        {
            return virtual_environment_name;
        }

        /// <summary>
        /// Gets base python version.
        /// </summary>
        /// <returns></returns>
        public TerminalResponse GetPythonVersionBase()
        {
            return TerminalSystemUtilities.RunTerminalProcess("python", "--version");
        }

        /// <summary>
        /// Gets python version of the selected virutal environment.
        /// </summary>
        /// <returns></returns>
        public TerminalResponse GetPythonVersionVirtualEnvironment()
        {
            string path = GetVirtualEnvironmentPythonExeFilePath();
            return TerminalSystemUtilities.RunTerminalProcess(path, "--version");
        }

        /// <summary>
        /// Executes a python script with optional arguements by passing it to your selected virtual environment.
        /// </summary>
        /// <param name="python_full_file_path"></param>
        /// <param name="scriptArgs"></param>
        /// <returns></returns>
        public TerminalResponse RunPythonScript(string python_full_file_path, string scriptArgs = "")
        {
            string pythonExe = GetVirtualEnvironmentPythonExeFilePath();
            string arguments = $"\"{python_full_file_path}\" {scriptArgs}";
            return TerminalSystemUtilities.RunTerminalProcess(pythonExe, arguments);
        }

        /// <summary>
        /// Gets virtual environment names.
        /// </summary>
        /// <param name="python_environment_parent_directory"></param>
        /// <returns></returns>
        public string[] GetVirtualEnvironmentNames()
        {
            if (string.IsNullOrEmpty(environment_directroy)) { return Array.Empty<string>(); }
            if (Directory.Exists(environment_directroy) == false) { return Array.Empty<string>(); }

            string[] directories = FileSystemUtilities.GetDirectoryNames(environment_directroy).ToArray();
            return directories;
        }

        /// <summary>
        /// Gets the file path of your python.exe file from your selected virtual environment.
        /// </summary>
        /// <returns></returns>
        private string GetVirtualEnvironmentPythonExeFilePath()
        {
            return Path.Combine(environment_directroy, virtual_environment_name, "Scripts", "python.exe");
        }
    }
}
