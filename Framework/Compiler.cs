/*============================================================
*
* File:     Compiler.cs
* Authors:  Țuțuianu Robert
* Purpose:  Compiles C++ files.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Compiler
{
    public class Compiler
    {
        private const string COMPILER_NAME = "g++";
        private const string OUTPUT_FILE_NAME = "main";
        private const string COMPILE_ARGUMENTS = $"-o {OUTPUT_FILE_NAME}";

        public string StatusText { get; set; } = "";

        public void RunFileInTerminal(string fullPath)
        {
            string comandaRulare = $"{OUTPUT_FILE_NAME}.exe";
            string comandaAsteptareTastatura = "echo. && echo. && echo Process returned status code %errorlevel% && set /p stat =\"Press any key to exit...\"";

            string? errors = BuildFile(fullPath);
            if (errors != null)
            {
                StatusText = errors;
                return;
            }
            StatusText = "Build succeeded";

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
