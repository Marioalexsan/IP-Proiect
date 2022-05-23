using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View.XMLParsing;
using System.Threading;
using Framework.LexicalAnalysis;
using Framework.MVP;
using View.Forms;

namespace View.Forms
{
    public partial class WorkspaceForm : Form
    {
        public event EventHandler<CreateProjectArgs>? OnCreateProject;
        public event EventHandler<OpenProjectArgs>? OnOpenProject;
        public event EventHandler<CreateFileArgs>? OnCreateFile;

        public TabPage? SelectedPage { get; set; }

        public WorkspaceForm()
        {
            InitializeComponent();
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            var form = new NewProjectForm();

            form.ShowDialog();

            if (form.Result == null)
                return;

            OnCreateProject?.Invoke(this, form.Result);
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            DialogResult result = ofd.ShowDialog();

            if (result != DialogResult.OK)
                return;

            OnOpenProject?.Invoke(this, new OpenProjectArgs()
            {
                FilePath = ofd.FileName
            });
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            var form = new NewFileForm();

            form.ShowDialog();

            if (form.Result == null)
                return;

            OnCreateFile?.Invoke(this, form.Result);
        }

        private void ExitIDE_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void UpdateProjectData(object? sender, Project? e)
        {
            if (e == null)
                return;

            // Set current project
            label1.Text = $"Current Project: {e.ProjectTitle}";

            // Delete all tabs
            fileTabControl.TabPages.Clear();

            var paths = e.Files;

            // Insert new tabs
            for (int i = 0; i < paths.Count; i++)
            {
                string title = $"TabPage {fileTabControl.TabCount + 1}";
                string[] test = paths[i].FilePath.Split("\\");

                string text = "Cannot Read File";

                try
                {
                    text = File.ReadAllText(paths[i].FilePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                RichTextBox richTextBox1 = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Text = text
                };

                TabPage myTabPage = new TabPage(title)
                {
                    Text = test[^1]
                };

                myTabPage.Controls.Add(richTextBox1);

                fileTabControl.TabPages.Add(myTabPage);
            }
        }
    }
}
