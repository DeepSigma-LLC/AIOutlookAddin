using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_PlugIn.Utilities
{
    internal static class FileSystemUtilities
    {
        internal static IEnumerable<string> GetDirectoryNames(string base_path)
        {
            string[] directory_paths = System.IO.Directory.GetDirectories(base_path);
            foreach (string directory_path in directory_paths)
            {
                yield return Path.GetFileName(directory_path);
            }
        }

        internal static IEnumerable<string> GetFileNames(string base_path)
        {
            string[] file_paths = System.IO.Directory.GetFiles(base_path);
            foreach (string file_path in file_paths)
            {
                yield return Path.GetFileName(file_path);
            }
        }
    }
}
