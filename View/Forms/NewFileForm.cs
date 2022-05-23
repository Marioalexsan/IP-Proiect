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
public partial class NewFileForm : Form
{
    public CreateFileArgs? Result { get; private set; }

    public NewFileForm()
    {
        InitializeComponent();
    }

    private void OpenPath_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();

        DialogResult result = fbd.ShowDialog();

        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
            return;

        Debug.WriteLine("new file path---:", fbd.SelectedPath);
        filePathTextBox.Text = fbd.SelectedPath;
    }

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
