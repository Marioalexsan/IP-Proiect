using Framework.Data;
using Framework.MVP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Model;
public class FormModel : IModel
{
    private MVPManager? _manager;
    public MVPManager Manager
    {
        get => _manager ?? throw new Exception("Manager has not been set.");
        set => _manager = value;
    }

    public Project? Project { get; set; }
}
