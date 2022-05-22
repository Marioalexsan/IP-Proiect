using Editor.Forms;
using Editor.LexicalAnalysis;
using Editor.Properties;
using System;
using System.Threading;
using System.Windows.Forms;

namespace NaNtaEditor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Form splashScreen = new SplashScreenForm();
            new Thread(() => splashScreen.ShowDialog()).Start();

            Thread.Sleep(500);

            splashScreen.Invoke(() => splashScreen.Close());
            Form workspaceForm = new WorkspaceForm();

            workspaceForm.ShowDialog();
        }
    }
}