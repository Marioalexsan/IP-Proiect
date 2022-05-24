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

namespace Framework.Compilation;

/// <summary>
/// Compiles C++ projects using the gcc compiler.
/// </summary>
public class Compiler
{
    private const string COMPILER_NAME = "g++";
    private const string WAIT_TERMINAL = "echo. && echo. && echo Process returned status code %errorlevel% && set /p stat =\"Press any key to exit...\"";

    /// <summary>
    /// The status text of the last operation (build or run).
    /// </summary>
    public string StatusText { get; private set; } = "";

    /// <summary>
    /// The success state of the last operation (build or run).
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// Gets or sets the working directory of the compiler.
    /// Usually this should be set to the project directory.
    /// </summary>
    public string WorkingDirectory { get; set; } = "";

    /// <summary>
    /// Compiles the project and runs it in a separate process.
    /// The compilation option is skipped if the executable exists in the working directory.
    /// </summary>
    public bool RunFileInTerminal(CompileOptions options)
    {
        string comandaRulare = $"{options.OutputName}.exe";

        if (!File.Exists(comandaRulare) && !BuildFiles(options))
        {
            return Success = false;
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = WorkingDirectory != "" ? WorkingDirectory : Directory.GetCurrentDirectory(),
                FileName = "CMD.exe",
                Arguments = $"/C {comandaRulare} & {WAIT_TERMINAL}",
            }
        };
        
        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            StatusText = "Failed to run program. Exception:\n" + e;
            return Success = false;
        }

        StatusText = "Run suceeded";
        return Success = true;
    }

    /// <summary>
    /// Builds the project and produces an executable file.
    /// </summary>
    public bool BuildFiles(CompileOptions options)
    {
        string filesToCompile = string.Join(" " ,options.AbsoluteFilePaths);
        string compileArguments = $"{options.CommandLineArguments} -o {options.OutputName}";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = WorkingDirectory != "" ? WorkingDirectory : Directory.GetCurrentDirectory(),
                FileName = COMPILER_NAME,
                Arguments = $"{filesToCompile} {compileArguments}",
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            StatusText = "Failed to run compiler. Exception:\n" + e;
            return Success = false;
        }

        process.WaitForExit();

        string errors = process.StandardError.ReadToEnd();

        StatusText = errors != "" ? "Build failed: \n" + errors : "Build suceeded";

        return Success = !errors.Contains("error");
    }
}
