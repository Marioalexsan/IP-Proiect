/*============================================================
*
* File:     FileInstance.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  Defines an entity that manages the process of creating a new file
*
===========================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data;
/// <summary>
/// Retains data, as path and content, about the file to be created
/// </summary>
public class FileInstance
{
    public FileInstance(string filePath)
    {
        FilePath = filePath;

        Contents = "";
    }

    public string FilePath { get; }

    public string Contents { get; set; }
}
