using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_PlugIn.DataStructures
{
    internal class Result<T>
    {
        internal T Value { get; set; }
        internal bool ErrorEncountered { get; set; }
        internal string Message { get; set; }
        internal List<string> MessageLog { get; set; }
    }
}
