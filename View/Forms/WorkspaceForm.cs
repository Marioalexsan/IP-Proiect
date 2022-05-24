/*============================================================
*
* File:     WorkspaceForm.cs 
* Methods:  NewProject_Click, OpenProject_Click, NewFile_Click,Undo_Click, Redo_Click, UndoRedoHandler
* Authors:  Damian Gabriel-Mihai
* Purpose:  Main user interface
*
===========================================================*/

using Framework.Data;
using Framework.LexicalAnalysis;
using Framework.MVP;
using System.Diagnostics;
using System.Runtime.InteropServices;
using View.XMLParsing;

namespace View.Forms
{
    public partial class WorkspaceForm : Form
    {
        public event EventHandler<CreateProjectArgs>? OnCreateProject;
        public event EventHandler<OpenProjectArgs>? OnOpenProject;
        public event EventHandler<CreateFileArgs>? OnCreateFile;
        public event EventHandler? OnSave;

        public Dictionary<RichTextBox, FileInstance> InstanceMapping { get; } = new();
        public Memento memento = new Framework.Data.Memento();

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

        public void UpdateProjectData(object? sender, Project? project)
        {
            addFileToolStripMenuItem.Enabled = project != null;

            if (project == null)
                return;

            // Set current project
            label1.Text = $"Current Project: {project.ProjectTitle}";

            // Delete all tabs
            foreach (var mapping in InstanceMapping)
            {
                mapping.Key.TextChanged -= SourceCodeChanged;
            }
            InstanceMapping.Clear();

            fileTabControl.TabPages.Clear();

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

                myTabPage.Controls.Add(richTextBox1);

                fileTabControl.TabPages.Add(myTabPage);
            }

            SaveNow();

            //memento
            List<FileInstance> filesContent = new List<FileInstance>();
            foreach (RichTextBox a in InstanceMapping.Keys)
            {
                filesContent.Add(InstanceMapping[a]);
            }
            memento.InitializeHistory(filesContent);
            memento.CheckFilesHistory();
            memento.CheckCurrentIndexes();
        
        
        }

        private void SourceCodeChanged(object? sender, EventArgs e)
        {
            if (sender is not RichTextBox box)
                return;

            if (InstanceMapping.TryGetValue(box, out var file))
            {
                file.Contents = box.Text;

                Debug.WriteLine(file);
                Debug.WriteLine(box);
                Debug.WriteLine("update in file:" + file.FilePath);
                Debug.WriteLine("new value:" + box.Text);
                memento.TabContentUpdated(file, box.Text);
            }

            QueueSaving();
        }

        private void EditTimer_Tick(object sender, EventArgs e)
        {
            editTimer.Stop();
            editTimer.Interval = 1000;

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

        [DllImport("User32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0x0b;
        /// <summary>
        /// Callback to the undo button
        /// </summary>
        private void Undo_Click(object sender, EventArgs e)
        {
            UndoRedoHandler("undo");
        }
        /// <summary>
        /// Callback to the redo button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redo_Click(object sender, EventArgs e)
        {
            UndoRedoHandler("redo");
        }
        /// <summary>
        /// Manages how actions are parsed to the Memento Entity and how the Rich Text Boxes are updated corresponding with the user's actions
        /// </summary>
        private void UndoRedoHandler(string who)
        {
            Debug.WriteLine("undo");
            foreach (RichTextBox K in InstanceMapping.Keys)
            {
                if (fileTabControl.SelectedTab.Controls.Contains(K))
                {
                    Debug.WriteLine("tab founded");
                    Debug.WriteLine("FileInstance aferenmt");
                    Debug.WriteLine(InstanceMapping[K]);
                    string undoValue = "";
                    if(who == "undo")
                    {
                        undoValue = memento.UndoTab(InstanceMapping[K]);

                    }
                    else
                    {
                        undoValue = memento.RedoTab(InstanceMapping[K]);

                    }
                    Debug.WriteLine("UNDO VALUE:" + undoValue);

                    foreach (var mapping in InstanceMapping)
                    {
                        mapping.Key.TextChanged -= SourceCodeChanged;
                    }

                    K.Text = undoValue;

                    foreach (var mapping in InstanceMapping)
                    {
                        mapping.Key.TextChanged += SourceCodeChanged;
                    }


                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void WorkspaceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
