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

        public void RunFileInTerminal(string fullPath, TextBox textBox)
        {
            string comandaRulare = $"{OUTPUT_FILE_NAME}.exe";
            string comandaAsteptareTastatura = "echo. && echo. && echo Process returned status code %errorlevel% && set /p stat =\"Press any key to exit...\"";

            string? errors = BuildFile(fullPath);
            if (errors != null)
            {
                textBox.Text = errors;
                return;
            }
            textBox.Text = "Build succeeded";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = $"/C {comandaRulare} & {comandaAsteptareTastatura}",
                }
            };

            process.Start();
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
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();

            string errors = process.StandardError.ReadToEnd();
            return errors != "" ? errors : null;
        }
    }
}
