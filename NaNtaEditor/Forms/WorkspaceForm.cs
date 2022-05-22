using Editor.LexicalAnalysis;
using Editor.Properties;
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
using Editor.XMLParser;
using Editor.TabsHandler;

namespace Editor.Forms
{
    public partial class WorkspaceForm : Form
    {
        XMLParserMaster xmlParses_ = new XMLParserMaster();
        TabsHandeler tabs_ = new TabsHandeler();

        NewProjectEntity? newProject = null;
        Form? newProjectForm = null;


        public WorkspaceForm()
        {
            InitializeComponent();

            List<Token> lexerTest = Tokenizer.Tokenize(Resources.sampleSource2);

            int index = 0;

            foreach (Token token in lexerTest)
            {
                int nextIndex = index + token.Length;

                //editAreaRichTextBox.SelectionColor = GetForegroundColor(token.TokenType);
                //editAreaRichTextBox.SelectionBackColor = GetBackgroundColor(token.TokenType);
                editAreaRichTextBox.SelectionFont = new Font(FontFamily.GenericMonospace, 12, token.IsValid ? FontStyle.Regular : FontStyle.Strikeout);

                Color foreColor = editAreaRichTextBox.SelectionColor = GetForegroundColor(token.TokenType);
                editAreaRichTextBox.SelectionBackColor = Color.FromArgb((byte)~foreColor.R, (byte)~foreColor.G, (byte)~foreColor.B);


                editAreaRichTextBox.AppendText(Resources.sampleSource2[index..nextIndex]);

                Debug.WriteLine(token);

                index = nextIndex;
            }

        }

        private Color GetForegroundColor(TokenTypes tokenType)
        {
            return tokenType switch
            {
                TokenTypes.WhiteSpace => Color.White,
                TokenTypes.Comment => Color.Green,
                TokenTypes.Invalid => Color.White,
                TokenTypes.Unrecognized => Color.White,
                TokenTypes.Keyword => Color.Blue,
                TokenTypes.Identifier => Color.DarkGray,
                TokenTypes.IntegerLiteral => Color.LightBlue,
                TokenTypes.FloatLiteral => Color.DarkViolet,
                TokenTypes.BooleanLiteral => Color.Blue,
                TokenTypes.PointerLiteral => Color.Blue,
                TokenTypes.StringLiteral => Color.LightGreen,
                TokenTypes.CharacterLiteral => Color.LightGreen,
                TokenTypes.Punctuator => Color.Black,
                _ => Color.Black
            };
        }

        private Color GetBackgroundColor(TokenTypes tokenType)
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

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void selectFolderPath(Object? sender,EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if(newProject!= null)
                    {
                        newProject.setPath(fbd.SelectedPath);
                    }
                    else
                    {
                        //TODO throw custom exception
                        Debug.WriteLine("cannot set the path for the new proect");
                    }
                }
            }
        }
        private void createProject_Click(object? sender, EventArgs e)
        {
            Debug.WriteLine("create project");
            if(newProject != null)
            {
                if(newProject.getPath() != "" && newProject.getName() != "")
                {
                    Debug.WriteLine("create new project");

                    string completeXmlTarget = xmlParses_.createNewProject(newProject.getName(), newProject.getPath()); //create new xml at path

                    Debug.WriteLine("complete target:", completeXmlTarget);

                    xmlParses_.clearFilePaths();
                    tabs_.deleteAllTabs(TabPages);

                    xmlParses_.openXML(completeXmlTarget,false);
                    tabs_.insertTabs(TabPages, xmlParses_.getPaths());
                    label1.Text = "Current Project: " + xmlParses_.getProjectName();

                }
                else
                {
                    Debug.WriteLine("Empty field, new project dialog");
                }
            }

            if(newProjectForm != null)
            {
                newProjectForm.Close();
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.newProject = new NewProjectEntity();

            //create new  xml file
            this.newProjectForm = new Form();
            newProjectForm.Size = new Size(530, 220);
            
            Label label = new Label();
            label.Text = "Project Name:";
            label.Size = new Size(110, 50);
            label.Location = new Point(20, 25);
            newProjectForm.Controls.Add(label);

            Label label_1 = new Label();
            label_1.Text = "Location:";
            label_1.Size = new Size(110, 50);
            label_1.Location = new Point(20, 75);
            newProjectForm.Controls.Add(label_1);

            Button button = new Button();
            button.Text = "...";
            button.Size = new Size(40, 30);
            button.Location = new Point(440, 68);
            button.Click += new EventHandler(selectFolderPath);
            newProjectForm.Controls.Add(button);

            TextBox projectTitle= new TextBox();
            projectTitle.Size = new Size(300, 100);
            projectTitle.Location = new Point(130, 20);
            newProjectForm.Controls.Add(projectTitle);
            newProject.setLabelTitle(projectTitle);

            TextBox projectPath = new TextBox();
            projectPath.Size = new Size(300, 100);
            projectPath.Location = new Point(130, 70);
            newProjectForm.Controls.Add(projectPath);
            newProject.setLabelPath(projectPath);

            Button createProject = new Button();
            createProject.Size = new Size(180, 30);
            createProject.Location = new Point(150, 120);
            createProject.Text = "Create Project";
            createProject.Click += new EventHandler(createProject_Click);
            newProjectForm.Controls.Add(createProject);

            newProjectForm.ShowDialog();
        }
        
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editAreaRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            //daca este deja incarcat un proiect, goleste tab-urile
            Debug.WriteLine("TEST NAME:", xmlParses_.getProjectName());

            if(xmlParses_.getProjectName() != "")
            {
                Debug.WriteLine("---------------open-?>open");
                xmlParses_?.clearFilePaths();
                tabs_.deleteAllTabs(TabPages);
            }
            if(xmlParses_ != null)
            {
                xmlParses_.openXML("", true);
                tabs_.insertTabs(TabPages, xmlParses_.getPaths());
                label1.Text = "Current Project: " + xmlParses_.getProjectName();
                //xmlParses_.checkData();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ADD new file or add existing file 
            if (xmlParses_.getProjectName() != "") //adauga file doar daca exista un proiect deschis
            {

            }
            else
            {
                MessageBox.Show("Please open a project first","ceva", MessageBoxButtons.OK);
            }
        }
    }
}
