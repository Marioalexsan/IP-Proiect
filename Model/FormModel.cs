/*============================================================
*
* File:     FormModel.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the FormModel class, which implements the
*           IModel interface.
*
===========================================================*/

using Framework.Data;
using Framework.MVP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Model;

/// <summary>
/// Concrete implementation of the IModel interface.
/// </summary>
public class FormModel : IModel
{
    public Project? Project { get; set; }
}
