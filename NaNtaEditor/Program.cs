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
            var lexerTest = Tokenizer.Tokenize(Resources.sampleSource2);


            ApplicationConfiguration.Initialize();

            Form splashScreen = new SplashScreenForm();
            new Thread(() => splashScreen.ShowDialog()).Start();

            Thread.Sleep(2000);

            splashScreen.Invoke(() => splashScreen.Close());
            Form workspaceForm = new WorkspaceForm();

            workspaceForm.ShowDialog();
        }
    }
}