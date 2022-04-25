using Editor.LexicalAnalysis;
using Editor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor.Forms
{
    public partial class WorkspaceForm : Form
    {
        public WorkspaceForm()
        {
            InitializeComponent();

            List<Token> lexerTest = Tokenizer.Tokenize(Resources.sampleSource2);

            int index = 0;

            foreach (Token token in lexerTest)
            {
                int nextIndex = index + token.SourceLength;

                editAreaRichTextBox.SelectionColor = GetForegroundColor(token.TokenType);
                editAreaRichTextBox.SelectionBackColor = GetBackgroundColor(token.TokenType);

                editAreaRichTextBox.AppendText(Resources.sampleSource2[index..nextIndex]);

                index = nextIndex;
            }

        }

        private Color GetForegroundColor(TokenTypes tokenType)
        {
            return tokenType switch
            {
                TokenTypes.Keyword => Color.Blue,
                TokenTypes.CharacterLiteral => Color.Yellow,
                TokenTypes.Comment => Color.Green,
                TokenTypes.StringLiteral => Color.Orange,
                _ => Color.Black
            };
        }

        private Color GetBackgroundColor(TokenTypes tokenType)
        {
            return tokenType switch
            {
                _ => Color.White
            };
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
