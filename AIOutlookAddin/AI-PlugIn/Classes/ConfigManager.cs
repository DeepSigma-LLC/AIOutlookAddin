using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        internal static string[] GetOpenAIModels()
        {
            var section = (NameValueCollection)ConfigurationManager.GetSection("OpenAIModels");
            return section.AllKeys.Select(key => key).ToArray();
        }

        internal static string[] GetAzureModels()
        {
            var section = (NameValueCollection)ConfigurationManager.GetSection("AzureModels");
            return section.AllKeys.Select(key => key).ToArray();
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
