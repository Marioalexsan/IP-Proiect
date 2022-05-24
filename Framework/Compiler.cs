/*============================================================
*
* File:     Compiler.cs
* Authors:  Țuțuianu Robert
* Purpose:  Compiles C++ files using gcc.
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

        public string WorkingDirectory { get; set; } = "";

        public void RunFileInTerminal(CompileOptions options)
        {
            List<string> filesForCompilation = options.AbsoluteFilePaths.Where(path => !path.EndsWith(".h")).ToList();
            string compileArgumnets = $"{options.CommandLineArguments} -o {options.OutputName}";

            if (!BuildFiles(filesForCompilation, compileArgumnets))
            {
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
            
            if (WorkingDirectory != "")
                process.StartInfo.WorkingDirectory = WorkingDirectory;

            process.Start();
        }

        public bool BuildFiles(IEnumerable<string> fullFilePaths, string compileArguments)
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

            if (WorkingDirectory != "")
                process.StartInfo.WorkingDirectory = WorkingDirectory;

            process.Start();
            process.WaitForExit();

            string errors = process.StandardError.ReadToEnd();

            StatusText = errors != "" ? errors : "Build suceeded";

            return !errors.Contains("error");
        }
    }
}
