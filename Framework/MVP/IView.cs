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

public interface IView
{
    public IPresenter Presenter { get; set; }

    public void Start();

    public void ShowMessage(string message);
}
