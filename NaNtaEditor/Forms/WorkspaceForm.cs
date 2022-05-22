using CppParser.LexicalAnalysis;
using Editor.Properties;
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
