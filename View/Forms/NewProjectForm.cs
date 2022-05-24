/*============================================================
*
* File:     NewPojectForm.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  User interface that makes it easy to create a new project
*
===========================================================*/
using Framework.MVP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View.Forms;
/// <summary>
/// Manages the process of creating a new project
/// </summary>
public partial class NewProjectForm : Form
{
    public CreateProjectArgs? Result { get; private set; }

    public NewProjectForm()
    {
        InitializeComponent();
    }
    /// <summary>
    /// Get the location where the new project xml config file will be stored.
    /// </summary>
    private void OpenPath_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();

        DialogResult result = fbd.ShowDialog();

        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
            return;

        Debug.WriteLine("new file path---:", fbd.SelectedPath);
        projectPathTextBox.Text = fbd.SelectedPath;
    }
    
    /// <summary>
    /// Callback to the create new project button.
    /// </summary>
    private void CreateProject_Click(object sender, EventArgs e)
    {
        Result = new CreateProjectArgs()
        {
            Name = projectNameTextBox.Text,
            FolderPath = projectPathTextBox.Text
        };

        Close();
    }
}
