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
    public class CompileOptions
    {
        public IEnumerable<string> AbsoluteFilePaths { get; set; } = new List<string>();
        public string CommandLineArguments { get; set; } = string.Empty;
        public string OutputName { get; set; } = "main";
    }

    public class Compiler
    {
        private const string COMPILER_NAME = "g++";
        string comandaAsteptareTastatura = "echo. && echo. && echo Process returned status code %errorlevel% && set /p stat =\"Press any key to exit...\"";

        public string StatusText { get; set; } = "";

        public void RunFileInTerminal(CompileOptions options)
        {
            List<string> filesForCompilation = options.AbsoluteFilePaths.Where(path => !path.EndsWith(".h")).ToList();
            string compileArgumnets = $"{options.CommandLineArguments} -o {options.OutputName}";

            string? errors = BuildFiles(filesForCompilation, compileArgumnets);
            if (errors != null)
            {
                StatusText = errors;
                return;
            }

            StatusText = "Build succeeded";
            string comandaRulare = $"{options.OutputName}.exe";

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

        public string? BuildFiles(IEnumerable<string> fullFilePaths, string compileArguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = COMPILER_NAME,
                    Arguments = $"{string.Join(" ", fullFilePaths)} {compileArguments}",
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
