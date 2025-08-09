namespace AI_PlugIn
{
    partial class ToolbarRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ToolbarRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab_AIAddin = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.dropDown_modelSource = this.Factory.CreateRibbonDropDown();
            this.dropDown_LMModel = this.Factory.CreateRibbonDropDown();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.dropDown_AI_Task = this.Factory.CreateRibbonDropDown();
            this.button_Run = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.button_ConfigEnviornmentPath = this.Factory.CreateRibbonButton();
            this.editBox_PythonVersion = this.Factory.CreateRibbonEditBox();
            this.dropDown_pythonEnvironment = this.Factory.CreateRibbonDropDown();
            this.editBox_environmentVersion = this.Factory.CreateRibbonEditBox();
            this.dropDown_pythonDirectory = this.Factory.CreateRibbonDropDown();
            this.dropDown_pythonSubDirectory = this.Factory.CreateRibbonDropDown();
            this.dropDown_pythonScripts = this.Factory.CreateRibbonDropDown();
            this.button_pythonrun = this.Factory.CreateRibbonButton();
            this.tab_AIAddin.SuspendLayout();
            this.group1.SuspendLayout();
            this.group3.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_AIAddin
            // 
            this.tab_AIAddin.Groups.Add(this.group1);
            this.tab_AIAddin.Groups.Add(this.group3);
            this.tab_AIAddin.Groups.Add(this.group2);
            this.tab_AIAddin.Label = "AI Assist";
            this.tab_AIAddin.Name = "tab_AIAddin";
            // 
            // group1
            // 
            this.group1.Items.Add(this.dropDown_modelSource);
            this.group1.Items.Add(this.dropDown_LMModel);
            this.group1.Label = "LM Config";
            this.group1.Name = "group1";
            // 
            // dropDown_modelSource
            // 
            this.dropDown_modelSource.Label = "Model Source";
            this.dropDown_modelSource.Name = "dropDown_modelSource";
            this.dropDown_modelSource.SizeString = "XXXXXXXX";
            // 
            // dropDown_LMModel
            // 
            this.dropDown_LMModel.Label = "LM Model";
            this.dropDown_LMModel.Name = "dropDown_LMModel";
            this.dropDown_LMModel.SizeString = "XXXXXXXXXXXXX";
            // 
            // group3
            // 
            this.group3.Items.Add(this.dropDown_AI_Task);
            this.group3.Items.Add(this.button_Run);
            this.group3.Label = "Request";
            this.group3.Name = "group3";
            // 
            // dropDown_AI_Task
            // 
            this.dropDown_AI_Task.Label = "Task";
            this.dropDown_AI_Task.Name = "dropDown_AI_Task";
            this.dropDown_AI_Task.SizeString = "XXXXXXXXXXX";
            // 
            // button_Run
            // 
            this.button_Run.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_Run.Label = "Run";
            this.button_Run.Name = "button_Run";
            this.button_Run.OfficeImageId = "AnimationStartDropdown";
            this.button_Run.ShowImage = true;
            this.button_Run.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_Run_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.button_ConfigEnviornmentPath);
            this.group2.Items.Add(this.editBox_PythonVersion);
            this.group2.Items.Add(this.dropDown_pythonEnvironment);
            this.group2.Items.Add(this.editBox_environmentVersion);
            this.group2.Items.Add(this.dropDown_pythonDirectory);
            this.group2.Items.Add(this.dropDown_pythonSubDirectory);
            this.group2.Items.Add(this.dropDown_pythonScripts);
            this.group2.Items.Add(this.button_pythonrun);
            this.group2.Label = "Python";
            this.group2.Name = "group2";
            // 
            // button_ConfigEnviornmentPath
            // 
            this.button_ConfigEnviornmentPath.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_ConfigEnviornmentPath.Label = "Set Environment Path";
            this.button_ConfigEnviornmentPath.Name = "button_ConfigEnviornmentPath";
            this.button_ConfigEnviornmentPath.OfficeImageId = "AddInManager";
            this.button_ConfigEnviornmentPath.ShowImage = true;
            this.button_ConfigEnviornmentPath.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_ConfigEnviornmentPath_Click);
            // 
            // editBox_PythonVersion
            // 
            this.editBox_PythonVersion.Label = "Base Version";
            this.editBox_PythonVersion.MaxLength = 500;
            this.editBox_PythonVersion.Name = "editBox_PythonVersion";
            this.editBox_PythonVersion.SizeString = "XXXXXXXXXXXX";
            this.editBox_PythonVersion.Text = null;
            // 
            // dropDown_pythonEnvironment
            // 
            this.dropDown_pythonEnvironment.Label = "Environment";
            this.dropDown_pythonEnvironment.Name = "dropDown_pythonEnvironment";
            this.dropDown_pythonEnvironment.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDown_pythonEnvironment_SelectionChanged);
            // 
            // editBox_environmentVersion
            // 
            this.editBox_environmentVersion.Label = "Version";
            this.editBox_environmentVersion.MaxLength = 500;
            this.editBox_environmentVersion.Name = "editBox_environmentVersion";
            this.editBox_environmentVersion.SizeString = "XXXXXXXXXXXX";
            this.editBox_environmentVersion.Text = null;
            // 
            // dropDown_pythonDirectory
            // 
            this.dropDown_pythonDirectory.Label = "Directory";
            this.dropDown_pythonDirectory.Name = "dropDown_pythonDirectory";
            this.dropDown_pythonDirectory.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDown_pythonDirectory_SelectionChanged);
            // 
            // dropDown_pythonSubDirectory
            // 
            this.dropDown_pythonSubDirectory.Label = "Sub Directory";
            this.dropDown_pythonSubDirectory.Name = "dropDown_pythonSubDirectory";
            this.dropDown_pythonSubDirectory.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDown_pythonProject_SelectionChanged);
            // 
            // dropDown_pythonScripts
            // 
            this.dropDown_pythonScripts.Label = "Script";
            this.dropDown_pythonScripts.Name = "dropDown_pythonScripts";
            // 
            // button_pythonrun
            // 
            this.button_pythonrun.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_pythonrun.Label = "Run Script";
            this.button_pythonrun.Name = "button_pythonrun";
            this.button_pythonrun.OfficeImageId = "AnimationStartDropdown";
            this.button_pythonrun.ShowImage = true;
            this.button_pythonrun.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_pythonrun_Click);
            // 
            // ToolbarRibbon
            // 
            this.Name = "ToolbarRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Compose, Microsoft.Outlook.Mai" +
    "l.Read";
            this.Tabs.Add(this.tab_AIAddin);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ToolbarRibbon_Load);
            this.tab_AIAddin.ResumeLayout(false);
            this.tab_AIAddin.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab_AIAddin;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_Run;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_LMModel;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_modelSource;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox editBox_PythonVersion;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_pythonEnvironment;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_pythonDirectory;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_pythonrun;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox editBox_environmentVersion;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_pythonScripts;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_pythonSubDirectory;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDown_AI_Task;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_ConfigEnviornmentPath;
    }

    partial class ThisRibbonCollection
    {
        internal ToolbarRibbon ToolbarRibbon
        {
            get { return this.GetRibbon<ToolbarRibbon>(); }
        }
    }
}
