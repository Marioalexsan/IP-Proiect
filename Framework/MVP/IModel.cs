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

/// <summary>
/// Defines an interface for models in the Model-View-Presenter architecture.
/// </summary>
public interface IModel
{
    /// <summary>
    /// The current loaded project.
    /// </summary>
    public Project? Project { get; set; }
}
