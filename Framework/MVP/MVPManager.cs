using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MVP;
public class MVPManager
{
    public MVPManager(IModel model, IView view, IPresenter presenter)
    {
        Model = model;
        View = view;
        Presenter = presenter;
    }

    public IModel Model { get; }

    public IView View { get; }

    public IPresenter Presenter { get; }

    public void Start()
    {
        View.Presenter = Presenter;

        Presenter.Model = Model;
        Presenter.View = View;

        View.Start();
    }
}
