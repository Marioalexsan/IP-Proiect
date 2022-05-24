/*============================================================
*
* File:     NewFileForm.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  A dialog that queries the user for information about
*           new files that should be added to the project.
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
/// Manages the process of creating a new file
/// </summary>
public partial class NewFileForm : Form
{
    public CreateFileArgs? Result { get; private set; }

    public NewFileForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Get the location where the new file will be stored.
    /// </summary>
    private void OpenPath_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();

        DialogResult result = fbd.ShowDialog();

        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
            return;

        Debug.WriteLine("new file path---:", fbd.SelectedPath);
        filePathTextBox.Text = fbd.SelectedPath;
    }
    /// <summary>
    /// Callback to the create new file button.
    /// </summary>
    private void CreateFile_Click(object sender, EventArgs e)
    {
        Result = new CreateFileArgs()
        {
            Name = fileNameTextBox.Text,
            FolderPath = filePathTextBox.Text
        };

        Close();
    }
}
