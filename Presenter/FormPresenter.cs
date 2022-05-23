using Framework.MVP;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Presenter;
public class FormPresenter : IPresenter
{
    private IModel? _model;
    public IModel Model
    {
        get => _model ?? throw new Exception("Model has not been set.");
        set => _model = value;
    }

    private IView? _view;
    public IView View
    {
        get => _view ?? throw new Exception("View has not been set.");
        set => _view = value;
    }

    public event EventHandler<Project?>? ProjectUpdated;

    public void OpenProject(object? sender, OpenProjectArgs e)
    {
        Model.Project?.SaveXML();
        Model.Project = new Project()
        {
            FilePath = e.FilePath
        };
        Model.Project.LoadFromXML(e.FilePath);
        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void CreateProject(object? sender, CreateProjectArgs e)
    {
        Model.Project?.SaveXML();
        Model.Project = new Project()
        {
            ProjectTitle = e.Name,
            FilePath = Path.Combine(e.FolderPath, e.Name)
        };
        Model.Project.SaveXML();

        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void OpenFile(object? sender, OpenFileArgs e)
    {

    }

    public void CreateFile(object? sender, CreateFileArgs e)
    {
        if (Model.Project == null)
            return;

        Model.Project.AddFileToProject(e.Name, e.FolderPath);

        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void Save(object? sender, EventArgs e)
    {
        Model.Project?.SaveXML();
    }
}
