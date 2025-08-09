using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using AI_PlugIn.Classes;
using AI_PlugIn.DataStructures;
using AI_PlugIn.Utilities;

namespace AI_PlugIn
{
    public partial class ToolbarRibbon
    {
        private OpenAIRoute AI { get; set; }

        private string ScriptPath = ConfigManager.GetScriptPath();
        private PythonController Python { get; set; }
        private void ToolbarRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            Python = new PythonController(ConfigManager.GetPythonEnvironmentPath());
            SetUIVariables();
            LoadModelSource();
            LoadLLMModels();
            LoadTaskSelection();
            LoadPythonVersion();
            RefreshPythonEnvironments();
            LoadPythonDirectories();

            string key = ConfigManager.GetApiKey();
            AI = new OpenAIRoute(key);
            
        }

        private void LoadTaskSelection()
        {
            string[] items = { "Summarize", "Re-write for clarity", "Custom Prompt" };
            UIControls.LoadListControl(this, dropDown_AI_Task, items);
        }

        private void LoadModelSource()
        {
            string[] items = { "Azure", "Local" };
            UIControls.LoadListControl(this,dropDown_modelSource, items);
        }

        private void LoadLLMModels()
        {
            string[] items = { "gpt-4o", "gpt-4o mini", "o3-mini", "o1-mini" , "o1", "o3", "gpt-4.1 mini", "gpt-4.1" };
            UIControls.LoadListControl(this, dropDown_LMModel, items);
        }

        private void LoadPythonVersion()
        {
            editBox_PythonVersion.Text = Python.GetPythonVersionBase().Output ?? string.Empty;
        }

        private void SetUIVariables()
        {
            editBox_PythonVersion.Enabled = false;
            editBox_environmentVersion.Enabled = false;
        }

        private void LoadPythonDirectories()
        {
            string[] project_directory_names = FileSystemUtilities.GetDirectoryNames(ScriptPath).ToArray();
            UIControls.LoadListControl(this, dropDown_pythonDirectory, project_directory_names);
            LoadPythonProjectDirectories();
        }

        private void dropDown_pythonDirectory_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            UIControls.ClearDropDown(this, dropDown_pythonScripts);
            LoadPythonProjectDirectories();
        }

        private void dropDown_pythonProject_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            var selected_directory = dropDown_pythonDirectory.SelectedItem;
            var selected_project_directory = dropDown_pythonSubDirectory.SelectedItem;
            if (selected_directory is null || selected_project_directory is null) { return; }   

            string selectedPath = Path.Combine(ScriptPath, selected_directory.Label,selected_project_directory.Label);
            LoadPythonFilesFromSelectedDirectory(selectedPath);
        }

        private void LoadPythonProjectDirectories()
        {

            var selectedItem = dropDown_pythonDirectory.SelectedItem;
            if(selectedItem == null) { return; }

            string FullPath = Path.Combine(ScriptPath, selectedItem.Label);

            string[] sub_directory_names = FileSystemUtilities.GetDirectoryNames(FullPath).ToArray();
            UIControls.LoadListControl(this, dropDown_pythonSubDirectory, sub_directory_names);
        }


        /// <summary>
        /// Loads python files from selected directory.
        /// </summary>
        /// <param name="selected_directory"></param>
        private void LoadPythonFilesFromSelectedDirectory(string selected_directory)
        {
            string[] file_names = FileSystemUtilities.GetFileNames(selected_directory).ToArray();
            UIControls.LoadListControl(this, dropDown_pythonScripts, file_names.ToArray());
        }

        private void button_Run_Click(object sender, RibbonControlEventArgs e)
        {
            Result<string> result = OutlookUtilities.GetTextFromSelectedMailItem();
            if (result == null) { return; }
            if (result.ErrorEncountered)
            {
                MessageBox.Show(result.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            MessageBox.Show(result.Value);
        }


        private void button_pythonrun_Click(object sender, RibbonControlEventArgs e)
        {
            var selected_directory = dropDown_pythonDirectory.SelectedItem;
            var selected_project_directory = dropDown_pythonSubDirectory.SelectedItem;
            var selected_script = dropDown_pythonScripts.SelectedItem;
            if (selected_directory is null || selected_project_directory is null || selected_script is null) { return; }

            string selectedPath = Path.Combine(ScriptPath, selected_directory.Label, selected_project_directory.Label, selected_script.Label);
            if (File.Exists(selectedPath) == false)
            {
                MessageBox.Show($"Invalid python script path: {selectedPath}","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TerminalResponse output = Python.RunPythonScript(selectedPath);
            MessageBox.Show(output.Output);
        }

        private void button_ConfigEnviornmentPath_Click(object sender, RibbonControlEventArgs e)
        {
            string selected_path = string.Empty;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder containing your Python environment";
                dialog.ShowNewFolderButton = false; // optional: hide "New Folder" button

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selected_path = dialog.SelectedPath;
                }
                else
                {
                    return; //User cancelled
                }
            }
            
            if(Directory.Exists(selected_path) == false)
            {
                MessageBox.Show($"Invalid python environment path selected: {selected_path}");
                return;
            }

            DialogResult result = MessageBox.Show($"You are about to update your Add-in configuration setting for your python environments to: {selected_path}. Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(result == DialogResult.Yes)
            {
                ConfigManager.SetPythonEnvironmentPath(selected_path);
                Python.SetEnvironmentDirectory(selected_path);
                RefreshPythonEnvironments();
                RefreshPythonEnviromentVersion();
            }
        }
        private void dropDown_pythonEnvironment_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            RefreshPythonEnviromentVersion();
        }

        private void RefreshPythonEnvironments()
        {
            string[] directories = Python.GetVirtualEnvironmentNames();
            UIControls.LoadListControl(this, dropDown_pythonEnvironment, directories);
            RefreshPythonEnviromentVersion();
        }

        private void RefreshPythonEnviromentVersion()
        {
            bool success = TrySetPythonVirtualEnvironment();
            if(success == false) { return; }

            TerminalResponse response = Python.GetPythonVersionVirtualEnvironment();
            editBox_environmentVersion.Text = response.Output;
        }

        private bool TrySetPythonVirtualEnvironment()
        {
            var selected_environment = dropDown_pythonEnvironment.SelectedItem;
            if (selected_environment is null)
            {
                MessageBox.Show("A vaild python environment was not selected.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            System.Exception result = Python.SetVirutalEnvironmentName(selected_environment.Label);
            if(result is null) { return true; }
            return false;
        }
    }
}
