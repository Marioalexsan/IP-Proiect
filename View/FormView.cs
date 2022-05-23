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

        WorkspaceForm.OnSave += Presenter.Save;

        Presenter.ProjectUpdated += WorkspaceForm.UpdateProjectData;

        WorkspaceForm.ShowDialog();
    }
}
