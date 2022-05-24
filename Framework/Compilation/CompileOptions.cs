/*============================================================
*
* File:     CompilerOptions.cs
* Authors:  Țuțuianu Robert
* Purpose:  Compile options for use with Compiler
*
===========================================================*/

namespace Framework.Compilation;

/// <summary>
/// Data class with compile options.
/// </summary>
public class CompileOptions
{
    public List<string> AbsoluteFilePaths { get; set; } = new List<string>();

    public string CommandLineArguments { get; set; } = string.Empty;

    public string OutputName { get; set; } = "main";
}
