using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace Editor.Compile
{
    public class Compiler
    {
        private const string COMPILER_NAME = "g++";
        private const string OUTPUT_FILE_NAME = "main";
        private const string COMPILE_ARGUMENTS = $"-o {OUTPUT_FILE_NAME}";

        private TextBox? _textBox;
        private string _undeDeletableText = "";
        private System.IO.StreamWriter? _standardInput;

        public void RunFile(string fullPath, TextBox outputTextBox)
        {
            this._textBox = outputTextBox;

            string? compilation_errors = BuildFile(fullPath);
            if (compilation_errors != null)
            {
                this._textBox.Text += $"{compilation_errors}\r\n";
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = OUTPUT_FILE_NAME,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            };

            process.Start();
            this._standardInput = process.StandardInput;
            var standartOutput = process.StandardOutput;
            var standardError = process.StandardError;

            var taskStdOut = Task.Run(() =>
            {
                while (!standartOutput.EndOfStream)
                {
                    var character = ((char)standartOutput.Read()).ToString();
                    this._undeDeletableText = this._textBox.Text+character;

                    this._textBox.BeginInvoke((MethodInvoker)delegate ()
                    {
                        this._textBox.AppendText(character);
                    });
                }
            });

            var taskStdErr = Task.Run(() =>
            {
                while (!standardError.EndOfStream)
                {
                    var character = ((char)standardError.Read()).ToString();
                    this._undeDeletableText = this._textBox.Text+character;


                    this._textBox.BeginInvoke((MethodInvoker)delegate ()
                    {
                        this._textBox.AppendText(character);
                    });
                }
            });


            this._textBox.TextChanged += new EventHandler(textListener);
            process.Exited += new EventHandler((object? sender, EventArgs e) =>
            {
                string exitMessage = "\r\nProcess finished";
                this._undeDeletableText = this._textBox.Text + exitMessage;
                this._textBox.Text += exitMessage;
            });

        }

        public void textListener(object? sender, EventArgs e)
        {
            if (this._textBox!.Text == this._undeDeletableText)
                return;

            var currentText = this._textBox!.Text;
            var newText = currentText.Substring(this._undeDeletableText.Length, currentText.Length - this._undeDeletableText.Length);

            if (!newText.Contains("\r\n"))
                return;

            foreach(string command in newText.Split("\r\n"))
            {
                this._standardInput!.WriteLine(command);
            }
            this._undeDeletableText += newText;
            
        }

        public static string? BuildFile(string fullPath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = COMPILER_NAME,
                    Arguments = $"{fullPath} {COMPILE_ARGUMENTS}",
                    RedirectStandardError = true,
                }
            };
            process.Start();
            process.WaitForExit();

            string errors = process.StandardError.ReadToEnd();
            return errors != "" ? errors : null;
        }
    }
}
