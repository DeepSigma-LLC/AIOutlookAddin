using AI_PlugIn.Classes;
using AI_PlugIn.DataStructures;
using AI_PlugIn.Enums;
using AI_PlugIn.Utilities;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_PlugIn
{
    public partial class ToolbarRibbon
    {
        private string ScriptPath = ConfigManager.GetScriptPath();
        private PythonController Python { get; set; }
        private void ToolbarRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            Python = new PythonController(ConfigManager.GetPythonEnvironmentPath());
            SetUIVariables();
            LoadModelSource();
            LoadTaskSelection();
            LoadPythonVersion();
            RefreshPythonEnvironments();
            LoadPythonDirectories();

            string key = ConfigManager.GetApiKey();
            
        }

        private void LoadTaskSelection()
        {
            string[] items = { "Summarize", "Rewrite for clarity", "Custom Prompt" };
            UIControls.LoadListControl(this, dropDown_AI_Task, items);
        }

        private void LoadModelSource()
        {
            string[] items = { "Azure", "OpenAI", "Ollama" };
            UIControls.LoadListControl(this,dropDown_modelSource, items);
        }

        private void LoadOpenAILLMModels()
        {
            string[] items = ConfigManager.GetOpenAIModels();
            UIControls.LoadListControl(this, dropDown_LMModel, items);
        }

        private void LoadAzureModels()
        {
            string[] items = ConfigManager.GetAzureModels();
            UIControls.LoadListControl(this, dropDown_LMModel, items);
        }


        private async System.Threading.Tasks.Task LoadLocalModels()
        {
            OllamaWrapper ollamaWrapper = new OllamaWrapper();
            string version = await ollamaWrapper.GetVersionAsync();
            if (version.Length == 0)
            {
                UIControls.ClearDropDown(this, dropDown_LMModel);
                return;
            } //Escape if no version is returned

            IEnumerable<string> models = await ollamaWrapper.GetAvailableModelsAsync();
            UIControls.LoadListControl(this, dropDown_LMModel, models.ToArray());
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

        private async void button_Run_Click(object sender, RibbonControlEventArgs e)
        {
            bool valid_selection = AIRunInputValidation();
            if (valid_selection == false) { return; };
           
            Result<string> result = OutlookUtilities.GetTextFromSelectedMailItem();
            if (result == null) { return; }
            if (result.ErrorEncountered)
            {
                MessageBox.Show(result.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(result.Value)) { return; }

            button_Run.Enabled = false;
            string selectedSource = dropDown_modelSource.SelectedItem?.Label ?? string.Empty;
            string model = dropDown_LMModel.SelectedItem?.Label ?? string.Empty;
            AIService ai = new AIService(selectedSource, ConfigManager.GetApiKey(), string.Empty);
            
            string ai_results = await ai.GenerateText(model, "Please summarize the following email: " + result.Value, null);
            MessageBox.Show(ai_results);
            button_Run.Enabled = true;
        }

        private bool AIRunInputValidation()
        {
            var selectedSource = dropDown_modelSource.SelectedItem;
            if (selectedSource is null)
            {
                MessageBox.Show("Please select a model source.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var selectedModel = dropDown_LMModel.SelectedItem;
            if (selectedSource is null)
            {
                MessageBox.Show("Please select a valid model.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var selectedTask = dropDown_AI_Task.SelectedItem;
            if (selectedSource is null)
            {
                MessageBox.Show("Please select a valid task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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

        private async void dropDown_modelSource_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            var selectedItem = dropDown_modelSource.SelectedItem;
            if (selectedItem is null) { return; }

            switch (selectedItem.Label)
            {
                case "Azure":
                    LoadAzureModels();
                    break;
                case "OpenAI":
                    LoadOpenAILLMModels();
                    break;
                case "Ollama":
                    await LoadLocalModels();
                    break;
                default:
                    MessageBox.Show("Model source not implemented: " + selectedItem.Label);
                    break;
            }
        }
    }
}
