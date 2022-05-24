/*============================================================
*
* File:     IModel.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the MVPManager helper class that starts the
*           Model-View-Presenter application.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MVP;

/// <summary>
/// A helper class for use with MVP.
/// </summary>
public class MVPManager
{
    /// <summary>
    /// Constructs a manager using each of the MVP components.
    /// </summary>
    public MVPManager(IModel model, IView view, IPresenter presenter)
    {
        Model = model;
        View = view;
        Presenter = presenter;
    }

    /// <summary>
    /// The model to use.
    /// </summary>
    public IModel Model { get; }

    /// <summary>
    /// The view to use.
    /// </summary>
    public IView View { get; }

    /// <summary>
    /// The presenter to use.
    /// </summary>
    public IPresenter Presenter { get; }

    /// <summary>
    /// Starts the application using provided components.
    /// </summary>
    public void Start()
    {
        View.Presenter = Presenter;

        Presenter.Model = Model;
        Presenter.View = View;

        View.Start();
    }
}
