/*============================================================
*
* File:     EventArgs.cs
* Authors:  Miron Alexandru
* Purpose:  Defines data sturctures used as event arguments throughout
*           the application.
*
===========================================================*/

namespace Framework.MVP;

public class CreateFileArgs
{
    public string Name = "";
    public string FolderPath = "";
}

public class CreateProjectArgs
{
    public string Name = "";
    public string FolderPath = "";
}

public class OpenFileArgs
{
    public string FilePath = "";
}

public class OpenProjectArgs
{
    public string FilePath = "";
}