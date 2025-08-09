using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.IO;

namespace AI_PlugIn.Classes
{
    internal static class ConfigManager
    {
        internal static string GetScriptPath()
        {
            return ConfigurationManager.AppSettings["PythonScriptPath"];
        }

        internal static string GetApiKey()
        {
            string path = ConfigurationManager.AppSettings["ApiKeyPath"];
            if(File.Exists(path) == false) { return string.Empty; }
            return File.ReadAllText(path);  
        }

        internal static string GetPythonEnvironmentPath()
        {
            return ConfigurationManager.AppSettings["PythonEnvironmentPath"];
        }

        internal static void SetPythonEnvironmentPath(string Path)
        {
            if (System.IO.Directory.Exists(Path) == false)
            {
                MessageBox.Show($"Directory does not exist: {Path}");
                return;
            }

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["PythonEnvironmentPath"].Value = Path;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
