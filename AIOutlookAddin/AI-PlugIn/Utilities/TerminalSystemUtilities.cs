using AI_PlugIn.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_PlugIn.DataStructures;

namespace AI_PlugIn.Utilities
{
    internal static class TerminalSystemUtilities
    {

        public static TerminalResponse RunTerminalProcess(string file_name, string arguments = "")
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = file_name,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Read output
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            TerminalResponse response = new TerminalResponse(output, error);
            return response;
        }
    }
}
