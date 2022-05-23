using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data;
public class FileInstance
{
    public FileInstance(string filePath)
    {
        FilePath = filePath;

        Contents = new StringBuilder();
    }

    public string FilePath { get; }

    public StringBuilder Contents { get; }
}
