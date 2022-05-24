/*============================================================
*
* File:     IView.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the IView interface for the Model-View-Presenter
*           architecture.
*
===========================================================*/

using Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Framework.MVP;

/// <summary>
/// Defines an interface for views in the Model-View-Presenter architecture.
/// </summary>
public interface IView
{
    /// <summary>
    /// Gets or sets the presenter instance to use.
    /// </summary>
    public IPresenter Presenter { get; set; }

    /// <summary>
    /// Starts the IView instance.
    /// This acts as the entry point for the MVP architecture.
    /// </summary>
    public void Start();

    /// <summary>
    /// Displays messages to the user.
    /// This can be used to display information or error reports.
    /// </summary>
    public void ShowMessage(string message);
}
