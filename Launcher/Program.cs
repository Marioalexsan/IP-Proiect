using View.Forms;
using System;
using System.Threading;
using System.Windows.Forms;
using View;
using Framework.MVP;
using Model;
using Presenter;

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