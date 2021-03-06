/*============================================================
*
* File:     Program.cs
* Authors:  Miron Alexandru
* Purpose:  Contains the entry point of the application.
*
===========================================================*/

using View.Forms;
using System;
using System.Threading;
using System.Windows.Forms;
using View;
using Framework.MVP;
using Model;
using Presenter;


/// <summary>
/// The entry point of the application.
/// </summary>
namespace NaNtaEditor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            new MVPManager(
                new FormModel(),
                new FormView(),
                new FormPresenter()
                ).Start();
        }
    }
}