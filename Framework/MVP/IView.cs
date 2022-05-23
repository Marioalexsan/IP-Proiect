using Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Framework.MVP;

public interface IView
{
    public IPresenter Presenter { get; set; }

    public void Start();
}
