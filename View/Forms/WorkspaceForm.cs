/*============================================================
*
* File:     WorkspaceForm.cs 
* Authors:  Damian Gabriel-Mihai, Țuțuianu Robert, Florea Alexandru-Daniel,
*           Miron Alexandru
* Purpose:  Defines the main interface of the IDE.
*
===========================================================*/

using Framework.Commands;
using Framework.Data;
using Framework.LexicalAnalysis;
using Framework.MVP;
using System.Diagnostics;
using System.Runtime.InteropServices;
using View.Commands;
using View.XMLParsing;

namespace View.Forms
{
    public partial class WorkspaceForm : Form
    {
        public event EventHandler<CreateProjectArgs>? OnCreateProject;
        public event EventHandler<OpenProjectArgs>? OnOpenProject;
        public event EventHandler<CreateFileArgs>? OnCreateFile;
        public event EventHandler<DeleteFileArgs>? OnDeleteFile;
        public event EventHandler? OnSave;

        public event EventHandler? OnBuildProject;
        public event EventHandler? OnRunProject;
        public event EventHandler? OnCloseProject;

        public Dictionary<RichTextBox, FileInstance> InstanceMapping { get; } = new();

        public Dictionary<RichTextBox, string> PreviousText { get; } = new Dictionary<RichTextBox, string>();

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

        private void DeleteFile_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this file?", "Delete File", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            foreach ((var box, var file) in InstanceMapping)
            {
                if (fileTabControl.SelectedTab.Controls.Contains(box))
                {
                    var args = new DeleteFileArgs()
                    {
                        Instance = InstanceMapping[box]
                    };

                    OnDeleteFile?.Invoke(this, args);
                    return;
                }
            }
        }

        private void ExitIDE_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void UpdateProjectData(object? sender, Project? project)
        {
            addFileToolStripMenuItem.Enabled = project != null;
            deleteFileToolStripMenuItem.Enabled = project != null;
            closeProjectToolStripMenuItem.Enabled = project != null;


            // Delete all tabs
            foreach (var mapping in InstanceMapping)
            {
                mapping.Key.TextChanged -= SourceCodeChanged;
            }
            InstanceMapping.Clear();
            PreviousText.Clear();

            fileTabControl.TabPages.Clear();

            label1.Text = $"Current Project: None";

            if (project == null)
                return;

            // Set current project
            label1.Text = $"Current Project: {project.ProjectTitle}";

            // Insert new tabs
            foreach (var file in project.Files)
            {
                string title = $"TabPage {fileTabControl.TabCount + 1}";
                string[] test = file.FilePath.Split("\\");

                string text = "Cannot Read File";

                try
                {
                    text = File.ReadAllText(file.FilePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                RichTextBox richTextBox1 = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Text = text,
                    Font = new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                    SelectionColor = Color.Black,
                    SelectionBackColor = Color.White
                };

                richTextBox1.TextChanged += SourceCodeChanged;

                TabPage myTabPage = new TabPage(title)
                {
                    Text = test[^1]
                };

                InstanceMapping[richTextBox1] = file;
                PreviousText[richTextBox1] = richTextBox1.Text;

                myTabPage.Controls.Add(richTextBox1);

                fileTabControl.TabPages.Add(myTabPage);
            }

            SaveNow();
        }

        private void SourceCodeChanged(object? sender, EventArgs e)
        {
            if (sender is not RichTextBox box)
                return;

            int pointerPos = box.SelectionStart;

            if (InstanceMapping.TryGetValue(box, out var file))
            {
                file.Contents = box.Text;

                int beginLen = 0;
                int endLen = 0;

                string newText = box.Text;
                string oldText = PreviousText[box];

                for (int i = 0; i < newText.Length && i < oldText.Length && i < pointerPos; i++)
                {
                    if (newText[i] == oldText[i])
                    {
                        beginLen++;
                    }
                }

                for (int i = newText.Length - 1, j = oldText.Length - 1; i >= Math.Max(pointerPos, 0) && j >= 0; i--, j--)
                {
                    if (newText[i] == oldText[j])
                    {
                        endLen++;
                    }
                }

                int unmatchedOld = oldText.Length - beginLen - endLen;
                int unmatchedNew = newText.Length - beginLen - endLen;

                if (unmatchedOld < 0)
                {
                    unmatchedNew += -unmatchedOld;
                    beginLen -= -unmatchedOld;
                    unmatchedOld = 0;
                }

                // Try for a Delete Command
                if (unmatchedOld > 0)
                {
                    file.Memento.Record(new DeleteCommand()
                    {
                        Position = beginLen,
                        DeletedString = oldText.Substring(beginLen, unmatchedOld),
                        Target = box
                    });
                }

                // Try for a Write Command
                if (unmatchedNew > 0)
                {
                    file.Memento.Record(new WriteCommand()
                    {
                        Position = beginLen,
                        WrittenString = newText.Substring(beginLen, unmatchedNew),
                        Target = box
                    });
                }
            }

            PreviousText[box] = box.Text;

            QueueSaving();
        }

        private void EditTimer_Tick(object sender, EventArgs e)
        {
            editTimer.Stop();

            Font validFont = new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular);
            Font invalidFont = new Font(FontFamily.GenericMonospace, 12, FontStyle.Strikeout);


            foreach ((var richTextBox, var file) in InstanceMapping)
            {
                richTextBox.TextChanged -= SourceCodeChanged;
                BeginRTFUpdate(richTextBox);

                int length = richTextBox.SelectionLength;
                int start = richTextBox.SelectionStart;
                int firstChar = richTextBox.GetCharIndexFromPosition(new Point(0, 0));


                string input = file.Contents.ToString();

                List<Token> lexerTest = Tokenizer.Tokenize(input);

                int index = 0;

                richTextBox.SelectAll();

                richTextBox.SelectionColor = Color.Black;
                richTextBox.SelectionBackColor = Color.White;
                richTextBox.SelectionFont = validFont;

                foreach (Token token in lexerTest)
                {
                    richTextBox.Select(index, token.Length);

                    richTextBox.SelectionColor = GetForegroundColor(token.TokenType);
                    richTextBox.SelectionBackColor = GetBackgroundColor(token.TokenType);
                    richTextBox.SelectionFont = token.IsValid ? validFont : invalidFont;

                    index += token.Length;
                }

                richTextBox.SelectionStart = 0;
                richTextBox.SelectionLength = 0;

                richTextBox.SelectionColor = Color.Black;
                richTextBox.SelectionBackColor = Color.White;
                richTextBox.SelectionFont = validFont;

                richTextBox.SelectionStart = start;
                richTextBox.SelectionLength = length;

                EndRTFUpdate(richTextBox);
                richTextBox.TextChanged += SourceCodeChanged;
            }

            // Attempt to save project
            OnSave?.Invoke(this, new EventArgs());
        }

        private static Color GetForegroundColor(TokenTypes tokenType)
        {
            return tokenType switch
            {
                TokenTypes.WhiteSpace => Color.Black,
                TokenTypes.Comment => Color.Green,
                TokenTypes.Invalid => Color.White,
                TokenTypes.Unrecognized => Color.White,
                TokenTypes.Keyword => Color.Blue,
                TokenTypes.Identifier => Color.DarkGray,
                TokenTypes.IntegerLiteral => Color.DeepSkyBlue,
                TokenTypes.FloatLiteral => Color.DarkViolet,
                TokenTypes.BooleanLiteral => Color.Blue,
                TokenTypes.PointerLiteral => Color.Blue,
                TokenTypes.StringLiteral => Color.LightGreen,
                TokenTypes.CharacterLiteral => Color.LightGreen,
                TokenTypes.Punctuator => Color.Black,
                _ => Color.Black
            };
        }

        private static Color GetBackgroundColor(TokenTypes tokenType)
        {
            return tokenType switch
            {
                TokenTypes.WhiteSpace => Color.White,
                TokenTypes.Comment => Color.White,
                TokenTypes.Invalid => Color.Red,
                TokenTypes.Unrecognized => Color.Red,
                TokenTypes.Keyword => Color.White,
                TokenTypes.Identifier => Color.White,
                TokenTypes.IntegerLiteral => Color.White,
                TokenTypes.FloatLiteral => Color.White,
                TokenTypes.BooleanLiteral => Color.White,
                TokenTypes.PointerLiteral => Color.White,
                TokenTypes.StringLiteral => Color.White,
                TokenTypes.CharacterLiteral => Color.White,
                TokenTypes.Punctuator => Color.White,
                _ => Color.White
            };
        }

        public void SaveNow()
        {
            editTimer.Stop();
            editTimer.Interval = 1;
            editTimer.Start();
        }

        public void QueueSaving()
        {
            editTimer.Stop();
            editTimer.Interval = 1000;
            editTimer.Start();
        }

        /* Win32 Native stuff */

        private void BeginRTFUpdate(RichTextBox box)
        {
            SendMessage(box.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        }

        private void EndRTFUpdate(RichTextBox box)
        {
            SendMessage(box.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            box.Invalidate();
        }

        /// <summary>
        /// Callback to the undo button
        /// </summary>
        private void Undo_Click(object sender, EventArgs e)
        {
            foreach ((var box, var file) in InstanceMapping)
            {
                if (!fileTabControl.SelectedTab.Controls.Contains(box))
                    continue;

                IEditorCommand? command = file.Memento.Undo();

                if (command == null)
                    return;

                box.TextChanged -= SourceCodeChanged;

                command.Undo();

                PreviousText[box] = box.Text;

                box.TextChanged += SourceCodeChanged;
            }

            QueueSaving();
        }

        /// <summary>
        /// Callback to the redo button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_Click(object sender, EventArgs e)
        {
            foreach ((var box, var file) in InstanceMapping)
            {
                if (!fileTabControl.SelectedTab.Controls.Contains(box))
                    continue;

                IEditorCommand? command = file.Memento.Redo();

                if (command == null)
                    return;

                box.TextChanged -= SourceCodeChanged;

                command.Redo();

                PreviousText[box] = box.Text;

                box.TextChanged += SourceCodeChanged;
            }

            QueueSaving();
        }

        private void RunProject_Click(object sender, EventArgs e)
        {
            OnRunProject?.Invoke(this, e);
        }

        private void BuildProject_Click(object sender, EventArgs e)
        {
            OnBuildProject?.Invoke(this, e);
        }

        [DllImport("User32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0x0b;

        private void CloseProject_Click(object sender, EventArgs e)
        {
            SaveNow();
            OnCloseProject?.Invoke(this, e);
        }

        private void Help_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @"Resources/NaNTa.chm");
        }
    }
}
