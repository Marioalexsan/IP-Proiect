/*============================================================
*
* File:     IModel.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the IModel interface for the Model-View-Presenter
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
public interface IModel
{
    public Project? Project { get; set; }
}
