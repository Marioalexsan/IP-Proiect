using Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Framework.MVP;
public interface IModel
{
    public MVPManager Manager { get; set; }

    public Project? Project { get; set; }
}
