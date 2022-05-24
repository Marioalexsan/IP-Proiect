/*============================================================
*
* File:     FormView.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the FormView class, which implements the
*           IView interface.
*
===========================================================*/

using View.Forms;
using Framework.MVP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presenter;
using View.XMLParsing;

namespace View;
public class FormView : IView
{
    private IPresenter? _presenter;
    public IPresenter Presenter
    {
        get => _presenter ?? throw new Exception("Presenter has not been set.");
        set => _presenter = value;
    }

    private WorkspaceForm? _workspaceForm;
    private WorkspaceForm WorkspaceForm
    {
        get => _workspaceForm ?? throw new Exception("Workspace form has not been created.");
        set => _workspaceForm = value;
    }

    public void Start()
    {
        var splashScreen = new SplashScreenForm();
        new Thread(() => splashScreen.ShowDialog()).Start();

        Thread.Sleep(1000);

        splashScreen.Invoke(() => splashScreen.Close());

        WorkspaceForm = new WorkspaceForm();
        WorkspaceForm.OnCreateProject += Presenter.CreateProject;
        WorkspaceForm.OnOpenProject += Presenter.OpenProject;
        WorkspaceForm.OnCreateFile += Presenter.CreateFile;

        WorkspaceForm.OnBuildProject += Presenter.BuildProject;
        WorkspaceForm.OnRunProject += Presenter.RunProject;

        WorkspaceForm.OnSave += Presenter.Save;

        Presenter.ProjectUpdated += WorkspaceForm.UpdateProjectData;

        try
        {
            WorkspaceForm.ShowDialog();
        }
        catch (Exception e)
        {
            File.WriteAllText("log.txt", e.ToString());
            MessageBox.Show("A fatal exception occurred!\nThe error log has been written to log.txt.\n\n" + e);
        }
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
