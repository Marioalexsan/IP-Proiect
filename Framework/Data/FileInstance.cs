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

        Contents = "";
    }

    public string FilePath { get; }

    public string Contents { get; set; }
}
