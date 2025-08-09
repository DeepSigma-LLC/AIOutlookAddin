using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_PlugIn.DataStructures
{
    internal class TerminalResponse
    {
        internal string Output { get; set; } = string.Empty;
        internal string Error { get; set; } = string.Empty;
        internal TerminalResponse(string Output = "", string Error = "")
        {
            this.Output = Output;
            this.Error = Error;
        }
    }
}
